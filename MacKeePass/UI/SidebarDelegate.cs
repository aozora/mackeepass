using System;
using System.Drawing;
using System.Collections.Generic;
using MonoMac.Foundation;
using MonoMac.AppKit;



namespace MacKeePass.UI
{
   public class SidebarDelegate : NSOutlineViewDelegate
   {
      private NSOutlineView sidebar;
      private SidebarDataSource dataSource;

      public SidebarDelegate (NSOutlineView outlineView)
      {
         sidebar = outlineView;
         dataSource = outlineView.DataSource as SidebarDataSource;
      }

      public override void SelectionDidChange (NSNotification notification)
      {
         if (sidebar != null && sidebar.SelectedRow >= 0) {
            Console.WriteLine ("SelectionDidChange - SelectedRow: " + sidebar.SelectedRow.ToString ());
            Console.WriteLine ("SelectionDidChange - SelectedItem.Name: " + this.SelectedItem.Name);
            // SelectedItem.DataObject is PwGroup
            SelectionChanged(this, new SidebarEventArgs(this.SelectedItem));
         }
      }



      #region public methods

      public void ExpandAllItems ()
      {
         for (int x = 0; x<sidebar.RowCount; x++) {
            sidebar.ExpandItem (sidebar.ItemAtRow (x));
         }
      }

      #endregion

//      #region fields
//
//      NSOutlineView outlineView = null;
//      NSView viewContainer = null;
//      SidebarDataSource dataSource = null;
//      
//      #endregion
//   
//      #region constructors
//
//      public SidebarDelegate (NSOutlineView outlineView)
//      {
//         this.viewContainer = outlineView.Superview;
//         this.outlineView = outlineView;
//         
//         this.outlineView.Delegate = this;
//         
//         dataSource = new SidebarDataSource ();
//         this.outlineView.DataSource = dataSource;
//         
//         this.outlineView.FloatsGroupRows = false;
//         this.outlineView.SelectionHighlightStyle = NSTableViewSelectionHighlightStyle.SourceList;
//      }
//
//      #endregion
//      
//      #region public methods
//
//      public void ExpandAllItems ()
//      {
//         for (int x = 0; x<outlineView.RowCount; x++) {
//            outlineView.ExpandItem (outlineView.ItemAtRow (x));
//         }
//      }
//
//      #endregion
//
  
      #region public properties

      public List<SidebarListItem> Items {
         get {
            return dataSource.Items;
         }
         set {
            dataSource.Items = value;
            sidebar.ReloadData ();
         }
      }
            
      public int SelectedIndex {
         get {
            return sidebar.SelectedRow;  
         }
      }
      
      public SidebarListItem SelectedItem {
         get {
            return (SidebarListItem)sidebar.ItemAtRow (SelectedIndex);   
         }
      }

      #endregion
//      
//      #region private methods
//
//      private SidebarListItem CastItem (NSObject item)
//      {
//         return (SidebarListItem)item;
//      }
//
//      #endregion
//      
//      #region delegate methods
//
      public override bool ShouldEditTableColumn (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
      {
         return false;
      }

      [Export ("outlineView:dataCellForTableColumn:tableColumn:row")]
      public NSCell GetDataCell (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
      {
         return base.GetCell (outlineView, tableColumn, item);
      }
//
//
//
      public override bool ShouldSelectItem (NSOutlineView outlineView, NSObject item)
      {
         if (((SidebarListItem)item).IsHeader) {
            return false;
         }
         else {
            return true;
         }
      }

      public override bool IsGroupItem (NSOutlineView outlineView, NSObject item)
      {
         if (((SidebarListItem)item).IsHeader) {
            return true;
         }
         else {
            return false;
         }
      }
//
//
//
//      public override float GetRowHeight (NSOutlineView outlineView, NSObject item)
//      {
//         if ((CastItem (item)).IsHeader) {
//            return 23f;
//         }
//         
//         return 20f;
//      }
//
//
//
//      public override void WillDisplayCell (NSOutlineView outlineView, NSObject cell, NSTableColumn tableColumn, NSObject item)
//      {
//         SidebarListItem sourceItem = CastItem (item);
//         NSImageAndTextCell sourceCell = (NSImageAndTextCell)cell;
//         
//         sourceCell.Icon = sourceItem.Icon;
//      }
//
//
//
//      public override void SelectionDidChange (NSNotification notification)
//      {
//         if (SelectionChanged != null) {
//            SelectionChanged (this, new SidebarEventArgs (SelectedItem));
//         }
//      }
//
//
//
      public override bool ShouldCollapseItem (NSOutlineView outlineView, NSObject item)
      {
         return ((SidebarListItem)item).Collapsable;
      }

      public override bool ShouldExpandItem (NSOutlineView outlineView, NSObject item)
      {
         return ((SidebarListItem)item).Expandable;
      }
//
//      #endregion
//      
//      
      #region events

      public event SelectionChangedEventHandler SelectionChanged;

      public delegate void SelectionChangedEventHandler (object sender, SidebarEventArgs e);

      #endregion

//      public EventHandler<SidebarEventArgs> SelectionChangedEventHandler;
//
//      protected void OnNewFileDetected (SidebarEventArgs e)
//      {
//         // Note this pattern for thread safety...
//         EventHandler<SidebarEventArgs> handler = this.SelectionChangedEventHandler; 
//         if (handler != null) {
//            handler (this, e);
//         }
//      }


   }



}

