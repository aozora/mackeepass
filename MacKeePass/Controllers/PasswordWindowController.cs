
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;



namespace MacKeePass
{
   public partial class PasswordWindowController : MonoMac.AppKit.NSWindowController
   {
      #region Constructors
      
      // Called when created from unmanaged code
      public PasswordWindowController (IntPtr handle) : base (handle)
      {
         Initialize ();
      }
      
      // Called when created directly from a XIB file
      [Export ("initWithCoder:")]
      public PasswordWindowController (NSCoder coder) : base (coder)
      {
         Initialize ();
      }
      
      // Call to load from the XIB/NIB file
      public PasswordWindowController () : base ("PasswordWindow")
      {
         Initialize ();
      }
      
      // Shared initialization code
      void Initialize ()
      {
      }
      
      #endregion
      
//      //strongly typed window accessor
//      public new PasswordWindow Window {
//         get {
//            return (PasswordWindow)base.Window;
//         }
//      }

      public override void WindowDidLoad ()
      {
         base.WindowDidLoad ();


      }


      bool cancelled;
      NSApplication NSApp = NSApplication.SharedApplication;   
      //NSMutableCharacterSet password ;



      /// <summary>
      /// Button Cancel  click
      /// </summary>
      /// <param name='sender'>
      /// Sender.
      /// </param>
      partial void cancelClick (NSObject sender)
      {
         NSApp.StopModal();
         cancelled = true;
      }



      /// <summary>
      /// Oks the click.
      /// </summary>
      /// <param name='sender'>
      /// Sender.
      /// </param>
      partial void okClick (NSObject sender)
      {
         // save the values for later
//         var editFields = editForm.Cells;
//         List<NSString> objects = new List<NSString> {new NSString(editFields[FIRST_NAME].StringValue),
//                                          new NSString(editFields[LAST_NAME].StringValue),
//                                          new NSString(editFields[PHONE].StringValue)};
//
//         savedFields = NSMutableDictionary.FromObjectsAndKeys(objects.ToArray(),
//                                                              TestWindowController.Keys.ToArray());
         
         NSApp.StopModal();
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




      public string GetPassword(MainWindowController sender)
      {
         NSWindow window = this.Window;
         
         cancelled = false;


         NSApp.BeginSheet(window, sender.Window);  //,null,null,IntPtr.Zero);
         NSApp.RunModalForWindow(window);
         // sheet is up here.....
         
         // when StopModal is called will continue here ....
         NSApp.EndSheet(window);
         window.OrderOut(this);



         var pwd = passwordTextField.StringValue;

         //password = NSMutableCharacterSet.FromString(editFields);

         return pwd;
      }


   }
}

