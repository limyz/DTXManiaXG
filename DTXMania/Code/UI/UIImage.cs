using System;
using FDK;
using SharpDX;
using SharpDX.Direct3D9;
using RectangleF = System.Drawing.RectangleF;

namespace DTXMania.Code.UI
{
    public class UIImage : UITexture
    {
        public RectangleF clipRect;
        
        public UIImage(string texturePath) : base(CDTXMania.tGenerateTexture(texturePath, false))
        {
            clipRect = new RectangleF(0, 0, texture.szImageSize.Width, texture.szImageSize.Height);
            size = new Vector2(texture.szImageSize.Width, texture.szImageSize.Height);
        }
        
        public UIImage(CTexture texture) : base(texture)
        {
            clipRect = new RectangleF(0, 0, texture.szImageSize.Width, texture.szImageSize.Height);
        }
    
        public override void Draw(Device device, Matrix parentMatrix)
        {
            if (!isVisible) return;
            
            UpdateLocalTransformMatrix();
            
            Matrix combinedMatrix = localTransformMatrix * parentMatrix;
            
            texture.tDraw2DMatrix(device, combinedMatrix, clipRect);
        }
    }
}