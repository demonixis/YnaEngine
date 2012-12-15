using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Framework.Display3D.Camera
{
    public class FixedCamera : BaseCamera
    {
        #region Constructors

        public FixedCamera()
            : this(new Vector3(0.0f, 5.0f, -5.0f), new Vector3(0.0f, 0.0f, 0.0f))
        {
            
        }

        public FixedCamera(Vector3 position, Vector3 target)
        {
            _reference = new Vector3(0.0f, 0.0f, 10.0f);

            _position = position;

            _target = target;

            SetupCamera(_position, _target, 1.0f, 3500.0f);
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Matrix matRotation = Matrix.CreateRotationX(_pitch) * Matrix.CreateRotationY(_yaw);

            Vector3 transformedReference = Vector3.Transform(_reference, matRotation);

            _target = _position + transformedReference;

            _view = Matrix.CreateLookAt(_position, _target, _vectorUp);

            _world = Matrix.Identity;
        }
    }
}
