using System;
using System.Drawing;
using System.Collections.Generic;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace MacKeePass.UI
{
   public class SidebarListItem : NSObject
   {
      String name = "";
      List<SidebarListItem> children = new List<SidebarListItem> ();
      NSImage icon = null;
      bool isHeader = false;
      int badge = 0;
      bool isCollapsable = true;
      bool isExpandable = true;
      object data = null;


      #region Constructor

      public SidebarListItem (String name)
      {
         this.name = name;
      }



      public SidebarListItem (String name, NSImage icon) : this(name)
      {
         this.icon = icon;
      }

      #endregion


      public bool Collapsable {
         get {
            return this.isCollapsable;
         }
         set {
            isCollapsable = value;
         }
      }

      public bool Expandable {
         get {
            return this.isExpandable;
         }
         set {
            isExpandable = value;
         }
      }

      public object DataObject {
         get {
            return this.data;
         }
         set {
            data = value;
         }
      }

      public int Badge {
         get {
            return this.badge;
         }
         set {
            badge = value;
         }
      }

      public bool IsHeader {
         get {
            return this.isHeader;
         }
         set {
            isHeader = value;
         }
      }



      public List<SidebarListItem> Children {
         get {
            return this.children;
         }
         set {
            children = value;
         }
      }

      public String Name {
         get {
            return this.name;
         }
         set {
            name = value;
         }
      }

      public NSImage Icon {
         get {
            return this.icon;
         }
         set {
            icon = value;
         }
      }
   }
}

