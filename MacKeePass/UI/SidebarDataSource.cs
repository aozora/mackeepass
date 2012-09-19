using System;
using System.Drawing;
using System.Collections.Generic;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace MacKeePass.UI
{

   public class SidebarDataSource : NSOutlineViewDataSource
   {
      private List<SidebarListItem> rootItems = null;

      public List<SidebarListItem> Items {
         get {
            return this.rootItems;
         }
         set {
            rootItems = value;
         }
      }

      #region Constructor

      public SidebarDataSource () : this(new List<SidebarListItem>())
      {
      }



      public SidebarDataSource (List<SidebarListItem> items)
      {
         rootItems = items;
      }

      #endregion

      #region Overrides

      public override int GetChildrenCount (NSOutlineView outlineView, NSObject item)
      {
         if (item == null) {
            return rootItems.Count;
         }
         else {
            return ((SidebarListItem)item).Children.Count;
         }
      }



      public override NSObject GetChild (NSOutlineView outlineView, int childIndex, NSObject ofItem)
      {
         if (ofItem != null) {
            return ((SidebarListItem)ofItem).Children [childIndex];
         }
         else {
            return rootItems [childIndex];
         }
      }



      public override NSObject GetObjectValue (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
      {
         // NSTableCellView *selectedCell = [tableView viewAtColumn:selectedColumn row:selectedRow makeIfNecessary:NO];

         if (item != null) {
            SidebarListItem sourceItem = (SidebarListItem)item;
            
            string name = sourceItem.Name;
            
            if (sourceItem.IsHeader) {
               name = name.ToUpper ();
            }
            
            return new NSString (name);
         }
         else {
            return new NSString ("");
         }
      }



      public override bool ItemExpandable (NSOutlineView outlineView, NSObject item)
      {
         if (((SidebarListItem)item).Children.Count > 0) {
            return true;
         }
         else {
            return false;
         }
      }




      #endregion

   }
}

