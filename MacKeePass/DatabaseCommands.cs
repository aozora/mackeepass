using System;
using System.Collections;
using System.Collections.Generic;
using KeePass.UI;
using KeePassLib;
using KeePassLib.Cryptography;
using KeePassLib.Keys;
using KeePassLib.Serialization;
using KeePassLib.Utility;
using MacKeePass.Models;
using KeePassLib.Collections;
using MacKeePass.Config;
using KeePassLib.Security;



namespace MacKeePass
{
   public class DatabaseCommands
   {
      private PwDatabase db;
      //private object keepassLock = new object ();
      private DateTime m_dtCachedNow;
      private Configuration configuration;


      public PwDatabase Database 
      {
         get 
         { 
            return db; 
         }
      }



      public bool IsOpen
      {
         get
         {
            return db.IsOpen;
         }
      }



      public bool IsNewAndNotSaved
      {
         get
         {
            return db.IOConnectionInfo == null || db.IOConnectionInfo.Path.Length == 0;
         }
      }


      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="database"></param>
      public DatabaseCommands (Configuration configuration)
      {
         this.configuration = configuration;
      }



      public void CloseDatabase ()
      {
         if (db != null) {
            db.Close ();
            db = null;
         }
      }


      #region Open Database

      public void OpenDatabase (string path, string password)
      {
         IOConnectionInfo ioc = IOConnectionInfo.FromPath (path);
         CompositeKey key = CreateCompositeKey (password);

         OpenDatabase (ioc, key);
      }


      /// <summary>
      /// Open a database. This function opens the specified database and updates
      /// the user interface.
      /// </summary>
      public void OpenDatabase (IOConnectionInfo ioc, CompositeKey cmpKey)
      {
         // Close active db
         CloseDatabase ();

         if (db != null && db.IsOpen)
            return;


         //IOConnectionInfo ioc = IOConnectionInfo.FromPath(ConfigurationManager.AppSettings["dbPath"]);

         //if (ioConnection == null)
         //{
         //   if (bOpenLocal)
         //   {
         //      OpenFileDialog ofdDb = UIUtil.CreateOpenFileDialog(KPRes.OpenDatabaseFile,
         //         UIUtil.CreateFileTypeFilter(AppDefs.FileExtension.FileExt,
         //         KPRes.KdbxFiles, true), 1, null, false, false);

         //      //GlobalWindowManager.AddDialog(ofdDb);
         //      //DialogResult dr = ofdDb.ShowDialog();
         //      //GlobalWindowManager.RemoveDialog(ofdDb);
         //      //if (dr != DialogResult.OK) return;

         //      ioc = IOConnectionInfo.FromPath(ofdDb.FileName);
         //   }
         //   else
         //   {
         //      ioc = CompleteConnectionInfo(new IOConnectionInfo(), false,
         //         true, true, true);
         //      if (ioc == null) return;
         //   }
         //}
         //else // ioConnection != null
         //{
         //   ioc = CompleteConnectionInfo(ioConnection, false, true, true, false);
         //   if (ioc == null) return;
         //}

         if (!ioc.CanProbablyAccess ()) {
            // TODO
            //MessageService.ShowWarning(ioc.GetDisplayName(), KPRes.FileNotFoundError);
            //return;
         }

         //if (OpenDatabaseRestoreIfOpened(ioc)) return;

         PwDatabase pwOpenedDb = null;

         if (cmpKey == null) {
            // get the master key
            CompositeKey key = CreateCompositeKey (System.Configuration.ConfigurationManager.AppSettings ["dbKey"]);
            pwOpenedDb = OpenDatabaseInternal (ioc, key);
         }
         else { // cmpKey != null
            pwOpenedDb = OpenDatabaseInternal (ioc, cmpKey);
         }


         //string strName = pwOpenedDb.IOConnectionInfo.GetDisplayName();
         //this.Text = strName;

         //m_mruList.AddItem(strName, pwOpenedDb.IOConnectionInfo.CloneDeep(), true);

         db = pwOpenedDb;
      }



      private PwDatabase OpenDatabaseInternal (IOConnectionInfo ioc, CompositeKey cmpKey)
      {
         PerformSelfTest ();

         //string strPathNrm = ioc.Path.Trim().ToLower();

         PwDatabase pwDb = new PwDatabase ();

         try {
            pwDb.Open (ioc, cmpKey, null);
         }
         catch (Exception ex) {
            //MessageService.ShowLoadWarning(ioc.GetDisplayName(), ex,
            //   (Program.CommandLineArgs[AppDefs.CommandLineOptions.Debug] != null));
            pwDb = null;
            throw;
         }

         return pwDb;
      }

      #endregion

      #region Save Database

      public void SaveDatabase()
      {
         if (db == null)
            return;

         if (!db.IsOpen)
            return;

         db.Save(new DefaultKeePassLogger());

      }



      public void SaveDatabase(string filePath)
      {
         if (db == null)
            return;

         if (!db.IsOpen)
            return;

         IOConnectionInfo ioc = IOConnectionInfo.FromPath (filePath);
         db.SaveAs(ioc, true, new DefaultKeePassLogger());
      }

      #endregion

      #region Database entities management

      public TreeGroupModel FindGroups ()
      {
         TreeGroupModel model = new TreeGroupModel ();
         DateTime m_dtCachedNow = DateTime.Now;
         GroupModel root = null;

         // add root if exists
         if (db.RootGroup != null) {
            int nIconID = ((!db.RootGroup.CustomIconUuid.EqualsValue (PwUuid.Zero)) ?
                                                                                        ((int)PwIcon.Count + db.GetCustomIconIndex (
                                                                                           db.RootGroup.CustomIconUuid)) : (int)db.RootGroup.IconId);

            if (db.RootGroup.Expires && (db.RootGroup.ExpiryTime <= m_dtCachedNow))
               nIconID = (int) PwIcon.Expired;

            root = new GroupModel ()
               {
                  Name = db.RootGroup.Name,
                  ImageIndex = nIconID,
                  SelectedImageIndex = nIconID,
                  Group = db.RootGroup,
                  Expires = db.RootGroup.Expires,
                  ExpiryTime = db.RootGroup.ExpiryTime
               };

            model.Nodes.Add (root);
         }

         RecursiveAddGroup (model, root, db.RootGroup, null);

         return model;
      }



      private void RecursiveAddGroup (TreeGroupModel tree, GroupModel parent, PwGroup pgContainer, PwGroup pgFind)
      {
         if (pgContainer == null)
            return;

         IList<GroupModel> tnc;

         if (parent == null)
            tnc = tree.Nodes;
         else
            tnc = parent.Childs;


         foreach (PwGroup pg in pgContainer.Groups) {
            string strName = pg.Name; // + GetGroupSuffixText(pg);

            int nIconID = ((!pg.CustomIconUuid.EqualsValue (PwUuid.Zero)) ? ((int)PwIcon.Count + db.GetCustomIconIndex (pg.CustomIconUuid)) : (int)pg.IconId);
            bool bExpired = (pg.Expires && (pg.ExpiryTime <= m_dtCachedNow));

            if (bExpired)
               nIconID = (int) PwIcon.Expired;

            GroupModel node = new GroupModel ()
            {
               Name = strName,
               ImageIndex = nIconID,
               SelectedImageIndex = nIconID,
               Group = pg,
               Expires = pg.Expires,
               ExpiryTime = pg.ExpiryTime
            };

            tnc.Add (node);

            RecursiveAddGroup (tree, node, pg, pgFind);
         }
      }



      public IList<EntryModel> FindEntries (GroupModel group)
      {
         IList<EntryModel> model = new List<EntryModel> ();
         PwGroup pg = group.Group;
         bool bSubEntries = false;

         PwObjectList<PwEntry> pwlSource = ((pg != null) ? pg.GetEntries (bSubEntries) : new PwObjectList<PwEntry> ());

         m_dtCachedNow = DateTime.Now;

         if (pg != null) {
            foreach (PwEntry pe in pwlSource) {

               EntryModel entry = new EntryModel ();

               entry.Group = group;
               entry.Entry = pe;

               string strMain = GetEntryFieldEx (pe, ColumnType.Title, true);
               entry.Title = strMain;
            
               if (pe.Expires && (pe.ExpiryTime <= m_dtCachedNow)) {
                  entry.CustomIconId = (int)PwIcon.Expired;
               }

               // get data for the other columns...
               entry.UserName = GetEntryFieldEx (pe, ColumnType.UserName, true);
               entry.Password = GetEntryFieldEx (pe, ColumnType.Password, true);
               entry.Url = GetEntryFieldEx (pe, ColumnType.Url, true);
               entry.Notes = GetEntryFieldEx (pe, ColumnType.Notes, true);

               model.Add (entry);
            }
         }

         return model;
      }



      private string GetEntryFieldEx (PwEntry pe, ColumnType columnType, bool bFormatForDisplay)
      {
         return GetEntryFieldEx(pe, Convert.ToInt32(columnType), bFormatForDisplay);
      }



      private string GetEntryFieldEx (PwEntry pe, int iColumnID, bool bFormatForDisplay)
      {
         IList<EntryColumn> columns = configuration.AvailableColumns;

         if ((iColumnID < 0) || (iColumnID >= columns.Count)) {
            return string.Empty;
         }

         EntryColumn col = columns[iColumnID];
//         if (bFormatForDisplay && col.HideWithAsterisks) {
////            bRequestAsync = false;
//            return PwDefs.HiddenPassword;
//         }

         string str;
         switch (col.ColumnType) 
         {
         case ColumnType.Title:
            str = pe.Strings.ReadSafe (PwDefs.TitleField);
            break;
         case ColumnType.UserName:
            str = pe.Strings.ReadSafe (PwDefs.UserNameField);
            break;
         case ColumnType.Password:
            str = pe.Strings.ReadSafe (PwDefs.PasswordField);
            break;
         case ColumnType.Url:
            str = pe.Strings.ReadSafe (PwDefs.UrlField);
            break;
         case ColumnType.Notes:
            if (!bFormatForDisplay)
               str = pe.Strings.ReadSafe (PwDefs.NotesField);
            else
               str = StrUtil.MultiToSingleLine (pe.Strings.ReadSafe (PwDefs.NotesField));
            break;
         case ColumnType.CreationTime:
            str = TimeUtil.ToDisplayString (pe.CreationTime);
            break;
         case ColumnType.LastAccessTime:
            str = TimeUtil.ToDisplayString (pe.LastAccessTime);
            break;
         case ColumnType.LastModificationTime:
            str = TimeUtil.ToDisplayString (pe.LastModificationTime);
            break;
         case ColumnType.ExpiryTime:
            if (pe.Expires)
               str = TimeUtil.ToDisplayString (pe.ExpiryTime);
            else
               str = ""; //m_strNeverExpiresText;
            break;
         case ColumnType.Uuid:
            str = pe.Uuid.ToHexString ();
            break;
         case ColumnType.Attachment:
            str = pe.Binaries.KeysToString ();
            break;
//         case ColumnType.CustomString:
//            if (!bFormatForDisplay)
//               str = pe.Strings.ReadSafe (col.CustomName);
//            else
//               str = StrUtil.MultiToSingleLine (pe.Strings.ReadSafe (col.CustomName));
//            break;
         //case AceColumnType.PluginExt:
         //   if (!bFormatForDisplay) str = Program.ColumnProviderPool.GetCellData(col.CustomName, pe);
         //   else str = StrUtil.MultiToSingleLine(Program.ColumnProviderPool.GetCellData(col.CustomName, pe));
         //   break;
         case ColumnType.OverrideUrl:
            str = pe.OverrideUrl;
            break;
         case ColumnType.Tags:
            str = StrUtil.TagsToString (pe.Tags, true);
            break;
         case ColumnType.ExpiryTimeDateOnly:
            if (pe.Expires)
               str = TimeUtil.ToDisplayStringDateOnly (pe.ExpiryTime);
            else
               str = ""; //m_strNeverExpiresText;
            break;
         case ColumnType.Size:
            str = StrUtil.FormatDataSizeKB (pe.GetSize ());
            break;
         case ColumnType.HistoryCount:
            str = pe.History.UCount.ToString ();
            break;
         default:
            //Debug.Assert (false);
            str = string.Empty;
            break;
         }

         return str;
      }



      public bool SaveEntry(EntryModel edited, PwEntry m_pwInitialEntry/*, bool bValidate*/)
      {
         const PwCompareOptions m_cmpOpt = (PwCompareOptions.NullEmptyEquivStd | PwCompareOptions.IgnoreTimes);

         PwEntry originalEntry = edited.Entry;
         //peTarget.History = m_vHistory; // Must be called before CreateBackup()

         bool bCreateBackup = !edited.IsNew;

         if(bCreateBackup) 
            originalEntry.CreateBackup(null);

//         peTarget.IconId = m_pwEntryIcon;
//         peTarget.CustomIconUuid = m_pwCustomIconID;
//
//         if(m_cbCustomForegroundColor.Checked)
//            peTarget.ForegroundColor = m_clrForeground;
//         else peTarget.ForegroundColor = Color.Empty;
//         if(m_cbCustomBackgroundColor.Checked)
//            peTarget.BackgroundColor = m_clrBackground;
//         else peTarget.BackgroundColor = Color.Empty;

         //peTarget.OverrideUrl = m_tbOverrideUrl.Text;

//         List<string> vNewTags = StrUtil.StringToTags(m_tbTags.Text);
//         peTarget.Tags.Clear();
//         foreach(string strTag in vNewTags) peTarget.AddTag(strTag);

         originalEntry.Expires = edited.Expires;

         if(originalEntry.Expires) 
            originalEntry.ExpiryTime = edited.ExpireDate;

         UpdateEntryStrings(edited, originalEntry.Strings);

//         peTarget.Strings = m_vStrings;
//         peTarget.Binaries = m_vBinaries;

//         m_atConfig.Enabled = m_cbAutoTypeEnabled.Checked;
//         m_atConfig.ObfuscationOptions = (m_cbAutoTypeObfuscation.Checked ?
//            AutoTypeObfuscationOptions.UseClipboard :
//            AutoTypeObfuscationOptions.None);
//
//         SaveDefaultSeq();

//         peTarget.AutoType = m_atConfig;

         originalEntry.Touch(true, false); // Touch *after* backup

//         if(object.ReferenceEquals(peTarget, m_pwEntry)) 
//            m_bTouchedOnce = true;

         StrUtil.NormalizeNewLines(originalEntry.Strings, true);

         bool bUndoBackup = false;
         PwCompareOptions cmpOpt = m_cmpOpt;

         if(bCreateBackup) 
            cmpOpt |= PwCompareOptions.IgnoreLastBackup;

         if(originalEntry.EqualsEntry(m_pwInitialEntry, cmpOpt, MemProtCmpMode.CustomOnly))
         {
            // No modifications at all => restore last mod time and undo backup
            originalEntry.LastModificationTime = m_pwInitialEntry.LastModificationTime;
            bUndoBackup = bCreateBackup;
         }
         else if(bCreateBackup)
         {
            // If only history items have been modified (deleted) => undo
            // backup, but without restoring the last mod time
            PwCompareOptions cmpOptNH = (m_cmpOpt | PwCompareOptions.IgnoreHistory);
            if(originalEntry.EqualsEntry(m_pwInitialEntry, cmpOptNH, MemProtCmpMode.CustomOnly))
               bUndoBackup = true;
         }

         if(bUndoBackup) 
            originalEntry.History.RemoveAt(originalEntry.History.UCount - 1);

         originalEntry.MaintainBackups(db);

         return true;
      }



      public void UpdateEntryStrings(EntryModel edited, ProtectedStringDictionary m_vStrings /*, bool bGuiToInternal, bool bSetRepeatPw*/)
      {
//         if(bGuiToInternal)
//         {
            m_vStrings.Set(PwDefs.TitleField, new ProtectedString(db.MemoryProtection.ProtectTitle, edited.Title));
            m_vStrings.Set(PwDefs.UserNameField, new ProtectedString(db.MemoryProtection.ProtectUserName, edited.UserName));

            byte[] pb = edited.Password.ToUtf8(); //m_icgPassword.GetPasswordUtf8();
            m_vStrings.Set(PwDefs.PasswordField, new ProtectedString(db.MemoryProtection.ProtectPassword, pb));
            MemUtil.ZeroByteArray(pb);

            m_vStrings.Set(PwDefs.UrlField, new ProtectedString(db.MemoryProtection.ProtectUrl, edited.Url));
            m_vStrings.Set(PwDefs.NotesField, new ProtectedString(db.MemoryProtection.ProtectNotes, edited.Notes));
//         }
//         else // Internal to GUI
//         {
//            m_tbTitle.Text = m_vStrings.ReadSafe(PwDefs.TitleField);
//            m_tbUserName.Text = m_vStrings.ReadSafe(PwDefs.UserNameField);
//
//            byte[] pb = m_vStrings.GetSafe(PwDefs.PasswordField).ReadUtf8();
//            m_icgPassword.SetPassword(pb, bSetRepeatPw);
//            MemUtil.ZeroByteArray(pb);
//
//            m_tbUrl.Text = m_vStrings.ReadSafe(PwDefs.UrlField);
//            m_rtNotes.Text = m_vStrings.ReadSafe(PwDefs.NotesField);
//
//            int iTopVisible = UIUtil.GetTopVisibleItem(m_lvStrings);
//            m_lvStrings.Items.Clear();
//            foreach(KeyValuePair<string, ProtectedString> kvpStr in m_vStrings)
//            {
//               if(!PwDefs.IsStandardField(kvpStr.Key))
//               {
//                  PwIcon pwIcon = (kvpStr.Value.IsProtected ? m_pwObjectProtected :
//                     m_pwObjectPlainText);
//
//                  ListViewItem lvi = m_lvStrings.Items.Add(kvpStr.Key, (int)pwIcon);
//
//                  if(kvpStr.Value.IsProtected)
//                     lvi.SubItems.Add(PwDefs.HiddenPassword);
//                  else
//                  {
//                     string strValue = StrUtil.MultiToSingleLine(
//                        kvpStr.Value.ReadString());
//                     lvi.SubItems.Add(strValue);
//                  }
//               }
//            }
//            UIUtil.SetTopVisibleItem(m_lvStrings, iTopVisible);
         }
      

      #endregion

      #region Helpers

      private CompositeKey CreateCompositeKey (string masterKey)
      {
         CompositeKey m_pKey = new CompositeKey ();

         SecureEdit se = new SecureEdit (masterKey);
         byte[] pb = se.ToUtf8 ();
         m_pKey.AddUserKey (new KcpPassword (pb));
         Array.Clear (pb, 0, pb.Length);

         return m_pKey;
      }

      private static bool PerformSelfTest ()
      {
         //if (m_bCachedSelfTestResult.HasValue)
         //   return m_bCachedSelfTestResult.Value;

         bool bResult = true;
         try {
            SelfTest.Perform ();
         }
         catch (Exception exSelfTest) {
            MessageService.ShowWarning ("KPRes.SelfTestFailed", exSelfTest);
            bResult = false;
         }

         //m_bCachedSelfTestResult = bResult;
         return bResult;
      }

      #endregion

   }
}