using System.Collections;
using System.Collections.Generic;

namespace MacKeePass.Models
{
   public class TreeGroupModel
   {
      public IList<GroupModel> Nodes { get; set; }



      public TreeGroupModel()
      {
         Nodes = new List<GroupModel>();
      }
   }
}