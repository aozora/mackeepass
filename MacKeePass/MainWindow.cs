
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace MacKeePass
{
	public partial class MainWindow : MonoMac.AppKit.NSWindow
	{
		#region Constructors
		
		// Called when created from unmanaged code
		public MainWindow (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MainWindow (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
		}
		
		#endregion



//		/// <summary>
//		/// Terminate the app when the Windows the will close.
//		/// </summary>
//		/// <param name='notification'>
//		/// Notification.
//		/// </param>
//		[Export ("windowWillClose:")]
//		public void WindowWillClose (NSNotification notification)
//		{
//			Console.WriteLine ("windowWillClose:");
//			NSApplication.SharedApplication.Terminate (this);       
//		}

	}
}

