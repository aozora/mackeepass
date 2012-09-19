// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;

namespace MacKeePass
{
	[Register ("PasswordWindowController")]
	partial class PasswordWindowController
	{
		[Outlet]
		MonoMac.AppKit.NSSecureTextField passwordTextField { get; set; }

		[Action ("okClick:")]
		partial void okClick (MonoMac.Foundation.NSObject sender);

		[Action ("cancelClick:")]
		partial void cancelClick (MonoMac.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (passwordTextField != null) {
				passwordTextField.Dispose ();
				passwordTextField = null;
			}
		}
	}

	[Register ("PasswordWindow")]
	partial class PasswordWindow
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
