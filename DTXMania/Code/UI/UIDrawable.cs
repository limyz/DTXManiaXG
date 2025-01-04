using System;
using SharpDX;
using SharpDX.Direct3D9;

namespace DTXMania.Code.UI
{
    public abstract class UIDrawable : IDisposable
    {
        public int renderOrder = 0;
        public Vector2 position = Vector2.Zero;
        public HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left;
        public VerticalAlignment verticalAlignment = VerticalAlignment.Top;
        public bool isVisible = true;
        
        public abstract void Draw(Device device, Vector2 offset);
        
        public virtual void SetAlignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            horizontalAlignment = horizontal;
            verticalAlignment = vertical;
        }
        
        public abstract void Dispose();
    }
    
    public enum HorizontalAlignment
    {
        Left,
        Center,
        Right
    }
    
    public enum VerticalAlignment
    {
        Top,
        Center,
        Bottom
    }
}