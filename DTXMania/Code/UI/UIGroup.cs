using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct3D9;

namespace DTXMania.Code.UI
{
    public class UIGroup : UIDrawable
    {
        protected List<UIDrawable> children = new List<UIDrawable>();
        
        public T AddChild<T>(T element) where T : UIDrawable
        {
            children.Add(element);
            return element;
        }
        
        public void RemoveChild(UIDrawable element)
        {
            children.Remove(element);
        }
        
        public override void Draw(Device device, Matrix parentMatrix)
        {
            if (!isVisible) return;
            
            UpdateLocalTransformMatrix();
            
            Matrix combinedMatrix = localTransformMatrix * parentMatrix;
            
            //sort by draw priority
            children.Sort((a, b) => a.renderOrder.CompareTo(b.renderOrder));
            
            foreach (UIDrawable element in children)
            {
                if (element.isVisible)
                {
                    //draw elements at their position relative to the group
                    element.Draw(device, combinedMatrix);
                }
            }
        }

        public override void Dispose()
        {
            foreach (UIDrawable element in children)
            {
                element.Dispose();
            }
            
            children.Clear();
        }
    }
}