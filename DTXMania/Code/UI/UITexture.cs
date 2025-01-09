using System.Diagnostics;
using FDK;
using SharpDX;
using SharpDX.Direct3D9;
using RectangleF = System.Drawing.RectangleF;

namespace DTXMania.Code.UI
{
    public abstract class UITexture : UIDrawable
    {
        protected UITexture(CTexture texture)
        {
            if (texture != null)
            {
                this.texture = texture;
            }
            else
            {
                texture = fallback;
            }
            
            size = new Vector2(texture.szTextureSize.Width, texture.szTextureSize.Height);
        }
        
        public CTexture Texture => texture;
        protected CTexture texture;
        
        public override void Draw(Device device, Matrix parentMatrix)
        {
            if (!isVisible) return;
            
            UpdateLocalTransformMatrix();
            
            Matrix combinedMatrix = localTransformMatrix * parentMatrix;
            
            texture.tDraw2DMatrix(device, combinedMatrix, size, new RectangleF(0, 0, texture.szTextureSize.Width, texture.szTextureSize.Height));
        }

        public override void Dispose()
        {
            CDTXMania.tReleaseTexture(ref texture);
        }

        protected static CTexture fallback = null;
        
        public static void LoadFallbackTexture()
        {
            string fallbackPath = @"System\Graphics\fallback.png";
            fallback = CDTXMania.tGenerateTexture(fallbackPath);
        }
    }
}