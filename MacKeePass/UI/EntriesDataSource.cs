using System;
using System.Collections.Generic;
using MacKeePass.Models;
using MonoMac.Foundation;
using MonoMac.AppKit;
using System.Text;


namespace MacKeePass
{
   public class EntriesDataSource : NSTableViewDataSource
   {
      private IList<EntryModel> entries;

      public IList<EntryModel> Items {
         get {
            return this.entries;
         }
      }



      public EntriesDataSource (IList<EntryModel> items)
      {
         entries = items;
      }



      /// <summary>
      /// This method will be called by the NSTableView control to learn the number of rows to display.
      /// </summary>
      /// <returns>
      /// The row count.
      /// </returns>
      /// <param name='tableView'>
      /// Table view.
      /// </param>
      public override int GetRowCount (NSTableView tableView)
      {
         return entries.Count;
      }        



      /// <summary>
      /// GThis method will be called by the control for each column and each row.
      /// </summary>
      /// <returns>
      /// The object value.
      /// </returns>
      /// <param name='tableView'>
      /// Table view.
      /// </param>
      /// <param name='tableColumn'>
      /// Table column.
      /// </param>
      /// <param name='rowIndex'>
      /// Row index.
      /// </param>
      public override NSObject GetObjectValue(NSTableView tableView, NSTableColumn tableColumn, int rowIndex)
      {
         string valueKey = (string)(NSString)tableColumn.Identifier;
         EntryModel e = entries[rowIndex];
         string s;

         switch (valueKey) 
         {
            case "title":
               s = e.Title;
               break;

            case "username":
               s = e.UserName;
               break;

            // don't reveal the password in clear
            case "password":
               string p = new string('*', e.Password.Length);
               s = p;
               break;

            case "notes":
               s = e.Notes;
               break;

            case "url":
               s = e.Url;
               break;

            default:
               s = "";
               break;
         }

         return new NSString(s);

         //throw new Exception(string.Format("Incorrect value requested '{0}'", (string)valueKey));
      }


   }
}

