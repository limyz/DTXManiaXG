using FDK;
using SharpDX;
using SharpDX.Direct3D9;
using RectangleF = System.Drawing.RectangleF;

namespace DTXMania.Code.UI
{
    public class UIImage : UITexture
    {
        public RectangleF clipRect;
        public RectangleF sliceRect;
        public ERenderMode renderMode = ERenderMode.Stretched;

        public UIImage(string texturePath) : base(null)
        { 
            CTexture temp = CDTXMania.tGenerateTexture(texturePath);
            texture = temp ?? fallback;
            
            size = new Vector2(texture.szTextureSize.Width, texture.szTextureSize.Height);
            clipRect = new RectangleF(0, 0, texture.szImageSize.Width, texture.szImageSize.Height);
        }

        public UIImage(CTexture texture) : base(texture)
        {
            if (texture != null)
            {
                clipRect = new RectangleF(0, 0, texture.szImageSize.Width, texture.szImageSize.Height);
            }
        }

        public override void Draw(Device device, Matrix parentMatrix)
        {
            if (!isVisible) return;
            
            UpdateLocalTransformMatrix();
            
            Matrix combinedMatrix = localTransformMatrix * parentMatrix;

            switch (renderMode)
            {
                case ERenderMode.Stretched:
                    texture.tDraw2DMatrix(device, combinedMatrix, size, clipRect);
                    break;
                
                case ERenderMode.Sliced:
                    texture.tDraw2DMatrixSliced(device, combinedMatrix, size, clipRect, sliceRect);
                    break;
            }
        }

        public void SetTexture(CTexture txAutoStatus, bool updateRects = true)
        {
            texture = txAutoStatus;

            if (updateRects)
            {
                size = new Vector2(texture.szTextureSize.Width, texture.szTextureSize.Height);
                clipRect = new RectangleF(0, 0, texture.szImageSize.Width, texture.szImageSize.Height);
            }
        }
    }
    
    public enum ERenderMode
    {
        Stretched,
        Sliced
    }
}