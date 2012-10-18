using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display3D.Camera
{
    public class FixedCamera : BaseCamera
    {
        #region Constructors

        public FixedCamera()
            : base()
        {
            _position = new Vector3(0.0f, 5.0f, -5.0f);
            _target = new Vector3(0.0f, 0.0f, 0.0f);
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            Matrix matRotation = Matrix.CreateRotationX(_pitch) * Matrix.CreateRotationY(_yaw);

            Vector3 transformedReference = Vector3.Transform(_reference, matRotation);

            Vector3 target = _position + transformedReference;

            _view = Matrix.CreateLookAt(_position, target, Vector3.Up);
            
            _projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, Near, Far);

            _world = Matrix.Identity;
        }
    }
}
