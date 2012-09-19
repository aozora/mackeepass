using System;
using KeePassLib.Interfaces;
using System.Diagnostics;



namespace MacKeePass
{
   public class DefaultKeePassLogger : IStatusLogger
   {
      public DefaultKeePassLogger ()
      {
      }

      #region IStatusLogger implementation

      public void StartLogging (string strOperation, bool bWriteOperationToLog)
      {
         Debug.WriteLine(strOperation);
      }


      public void EndLogging()
      {
      }
      
      
      
      public bool SetProgress(uint uPercent)
      {
         return true;
      }
      
      
      
      public bool SetText(string strNewText, LogStatusType lsType)
      {
         return true;
      }
      
      
      
      public bool ContinueWork()
      {
         return true;
      }

      #endregion
   }
}

