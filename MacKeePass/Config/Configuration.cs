using System;
using System.Collections.Generic;



namespace MacKeePass.Config
{
   public class Configuration
   {

      public IList<EntryColumn> AvailableColumns {get; set;} 



      public Configuration ()
      {
         AvailableColumns = new List<EntryColumn> ();
         AvailableColumns.Add(new EntryColumn (ColumnType.Title, "Title"));
         AvailableColumns.Add(new EntryColumn (ColumnType.UserName, "User Name"));
         AvailableColumns.Add(new EntryColumn (ColumnType.Password , "Password"));
         AvailableColumns.Add(new EntryColumn (ColumnType.Url, "Url"));
         AvailableColumns.Add(new EntryColumn (ColumnType.Notes, "Notes"));

      
      }


   }
}

