using SharpDX;
using SharpDX.Direct3D9;
using Color = System.Drawing.Color;

namespace DTXMania.Code.UI
{
    public class UIText : UITexture
    {
        private bool dirty = true;
        private string text = "";
        
        public Color fontColor = Color.White;
        public Color edgeColor = Color.Black;
        public Color gradationTopColor = Color.White;
        public Color gradationBottomColor = Color.White;
        
        public CPrivateFont.DrawMode drawMode = CPrivateFont.DrawMode.Edge;
        public CPrivateFont font;
        
        public UIText(CPrivateFont font) : base(null)
        {
            this.font = font;
        }
        
        public void SetText(string text)
        {
            this.text = text;
            dirty = true;
        }
        
        public override void Draw(Device device, Matrix parentMatrix)
        {
            if (dirty)
            {
                RenderTexture();
            }
            
            base.Draw(device, parentMatrix);
        }

        public void RenderTexture()
        {
            var bmp = font.DrawPrivateFont(text, drawMode, fontColor, edgeColor, gradationTopColor, gradationBottomColor);
            texture = CDTXMania.tGenerateTexture(bmp, false);
            size = new Vector2(texture.szImageSize.Width, texture.szImageSize.Height);
            bmp.Dispose();
            dirty = false;
        }

        public override void Dispose()
        {
            texture.Dispose();
        }
    }
}