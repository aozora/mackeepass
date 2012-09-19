using System;
using MonoMac.AppKit;
using MonoMac.Foundation;
using System.IO;



namespace MacKeePass.Helpers
{
   public static class ResourceHelper
   {
      static string resourcesPath = NSBundle.MainBundle.ResourceUrl.Path;
      
      public static NSImage LoadImageFromBundle (String fileName)
      {
         return new NSImage (Path.Combine (resourcesPath, "resources", fileName));
      }
   }
}

