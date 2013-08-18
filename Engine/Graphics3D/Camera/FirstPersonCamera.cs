// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics3D.Camera
{
    /// <summary>
    /// A first person camera.
    /// </summary>
    public class FirstPersonCamera : BaseCamera
    {
        /// <summary>
        /// Create a first person camera with default values.
        /// </summary>
        public FirstPersonCamera()
            : base()
        {
            SetupCamera();
            _isDynamic = true;
        }

        /// <summary>
        /// Create a first person camera with a position
        /// </summary>
        /// <param name="position">A position</param>
        public FirstPersonCamera(Vector3 position)
        {
            _position = position;
            SetupCamera(_position, _target, 1.0f, 3500.0f);
        }

        /// <summary>
        /// Update the camera.
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