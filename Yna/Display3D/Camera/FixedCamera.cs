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

        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            _view = Matrix.CreateLookAt(_position, _reference, Vector3.Up);

            _projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, Near, Far);
        }
    }
}
