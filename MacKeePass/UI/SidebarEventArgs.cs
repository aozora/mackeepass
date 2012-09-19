using System;



namespace MacKeePass.UI
{
   public class SidebarEventArgs : EventArgs
   {
      SidebarListItem selectedItem = null;
         
      public SidebarEventArgs (SidebarListItem item)
      {
         this.selectedItem = item;
      }

      public SidebarListItem SelectedItem {
         get {
            return this.selectedItem;
         }
      }
      
   }
}

