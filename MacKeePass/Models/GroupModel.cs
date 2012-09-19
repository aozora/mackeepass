using System;
using System.Collections.Generic;
using KeePassLib;

namespace MacKeePass.Models
{
   public class GroupModel
   {
      public string Name { get; set; }

      public PwGroup Group { get; set; }

      public GroupModel Parent { get; set; }

      public IList<GroupModel> Childs { get; set; }

      public int ImageIndex { get; set; }
      public int SelectedImageIndex { get; set; }

      public bool Expires { get; set; }
      public DateTime ExpiryTime { get; set; }


      public GroupModel()
      {
         Childs = new List<GroupModel>();
      }

   }
}