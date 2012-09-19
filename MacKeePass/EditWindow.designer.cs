// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;

namespace MacKeePass
{
	[Register ("EditWindowController")]
	partial class EditWindowController
	{
		[Outlet]
		MonoMac.AppKit.NSTextField TitleTextField { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField UserNameTextField { get; set; }

		[Outlet]
		MonoMac.AppKit.NSSecureTextField PasswordSecureTextField { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField PasswordClearTextField { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField UrlTextField { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField NotesTextField { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton ExpireCheckBox { get; set; }

		[Outlet]
		MonoMac.AppKit.NSDatePicker ExpireTimeDatePicker { get; set; }

		[Action ("CancelClick:")]
		partial void CancelClick (MonoMac.Foundation.NSObject sender);

		[Action ("OkClick:")]
		partial void OkClick (MonoMac.Foundation.NSObject sender);

		[Action ("ShowClearPasswordClick:")]
		partial void ShowClearPasswordClick (MonoMac.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (TitleTextField != null) {
				TitleTextField.Dispose ();
				TitleTextField = null;
			}

			if (UserNameTextField != null) {
				UserNameTextField.Dispose ();
				UserNameTextField = null;
			}

			if (PasswordSecureTextField != null) {
				PasswordSecureTextField.Dispose ();
				PasswordSecureTextField = null;
			}

			if (PasswordClearTextField != null) {
				PasswordClearTextField.Dispose ();
				PasswordClearTextField = null;
			}

			if (UrlTextField != null) {
				UrlTextField.Dispose ();
				UrlTextField = null;
			}

			if (NotesTextField != null) {
				NotesTextField.Dispose ();
				NotesTextField = null;
			}

			if (ExpireCheckBox != null) {
				ExpireCheckBox.Dispose ();
				ExpireCheckBox = null;
			}

			if (ExpireTimeDatePicker != null) {
				ExpireTimeDatePicker.Dispose ();
				ExpireTimeDatePicker = null;
			}
		}
	}

	[Register ("EditWindow")]
	partial class EditWindow
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
