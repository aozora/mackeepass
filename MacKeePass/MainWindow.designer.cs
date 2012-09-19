// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;

namespace MacKeePass
{
	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		[Outlet]
		MonoMac.AppKit.NSOutlineView GroupsSourceList { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTableView EntriesTableView { get; set; }

		[Outlet]
		MonoMac.AppKit.NSToolbar MainToolbar { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField ProgressLabel { get; set; }

		[Outlet]
		MonoMac.AppKit.NSProgressIndicator ProgressIndicator { get; set; }

		[Outlet]
		MonoMac.AppKit.NSPanel ProgressPanel { get; set; }

		[Action ("NewDatabaseToolbarClick:")]
		partial void NewDatabaseToolbarClick (MonoMac.Foundation.NSObject sender);

		[Action ("OpenDatabaseToolbarClick:")]
		partial void OpenDatabaseToolbarClick (MonoMac.Foundation.NSObject sender);

		[Action ("SaveDatabaseToolbarClick:")]
		partial void SaveDatabaseToolbarClick (MonoMac.Foundation.NSObject sender);

		[Action ("EditEntryToolbarClick:")]
		partial void EditEntryToolbarClick (MonoMac.Foundation.NSObject sender);

		[Action ("CopyPasswordToolbarClick:")]
		partial void CopyPasswordToolbarClick (MonoMac.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (GroupsSourceList != null) {
				GroupsSourceList.Dispose ();
				GroupsSourceList = null;
			}

			if (EntriesTableView != null) {
				EntriesTableView.Dispose ();
				EntriesTableView = null;
			}

			if (MainToolbar != null) {
				MainToolbar.Dispose ();
				MainToolbar = null;
			}

			if (ProgressLabel != null) {
				ProgressLabel.Dispose ();
				ProgressLabel = null;
			}

			if (ProgressIndicator != null) {
				ProgressIndicator.Dispose ();
				ProgressIndicator = null;
			}

			if (ProgressPanel != null) {
				ProgressPanel.Dispose ();
				ProgressPanel = null;
			}
		}
	}

	[Register ("MainWindow")]
	partial class MainWindow
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
