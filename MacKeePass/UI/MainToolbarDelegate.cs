//using System;
//using MonoMac.Foundation;
//using MonoMac.AppKit;
//
//
//namespace MacKeePass.UI
//{
//   public class MainToolbarDelegate : NSToolbarDelegate
//   {
//
//      #region implemented abstract members of MonoMac.AppKit.NSToolbarDelegate
//
//      public override NSToolbarItem WillInsertItem (NSToolbar toolbar, string itemIdentifier, bool willBeInserted)
//      {
//         throw new System.NotImplementedException ();
//      }
//
//
//
//      public override string[] DefaultItemIdentifiers (NSToolbar toolbar)
//      {
//         throw new System.NotImplementedException ();
//      }
//
//
//
//      public override string[] AllowedItemIdentifiers (NSToolbar toolbar)
//      {
//         return new string[]
//         {
//            "NSToolbarSeparatorItemIdentifier", 
//            "NSToolbarSpaceItemIdentifier", 
//            "NSToolbarFlexibleSpaceItemIdentifier",
//            "OpenDbIdentifier",
//            "AddGroupIdentifier",
//            "AddEntryIdentifier",
//            null
//         };
//
//      }
//
//      #endregion
//
//      #region implemented abstract members of MonoMac.AppKit.NSToolbarDelegate
//      public override string[] SelectableItemIdentifiers (NSToolbar toolbar)
//      {
//         throw new System.NotImplementedException ();
//      }
//
//      public override void WillAddItem (NSNotification notification)
//      {
//         throw new System.NotImplementedException ();
//      }
//
//      public override void DidRemoveItem (NSNotification notification)
//      {
//         throw new System.NotImplementedException ();
//      }
//      #endregion
//   }
//}
//
