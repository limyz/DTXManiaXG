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
            this.texture = texture;
        }
        
        public CTexture Texture => texture;
        protected CTexture texture;
        
        protected Vector2 alignmentOffset = Vector2.Zero;
        
        public override void Draw(Device device, Matrix parentMatrix)
        {
            if (!isVisible) return;
            
            UpdateLocalTransformMatrix();
            
            Matrix combinedMatrix = localTransformMatrix * parentMatrix;
            
            texture.tDraw2DMatrix(device, combinedMatrix, new RectangleF(0, 0, texture.szTextureSize.Width, texture.szTextureSize.Height));
        }

        public override void Dispose()
        {
            CDTXMania.tReleaseTexture(ref texture);
        }
    }
}