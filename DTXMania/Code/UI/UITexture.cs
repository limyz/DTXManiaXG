using FDK;
using SharpDX;
using SharpDX.Direct3D9;

namespace DTXMania.Code.UI
{
    public abstract class UITexture : UIDrawable
    {
        protected UITexture(CTexture texture)
        {
            this.texture = texture;
        }
        
        public CTexture Texture => texture;
        protected CTexture texture;
        
        protected Vector2 alignmentOffset = Vector2.Zero;
        
        public override void Draw(Device device, Vector2 offset)
        {
            Vector2 pos = position + offset + alignmentOffset;
            texture.tDraw2D(device, pos.X, pos.Y);
        }

        public override void SetAlignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            base.SetAlignment(horizontal, vertical);
            
            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    alignmentOffset.X = 0;
                    break;
                case HorizontalAlignment.Center:
                    alignmentOffset.X = -(texture.szImageSize.Width / 2.0f);
                    break;
                case HorizontalAlignment.Right:
                    alignmentOffset.X = -texture.szImageSize.Width;
                    break;
            }
            
            switch (verticalAlignment)
            {
                case VerticalAlignment.Top:
                    alignmentOffset.Y = 0;
                    break;
                case VerticalAlignment.Center:
                    alignmentOffset.Y = (texture.szImageSize.Height / 2.0f);
                    break;
                case VerticalAlignment.Bottom:
                    alignmentOffset.Y = texture.szImageSize.Height;
                    break;
            }
        }

        public override void Dispose()
        {
            CDTXMania.tReleaseTexture(ref texture);
        }
    }
}