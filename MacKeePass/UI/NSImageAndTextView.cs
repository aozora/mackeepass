using System;
using MonoMac.Foundation;
using MonoMac.AppKit;
using System.Drawing;



namespace MacKeePass.UI
{
   public class NSImageAndTextView : NSText
   {
      NSImageView imageView = null;
      NSTextField textView = null;
      NSButton badgteView = null;
      int badge = -1;
      bool badgeVisible = true;
      bool imageVisible = true;
      int imageWidth = 16;
      int badgeWidth = 27;
      
      #region properties
      public bool BadgeVisible {
         get {
            return this.badgeVisible;
         }
         set {
            if (badgeVisible != value) {
               int widthChange = badgeWidth;
               
               if (!value) {
                  widthChange = -1 * widthChange;
               }
            
               RectangleF frame = textView.Frame;
               frame.Width += widthChange;
               textView.Frame = frame;

               badgeVisible = value;
            }
         }
      }

      public bool ImageVisible {
         get {
            return this.imageVisible;
         }
         set {
            if (imageVisible != value) {
               int xChange = imageWidth;
               
               if (!value) {
                  xChange = -1 * xChange;
               }
            
               RectangleF frame = textView.Frame;
               frame.X += xChange;
               textView.Frame = frame;

               imageVisible = value;
            }
         }
      }

      public NSImage Image {
         get {
            return this.imageView.Image;
         }
         set {
            if (value == null) {
               this.imageView.Image = new NSImage ();
            }
            else {
               this.imageView.Image = value; 
            }
         }
      }

      public String Text {
         get {
            return this.textView.StringValue;
         }
         set {
            this.textView.StringValue = value;
         }
      }
      
      public int Badge {
         get {
            return this.badge;
         }
         set {
            this.badge = value;
            this.badgteView.Title = this.badge.ToString ();
         }
      }
      
      #endregion
      
      #region constructor
      private void Initialize ()
      {
         imageView = new NSImageView (new RectangleF (0, 0, 16, 16));
         textView = new NSTextField ();
         textView.Bordered = false;
         textView.DrawsBackground = false;
         
         badgteView = new NSButton ();
         badgteView.BezelStyle = NSBezelStyle.TexturedRounded;
      }
      
      public NSImageAndTextView ()
      {
         Initialize ();
      }
      
      public NSImageAndTextView (IntPtr handle) : base (handle)
      {
         Initialize ();
      }

      public override void DrawRect (RectangleF dirtyRect)
      {
//       float iconX = 0f;
//       float iconY = 0f;
//       float iconWidth = 0f;
//       float iconHeight = 0f;
//       
//       RectangleF newRect = dirtyRect;
//       if (imageView != null) {
//          iconX = dirtyRect.X;
//          iconY = dirtyRect.Y + 2;
//          iconWidth = imageView.Size.Width;
//          iconHeight = imageView.Size.Height;
//          
//          imageView.Draw(new RectangleF(iconX, iconY, iconWidth, iconHeight), imageView.AlignmentRect, NSCompositingOperation.SourceOver, 1.0f, true, null);
//          
//          newRect = new RectangleF(iconX + iconWidth, dirtyRect.Y + 2, dirtyRect.Width - iconX - iconWidth, dirtyRect.Height);
//       }
//       
//       base.DrawRect(newRect);
      }
      
      #endregion
   }
}

