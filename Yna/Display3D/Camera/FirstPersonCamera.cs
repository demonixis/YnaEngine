﻿using System;
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

        #endregion

        /// <summary>
        /// Update the position of the camera
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Matrix matRotation = Matrix.CreateRotationX(_pich) * Matrix.CreateRotationY(_yaw);

            Vector3 transformedReference = Vector3.Transform(_reference, matRotation);

            _target = _position + transformedReference;

            _view = Matrix.CreateLookAt(_position, _target, Vector3.Up);
            _projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, Near, Far);
        }
    }
}