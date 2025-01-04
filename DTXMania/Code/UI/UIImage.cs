using System;
using SharpDX;
using SharpDX.Direct3D9;
using Rectangle = System.Drawing.Rectangle;

namespace DTXMania.Code.UI
{
    public class UIImage : UITexture
    {
        public DrawMode drawMode = DrawMode.Stretched;
        public Rectangle size;
        
        public UIImage(string texturePath) : base(CDTXMania.tGenerateTexture(texturePath, false))
        {
            size = new Rectangle(0, 0, texture.szImageSize.Width, texture.szImageSize.Height);
        }

        public override void SetAlignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            horizontalAlignment = horizontal;
            verticalAlignment = vertical;
            
            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    alignmentOffset.X = 0;
                    break;
                case HorizontalAlignment.Center:
                    alignmentOffset.X = -(size.Width / 2.0f);
                    break;
                case HorizontalAlignment.Right:
                    alignmentOffset.X = -size.Width;
                    break;
            }
            
            switch (verticalAlignment)
            {
                case VerticalAlignment.Top:
                    alignmentOffset.Y = 0;
                    break;
                case VerticalAlignment.Center:
                    alignmentOffset.Y = (size.Height / 2.0f);
                    break;
                case VerticalAlignment.Bottom:
                    alignmentOffset.Y = size.Height;
                    break;
            }
        }

        public override void Draw(Device device, Vector2 offset)
        {
            Vector2 pos = position + offset + alignmentOffset;

            switch (drawMode)
            {
                case DrawMode.Stretched:
                    texture.tDraw2D(device, pos.X, pos.Y, size);
                    break;
                
                case DrawMode.TiledLeftRight:
                    Rectangle rect = new Rectangle(0, 0, texture.szImageSize.Width / 4, size.Height);
                    
                    //draw left side
                    texture.tDraw2D(device, pos.X, pos.Y, rect);
                    rect.X = rect.Width * 3;
                    
                    //draw right side
                    texture.tDraw2D(device, pos.X + size.Width - rect.Width, pos.Y, rect);
                    
                    int widthRemaining = size.Width;
                
                    pos.X += 16; //move x past the left edge
                    widthRemaining -= 32; //subtract the widths of the edges

                    //draw middle section in strips
                    while (widthRemaining > 0)
                    {
                        int drawWidth = Math.Min(16, widthRemaining);
                        Rectangle rectangle = new Rectangle(8, 0, drawWidth, 32);
                        texture.tDraw2D(CDTXMania.app.Device, pos.X, pos.Y, rectangle);

                        pos.X += drawWidth;
                        widthRemaining -= drawWidth;
                    }
                    break;
            }
        }
    }

    public enum DrawMode
    {
        Stretched,
        TiledLeftRight,
        TiledTopBottom,
        TiledAllSides
    }
}