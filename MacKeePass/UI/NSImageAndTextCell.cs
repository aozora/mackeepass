using System;
using MonoMac.Foundation;
using MonoMac.AppKit;
using System.Drawing;



namespace MacKeePass.UI
{
   public class NSImageAndTextCell : NSTextFieldCell
   {
      NSImage image = null;

      public NSImage Icon {
         get {
            return this.image;
         }
         set {
            image = value;
         }
      }

      public String Text {
         get {
            return this.StringValue;
         }
         set {
            this.StringValue = value;
         }
      }
      
      public NSImageAndTextCell ()
      {
         base.LineBreakMode = NSLineBreakMode.TruncatingMiddle;
      }
      
      public NSImageAndTextCell (IntPtr handle) : base (handle)
      {
         
      }
      
      public NSImageAndTextCell (NSImage image, String text)
      {
         this.image = image;
         this.StringValue = text;
      }

//    public override void DrawWithFrame(RectangleF cellFrame, NSView inView) {
//       float iconX = 0f;
//       float iconY = 0f;
//       float iconWidth = 0f;
//       float iconHeight = 0f;
//       
//       cellFrame.X = cellFrame.X - 10;
//       cellFrame.Height = cellFrame.Height - 1;
//       RectangleF newRect = cellFrame;
//       
//       if (image != null) {
//          iconX = cellFrame.X + 1;
//          iconY = cellFrame.Y + 2;
//          iconWidth = image.Size.Width;
//          iconHeight = image.Size.Height;
//          
//          image.Draw(new RectangleF(iconX, iconY, iconWidth, iconHeight), image.AlignmentRect, NSCompositingOperation.SourceOver, 1.0f, true, null);
//          
//          newRect = new RectangleF(iconX + iconWidth, cellFrame.Y, cellFrame.Width - iconX - iconWidth, cellFrame.Height - 1);
//       }
//       
//       base.DrawWithFrame(newRect, inView);
//    }

      public override void DrawInteriorWithFrame (RectangleF cellFrame, NSView inView)
      {
         float iconX = 0f;
         float iconY = 0f;
         float iconWidth = 0f;
         float iconHeight = 0f;
         
         cellFrame.X = cellFrame.X - 10;
         cellFrame.Height = cellFrame.Height - 1;
         RectangleF newRect = cellFrame;
         
         if (image != null) {
            iconX = cellFrame.X + 1;
            iconY = cellFrame.Y + 2;
            iconWidth = image.Size.Width;
            iconHeight = image.Size.Height;
            
            image.Draw (new RectangleF (iconX, iconY, iconWidth, iconHeight), image.AlignmentRect, NSCompositingOperation.SourceOver, 1.0f, true, null);
            
            newRect = new RectangleF (iconX + iconWidth, cellFrame.Y, cellFrame.Width - iconX - iconWidth, cellFrame.Height - 1);
         }
         
         base.DrawInteriorWithFrame (newRect, inView);
      }
   }
}

