using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MacKeePass.Models;
using MacKeePass.UI;
using MacKeePass.Helpers;
using MacKeePass.Config;
using System.Globalization;



namespace MacKeePass
{
   public partial class MainWindowController : MonoMac.AppKit.NSWindowController
   {

      #region Private Fields

      private DatabaseCommands databaseCommands;
      private Configuration configuration;
      private PasswordWindowController passwordWindowController = null;
      private EditWindowController editWindowController = null;
      private NSApplication NSApp = NSApplication.SharedApplication;   

      private bool dbNeedsTobeSaved = false;      

      #endregion

      #region Constructors & Initialize
      
      // Called when created from unmanaged code
      public MainWindowController (IntPtr handle) : base (handle)
      {
         Initialize ();
      }
      
      // Called when created directly from a XIB file
      [Export ("initWithCoder:")]
      public MainWindowController (NSCoder coder) : base (coder)
      {
         Initialize ();
      }
      
      // Call to load from the XIB/NIB file
      public MainWindowController () : base ("MainWindow")
      {
         Initialize ();
      }


      /// <summary>
      /// Initialize this instance.
      /// </summary>
      void Initialize ()
      {
         // set the default settings
         NSUserDefaults userDefaults = NSUserDefaults.StandardUserDefaults;

         NSObject [] values = new NSObject [] {
            FromObject (true),
            FromObject (true),
            FromObject (true),
            FromObject (20),
            FromObject("")
         };
         NSObject [] keys = new NSObject [] {
            FromObject ("openlastdb"), 
            FromObject ("lockonminimize"), 
            FromObject ("clearclipboard"), 
            FromObject ("secondsafterclear"),
            FromObject ("lastdbpath") 
         };
         NSDictionary settings = NSDictionary.FromObjectsAndKeys (values, keys);

         userDefaults.RegisterDefaults(settings);
      }
      
      #endregion
      
      #region strongly typed window accessor

      public new MainWindow Window {
         get {
            return (MainWindow)base.Window;
         }
      }

      #endregion

      #region Public Properties

      public DatabaseCommands DatabaseCommands {
         get {
            return databaseCommands;
         }
      }



      public Configuration Configuration 
      {
         get
         {
            return configuration;
         }
      }

      #endregion

      #region Awake from nib

      /// <summary>
      /// Awakes from nib. (Load)
      /// </summary>
      public override void AwakeFromNib ()
      {
         base.AwakeFromNib ();

         configuration = new Configuration();
         databaseCommands = new DatabaseCommands (configuration);  

         SidebarDelegate sidebarDelegate = new SidebarDelegate(GroupsSourceList);
         GroupsSourceList.Delegate = sidebarDelegate;
         sidebarDelegate.SelectionChanged += sidebarSelectionChanged;

         EntriesTableView.DoubleClick += EntriesTableView_HandleDoubleClick;

         NSUserDefaults userDefaults = NSUserDefaults.StandardUserDefaults;

      }

      #endregion

      #region UI

      #region UI Events

      #region Toolbar Items

      partial void NewDatabaseToolbarClick (NSObject sender)
      {
         Console.WriteLine("NewDatabaseToolbarClick");
      }



      partial void OpenDatabaseToolbarClick (MonoMac.Foundation.NSObject sender)
      {
         MenuOpenClick();
      }


      partial void SaveDatabaseToolbarClick (NSObject sender)
      {
         //Console.WriteLine("SaveDatabaseToolbarClick");
         SaveDatabase ();
      }


      partial void EditEntryToolbarClick (NSObject sender)
      {
         Console.WriteLine("EditEntryToolbarClick");
      }


      partial void CopyPasswordToolbarClick (NSObject sender)
      {
         Console.WriteLine("CopyPasswordToolbarClick");
         CopyToClipboardSelectedItemPassword();
      }



      [Export("validateToolbarItem:")]
      public bool validateToolbarItem(NSToolbarItem toolbarItem)
      {
         //Console.WriteLine("validateToolbarItem: " + toolbarItem.Identifier);
         switch (toolbarItem.Tag) 
         {
            // new
            case 0:
               return true;

            // open
            case 1:
               return true;
            
            // save
            case 2:
               return dbNeedsTobeSaved;

            // edit
            case 3:
               return true;

            // copy password
            case 4:
               bool entryIsSelected = EntriesTableView.SelectedRowCount == 1;
               return entryIsSelected;

            default:
               break;
         }

         return true;
      }



      #endregion

      /// <summary>
      /// Menus the open click.
      /// </summary>
      public void MenuOpenClick ()
      {
         var openPanel = new NSOpenPanel ();
         openPanel.ReleasedWhenClosed = true;
         openPanel.Prompt = "Select file";
         openPanel.CanChooseDirectories = false;
         openPanel.CanChooseFiles = true;
         openPanel.AllowedFileTypes = new string[]{"kdbx"};

         var result = openPanel.RunModal ();
         string fileName;

         if (result == 1) {
            fileName = openPanel.Url.AbsoluteString;

            // arequest password
            string password = RequestForPassword();

            if (password == string.Empty)
               return;

            OpenDatabase (fileName, password);
         }
      }



      private string RequestForPassword()
      {
         // setup the password sheet controller if one hasn't been setup already
         if (passwordWindowController == null)
            passwordWindowController = new PasswordWindowController ();

         // ask our edit sheet for information on the record we want to add
         string password = passwordWindowController.GetPassword (this);

         if (passwordWindowController.Cancelled)
            return string.Empty;

         return password;
      }


      /// <summary>
      /// Sidebars the selection changed.
      /// </summary>
      /// <param name='sender'>
      /// Sender.
      /// </param>
      /// <param name='e'>
      /// E.
      /// </param>
      private void sidebarSelectionChanged(object sender, SidebarEventArgs e) {
         FillEntriesList(e.SelectedItem);
      }



      /// <summary>
      /// Entrieses the table view_ handle double click.
      /// </summary>
      /// <param name='sender'>
      /// Sender.
      /// </param>
      /// <param name='e'>
      /// E.
      /// </param>
      private void EntriesTableView_HandleDoubleClick (object sender, EventArgs e)
      {
         EditEntry();
      }



      /// <summary>
      /// Copies to clipboard selected item password.
      /// </summary>
      private void CopyToClipboardSelectedItemPassword()
      {
         EntriesDataSource ds = (EntriesTableView.DataSource as EntriesDataSource);
         EntryModel selected = ds.Items[EntriesTableView.SelectedRow];

         string passwordToCopy = selected.Password;

         NSPasteboard pasteBoard = NSPasteboard.GeneralPasteboard;
         pasteBoard.DeclareTypes(new string[]{NSPasteboard.NSStringType}, null );
         pasteBoard.SetStringForType(passwordToCopy, NSPasteboard.NSStringType);

         //Console.WriteLine("Pasted: " + pasteBoard.GetStringForType(NSPasteboard.NSStringType));
      }



      // TODO: don't know if this works....
      [Export("windowShouldClose:")]
      public virtual bool WindowShouldClose (NSObject sender)
      {
         Console.WriteLine ("WindowShouldClose");
         databaseCommands.CloseDatabase();
         return true;
      }

      #endregion

      #region Fill Groups

      private void FillGroups(TreeGroupModel tree)
      {
         List<SidebarListItem> roots = new List<SidebarListItem> ();

         foreach (GroupModel group in tree.Nodes) {
   
            SidebarListItem rootItem = new SidebarListItem (group.Name);
            rootItem.DataObject = group;
            rootItem.IsHeader = true;
            rootItem.Expandable = true;

            FillGroupsRecursive(rootItem, group);

            roots.Add(rootItem);
         }

         GroupsSourceList.DataSource = new SidebarDataSource(roots);

         // expand the 1st item and all childs
         GroupsSourceList.ExpandItem(GroupsSourceList.ItemAtRow (0), true);
      }



      private void FillGroupsRecursive(SidebarListItem addTo, GroupModel parent)
      {
         if (parent.Childs.Count == 0)
            return;

         foreach (GroupModel child in parent.Childs) {
            SidebarListItem item = new SidebarListItem (child.Name);
            item.DataObject = child;

            FillGroupsRecursive(item, child);

            addTo.Children.Add(item);
         }
      }

      #endregion

      #region Fill Entries

      private void FillEntriesList(SidebarListItem selectedGroup)
      {
         GroupModel group = selectedGroup.DataObject as GroupModel;
         IList<EntryModel> entries = databaseCommands.FindEntries(group).OrderBy(x => x.Title).ToList();

         EntriesTableView.DataSource = new EntriesDataSource(entries);

         for (int index = 0; index < configuration.AvailableColumns.Count; index++) 
         {
            EntryColumn c = configuration.AvailableColumns[index];
            NSTableColumn[] columns = EntriesTableView.TableColumns();
            NSTableColumn col = columns[index];
            col.HeaderCell.StringValue = c.Header;
         }


      }

      #endregion

      #region Edit Entry

      private void EditEntry ()
      {
         // setup the edit sheet controller if one hasn't been setup already
         if (editWindowController == null)
            editWindowController = new EditWindowController ();
         
         EntriesDataSource ds = (EntriesTableView.DataSource as EntriesDataSource);
         EntryModel selected = ds.Items [EntriesTableView.SelectedRow];
         
         // get the current selected object and start the edit sheet
         EntryModel newValues = editWindowController.Edit (selected, this);

         Console.WriteLine("EditEntry - returned from editWindowController");
         Console.WriteLine("EditEntry - editWindowController.Cancelled = " + editWindowController.Cancelled.ToString(CultureInfo.InvariantCulture));

         if (!editWindowController.Cancelled) {
            // save updated data
            databaseCommands.SaveEntry (newValues, newValues.Entry);
            dbNeedsTobeSaved = true;
            
            //               // remove the current selection and replace it with the newly edited one
            //               var currentObjects = myTableArray.SelectedObjects;
            //               myTableArray.Remove(currentObjects);
            //               
            //               // make sure to add the new entry at the same selection location as before
            //               myTableArray.Insert(newValues, savedSelectionIndex);
         }
      }


      #endregion

      #endregion

      #region KeePass Database Operations

      /// <summary>
      /// Opens the database.
      /// </summary>
      /// <param name='filePath'>
      /// File path.
      /// </param>
      /// <param name='password'>
      /// Password.
      /// </param>
      private void OpenDatabase (string filePath, string password)
      {
         try
         {
            StartProgress("Opening password database...");
            databaseCommands.OpenDatabase(filePath, password);  

            // store the path of this db..
            NSUserDefaults userDefaults = NSUserDefaults.StandardUserDefaults;
            userDefaults.SetString(filePath, "lastdbpath");
         }
         catch(Exception ex) 
         {
            NSAlert alert = new NSAlert();
            alert.AlertStyle = NSAlertStyle.Critical;
            alert.MessageText = ex.Message;
//            alert.AddButton("Yes"); 
//            alert.AddButton("No"); 
            alert.RunModal();

            return;
         }
         finally
         {
            EndProgress();
         }



         //SetupSidebar();
         TreeGroupModel groups = databaseCommands.FindGroups();

         FillGroups(groups);
      }



      /// <summary>
      /// Saves the database.
      /// </summary>
      private void SaveDatabase()
      {
         if (!databaseCommands.IsOpen)
            return;

         Console.WriteLine("SaveDatabase");

         try
         {
            // if the db is not yet saved then show the save dialog
            if (databaseCommands.IsNewAndNotSaved)
            {
               var savePanel = new NSSavePanel ();
               savePanel.ReleasedWhenClosed = true;
               savePanel.Prompt = "Save file";
               savePanel.CanCreateDirectories = true;
               savePanel.AllowedFileTypes = new string[]{"kdbx"};
               // suggest a name
               savePanel.NameFieldStringValue = "mydb";
               var result = savePanel.RunModal ();

               string fileName;

               if (result != 1)
                  return;

               fileName = savePanel.Url.AbsoluteString;

               StartProgress("Saving...");
               databaseCommands.SaveDatabase (fileName);
            }
            else
            {
               StartProgress("Saving...");
               databaseCommands.SaveDatabase ();
            }

            // reset the flag
            dbNeedsTobeSaved = false;
         }
         catch(Exception ex) 
         {
            Console.WriteLine(ex.ToString());

            NSAlert alert = new NSAlert();
            alert.AlertStyle = NSAlertStyle.Critical;
            alert.MessageText = ex.Message;
            alert.RunModal();

            return;
         }
         finally
         {
            EndProgress();
         }

      }



      #endregion

      #region utils

      private void StartProgress(string text)
      {
         ProgressLabel.StringValue = text;
         ProgressIndicator.StartAnimation(this);
         ProgressPanel.OrderFront(this);
         //NSApp.BeginSheet(ProgressPanel, this.Window);
      }



      private void EndProgress()
      {
         ProgressIndicator.StopAnimation(this);
         ProgressPanel.OrderOut(this);

         //NSApp.EndSheet(ProgressPanel);
      }

      #endregion
   }
}

