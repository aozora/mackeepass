using System;
using System.Collections.Generic;
using KeePassLib;



namespace MacKeePass.Models
{
   public class EntryModel
   {
      public GroupModel Group { get; set; }

      public PwEntry Entry { get; set; }

      public string Title { get; set; }

      public string UserName { get; set; }

      public string Password { get; set; }

      public string Url { get; set; }

      public string Notes { get; set; }

      public string IconKey { get; set; }

      public int CustomIconId { get; set; }

      public DateTime CreationDate { get; set; }

      public DateTime LastModifiedDate { get; set; }

      public DateTime LastAccessDate { get; set; }

      public DateTime ExpireDate  { get; set; }

      public bool Expires { get; set; }

      public ulong UsageCount { get; set; }

      public IList<string> Tags { get; set; }

      public bool IsNew {get; set;}

      public EntryModel ()
      {
         Tags = new List<string> ();
      }

   }
}

