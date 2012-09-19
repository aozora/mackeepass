using System;



namespace MacKeePass.Config
{
   public class EntryColumn
   {
      private ColumnType columnType;

      public ColumnType ColumnType {
         get {
            return columnType;
         }
         set {
            columnType = value;
         }
      }


      public string Header {get; set;}


      public EntryColumn ()
      {
      }



      public EntryColumn (ColumnType columnType)
      {
         this.columnType = columnType;
      }

      public EntryColumn (ColumnType columnType, string header)
      {
         this.columnType = columnType;
         Header = header;
      }

   }
}

