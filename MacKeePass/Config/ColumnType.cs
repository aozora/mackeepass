using System;



namespace MacKeePass
{
   public enum ColumnType
   {
      Title = 0,
      UserName,
      Password,
      Url,
      Notes,
      CreationTime,
      LastAccessTime,
      LastModificationTime,
      ExpiryTime,
      Uuid,
      Attachment,

      CustomString,

      PluginExt, // Column data provided by a plugin

      OverrideUrl,
      Tags,
      ExpiryTimeDateOnly,
      Size,
      HistoryCount,

      Count // Virtual identifier representing the number of types
   }

}

