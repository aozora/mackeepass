
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;



namespace MacKeePass
{
   public partial class PasswordWindow : MonoMac.AppKit.NSWindow
   {
      #region Constructors
      
      // Called when created from unmanaged code
      public PasswordWindow (IntPtr handle) : base (handle)
      {
         Initialize ();
      }
      
      // Called when created directly from a XIB file
      [Export ("initWithCoder:")]
      public PasswordWindow (NSCoder coder) : base (coder)
      {
         Initialize ();
      }
      
      // Shared initialization code
      void Initialize ()
      {
      }
      
      #endregion
   }
}

