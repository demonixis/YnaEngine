using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display3D.Camera
{
    public class FirstPersonCamera : BaseCamera
    {
        #region Constructors

        public FirstPersonCamera()
            : base()
        {
            SetupCamera();
        }

        public FirstPersonCamera(Vector3 position)
        {
            _position = position;

            SetupCamera(_position, _target, 1.0f, 3500.0f);
        }

        #endregion

        /// <summary>
        /// Update the position of the camera
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Matrix matRotation = Matrix.CreateFromYawPitchRoll(_yaw, _pitch, _roll);

            Vector3 transformedReference = Vector3.Transform(_reference, matRotation);

            _target = _position + transformedReference;

            _view = Matrix.CreateLookAt(_position, _target, _vectorUp);

            _world = Matrix.Identity;
        }
    }
}