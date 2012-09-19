using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;



namespace MacKeePass
{
   public partial class AppDelegate : NSApplicationDelegate
   {
      MainWindowController mainWindowController;
      
      public AppDelegate ()
      {
      }

      public override void FinishedLaunching (NSObject notification)
      {
         mainWindowController = new MainWindowController ();
         mainWindowController.Window.MakeKeyAndOrderFront (this);
      }

      partial void MenuOpenClick (MonoMac.Foundation.NSObject sender)
      {
         mainWindowController.MenuOpenClick ();
      }



      /// <summary>
      /// Applications will terminate after last window closed.
      /// </summary>
      /// <returns>
      /// The should terminate after last window closed.
      /// </returns>
      /// <param name='sender'>
      /// If set to <c>true</c> sender.
      /// </param>
      public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
      {
         return true;
      }

   }
}

