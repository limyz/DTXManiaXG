using FDK;
using SharpDX;
using SharpDX.Direct3D9;

namespace DTXMania.Code.UI
{
    public class UIDFPText : UIDrawable
    {
        private CActDFPFont font;
        private string text;
        public bool isHighlighted = false;
        
        public UIDFPText(CActDFPFont font, string text)
        {
            this.font = font;
            this.text = text;

            CalculateSize();
        }
        
        public void SetText(string text)
        {
            this.text = text;

            CalculateSize();
        }
        
        private void CalculateSize()
        {
            size = new Vector2(0, 0);
            foreach( char ch in text )
            {
                foreach( CActDFPFont.STCharacterMap charcterRect in font.stCharacterRects )
                {
                    if (charcterRect.ch != ch) continue;
                    
                    size.X += charcterRect.rc.Width - 5;
                    size.Y = charcterRect.rc.Height;
                    break;
                }
            }
        }
        
        public override void Draw(Device device, Matrix parentMatrix)
        {
            CTexture texture = isHighlighted ? font.txHighlightCharacterMap : font.txCharacterMap;
            
            if( texture != null )
            {
                UpdateLocalTransformMatrix();

                float x = 0;
                
                foreach( char ch in text )
                {
                    foreach( CActDFPFont.STCharacterMap charcterRect in font.stCharacterRects )
                    {
                        if( charcterRect.ch == ch )
                        {
                            var characterOffset = Matrix.Translation(new Vector3(x, 0, 0));
                            Vector2 sz = new Vector2(charcterRect.rc.Width, charcterRect.rc.Height);
                            texture.tDraw2DMatrix(device, characterOffset * localTransformMatrix * parentMatrix, sz, charcterRect.rc);
                            x += charcterRect.rc.Width - 5;
                            break;
                        }
                    }
                }
            }
        }

        public override void Dispose()
        {
            
        }
    }
}