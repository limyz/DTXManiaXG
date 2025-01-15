using System;
using SharpDX;
using SharpDX.Direct3D9;

namespace DTXMania.Code.UI
{
    public abstract class UIDrawable : IDisposable
    {
        public int renderOrder = 0;
        public Vector3 position = Vector3.Zero;
        public Vector2 anchor = Vector2.Zero; //pivot in 2D space
        public Vector2 size = Vector2.One;    //scale in 3D space
        public Vector3 scale = Vector3.One;
        public Vector3 rotation = Vector3.Zero; //euler angles in radians (X = pitch, Y = yaw, Z = roll)
        
        public bool isVisible = true;
        
        protected Matrix localTransformMatrix = Matrix.Identity;

        public void UpdateLocalTransformMatrix()
        {
            Vector3 anchorOffset = new Vector3(-anchor.X * size.X, -anchor.Y * size.Y, 0);
            
            Matrix translationMatrix = Matrix.Translation(position);
            Matrix rotationMatrix = Matrix.RotationYawPitchRoll(rotation.Y, rotation.X, rotation.Z);
            Matrix scaleMatrix = Matrix.Scaling(scale);
            Matrix anchorMatrix = Matrix.Translation(anchorOffset);

            //combine transformations: anchor * scale * rotation * translation
            localTransformMatrix = scaleMatrix * rotationMatrix * anchorMatrix * translationMatrix;
        }
        
        public abstract void Draw(Device device, Matrix parentMatrix);
        
        public abstract void Dispose();
    }
}