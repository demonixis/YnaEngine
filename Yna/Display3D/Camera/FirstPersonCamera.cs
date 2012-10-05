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

        }

        public FirstPersonCamera(Game game)
            : base(game)
        {

        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            Matrix matRotationY = Matrix.CreateRotationY(_yaw);

            Vector3 transformedReference = Vector3.Transform(_reference, matRotationY);

            _target = _position + transformedReference;

            _view = Matrix.CreateLookAt(_position, _target, Vector3.Up);
            _projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, Near, Far);
        }
    }
}