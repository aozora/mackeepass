
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;



namespace MacKeePass
{
   public partial class PreferencesWindowController : MonoMac.AppKit.NSWindowController
   {
      #region Constructors
      
      // Called when created from unmanaged code
      public PreferencesWindowController (IntPtr handle) : base (handle)
      {
         Initialize ();
      }
      
      // Called when created directly from a XIB file
      [Export ("initWithCoder:")]
      public PreferencesWindowController (NSCoder coder) : base (coder)
      {
         Initialize ();
      }
      
      // Call to load from the XIB/NIB file
      public PreferencesWindowController () : base ("PreferencesWindow")
      {
         Initialize ();
      }
      
      // Shared initialization code
      void Initialize ()
      {
      }
      
      #endregion
      
      //strongly typed window accessor
      public new PreferencesWindow Window {
         get {
            return (PreferencesWindow)base.Window;
         }
      }


      partial void SecondsStepperAction (NSObject sender)
      {
         NSStepper stepper = sender as NSStepper;
         
         //Console.WriteLine ("Change level: {0}", stepper.IntValue);
         SecondsTextField.IntValue = stepper.IntValue;
      }
   }
}

