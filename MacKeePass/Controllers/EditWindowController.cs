
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MacKeePass.Models;
using KeePassLib.Collections;
using KeePassLib;



namespace MacKeePass
{
   public partial class EditWindowController : MonoMac.AppKit.NSWindowController
   {
      #region Constructors
      
      // Called when created from unmanaged code
      public EditWindowController (IntPtr handle) : base (handle)
      {
         Initialize ();
      }
      
      // Called when created directly from a XIB file
      [Export ("initWithCoder:")]
      public EditWindowController (NSCoder coder) : base (coder)
      {
         Initialize ();
      }
      
      // Call to load from the XIB/NIB file
      public EditWindowController () : base ("EditWindow")
      {
         Initialize ();
      }
      
      // Shared initialization code
      void Initialize ()
      {
      }
      
      #endregion
      
      //strongly typed window accessor
      public new EditWindow Window {
         get {
            return (EditWindow)base.Window;
         }
      }



      /// <summary>
      /// Awakes from nib. (Load)
      /// </summary>
      public override void AwakeFromNib ()
      {
         base.AwakeFromNib ();
      
         // initially hide the password in clear textfield
         PasswordClearTextField.AlphaValue = 0;

         NSDateFormatter.DefaultBehavior = NSDateFormatterBehavior.Default;
      }


      bool cancelled;
      NSApplication NSApp = NSApplication.SharedApplication;   
      private ProtectedStringDictionary m_vStrings = null;
      private ProtectedBinaryDictionary m_vBinaries = null;
      private PwObjectList<PwEntry> m_vHistory = null;
      private PwEntry originalEntry = null;
      public ProtectedStringDictionary EntryStrings { get { return m_vStrings; } }
      public ProtectedBinaryDictionary EntryBinaries { get { return m_vBinaries; } }


      /// <summary>
      /// Edit the specified source and sender.
      /// </summary>
      /// <param name='source'>
      /// Source.
      /// </param>
      /// <param name='sender'>
      /// Sender.
      /// </param>
      public EntryModel Edit(EntryModel entry, MainWindowController sender)      
      {
         NSWindow window = this.Window;

         cancelled = false;
         
         //var editFields = editForm.Cells;
         
         if (entry != null)
         {
            originalEntry = entry.Entry.CloneDeep();

//            m_vStrings = source.Entry.Strings.CloneDeep();
//            m_vBinaries = source.Entry.Binaries.CloneDeep();
//            m_vHistory = source.Entry.History.CloneDeep();

            TitleTextField.StringValue = entry.Title;
            UserNameTextField.StringValue = entry.UserName;

            PasswordSecureTextField.StringValue = PwDefs.HiddenPassword;
            PasswordClearTextField.StringValue = entry.Password;

            UrlTextField.StringValue = entry.Url;
            NotesTextField.StringValue = entry.Notes;
            ExpireCheckBox.State = entry.Expires ? NSCellStateValue.On : NSCellStateValue.Off;
            ExpireTimeDatePicker.DateValue = entry.ExpireDate;
         }
         else
         {
            entry = new EntryModel();
            entry.Entry = new PwEntry(true, true);
            entry.IsNew = true;

            // we are adding a new entry,
            // make sure the form fields are empty due to the fact that this controller is recycled
            // each time the user opens the sheet -            
            TitleTextField.StringValue = string.Empty;
            UserNameTextField.StringValue = string.Empty;
            PasswordSecureTextField.StringValue = string.Empty;
            PasswordClearTextField.StringValue = string.Empty;
            UrlTextField.StringValue = string.Empty;
            NotesTextField.StringValue = string.Empty;
            ExpireCheckBox.State = NSCellStateValue.Off;
            ExpireTimeDatePicker.DateValue = null;
         }
         
         NSApp.BeginSheet(window,sender.Window);
         NSApp.RunModalForWindow(window);
         // sheet is up here.....
         
         // when StopModal is called will continue here ....
         Console.WriteLine("Edit - exit from Edit");
         NSApp.EndSheet(window);
         window.OrderOut(this);


         //EntryModel saved = new EntryModel();
         entry.Title = TitleTextField.StringValue;
         entry.UserName = UserNameTextField.StringValue;
         entry.Password = PasswordSecureTextField.StringValue;
         entry.Url = UrlTextField.StringValue;
         entry.Notes = NotesTextField.StringValue;
         entry.Expires = ExpireCheckBox.State == NSCellStateValue.On;
         entry.ExpireDate = NSDateToDateTime(ExpireTimeDatePicker.DateValue);

         return entry;
      }



      partial void OkClick (NSObject sender)
      {
         //Console.WriteLine("OkClick - exit from Edit");
         NSApp.StopModal();
      }



      /// <summary>
      /// Cancel Action for cancel button
      /// </summary>
      /// <param name="sender">
      /// A <see cref="NSButton"/>
      /// </param>
      partial void CancelClick (NSObject sender)
      {
         NSApp.StopModal();
         cancelled = true;
      }



      /// <summary>
      /// Property for whether the panel was canceled or not.
      /// </summary>
      internal bool Cancelled
      {
         get
         {
            return cancelled; 
         }
      }



      partial void ShowClearPasswordClick(NSObject sender)
      {
//         [UIView beginAnimations:nil context:NULL];
//         self.widget1.alpha = 0;
//         self.widget2.alpha = 1;
//         [UIView commitAnimations];
         //var f = PasswordClearTextField;

         Console.WriteLine("PasswordClearTextField.Hidden = " + PasswordClearTextField.Hidden.ToString());
         Console.WriteLine("PasswordSecureTextField.Hidden = " + PasswordSecureTextField.Hidden.ToString());

         float alpha1 = PasswordSecureTextField.AlphaValue;
         float alpha2 = PasswordClearTextField.AlphaValue;

         PasswordClearTextField.AlphaValue = alpha1;
         PasswordSecureTextField.AlphaValue = alpha2;
      }


      public DateTime NSDateToDateTime(NSDate date)
      {
         return (new DateTime(2001,1,1,0,0,0)).AddSeconds(date.SecondsSinceReferenceDate);
      }


   }
}

