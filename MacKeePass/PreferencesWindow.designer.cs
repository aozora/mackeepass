// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;

namespace MacKeePass
{
	[Register ("PreferencesWindowController")]
	partial class PreferencesWindowController
	{
		[Outlet]
		MonoMac.AppKit.NSTextField SecondsTextField { get; set; }

		[Action ("SecondsStepperAction:")]
		partial void SecondsStepperAction (MonoMac.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (SecondsTextField != null) {
				SecondsTextField.Dispose ();
				SecondsTextField = null;
			}
		}
	}

	[Register ("PreferencesWindow")]
	partial class PreferencesWindow
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
