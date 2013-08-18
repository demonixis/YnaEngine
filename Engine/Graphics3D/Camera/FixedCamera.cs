// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics3D.Camera
{
    public class FixedCamera : BaseCamera
    {
        /// <summary>
        /// Create a fixed camera with default values
        /// </summary>
        public FixedCamera()
            : this(new Vector3(0.0f), new Vector3(0.0f, 0.0f, 0.0f))
        {
            
        }

        /// <summary>
        /// Create a fixed camera with an initiale position and a target.
        /// </summary>
        /// <param name="position">The initial position.</param>
        /// <param name="target">The initial target.</param>
        public FixedCamera(Vector3 position, Vector3 target)
            : this(position, target, new Vector3(0.0f, 0.0f, 10.0f))
        {
            
        }

        /// <summary>
        /// Create a fixed camera with an initiale position and a target.
        /// </summary>
        /// <param name="position">Initial position.</param>
        /// <param name="target">Initial target.</param>
        /// <param name="reference">Initial reference.</param>
        public FixedCamera(Vector3 position, Vector3 target, Vector3 reference)
        {
            _reference = reference;
            _position = position;
            _target = target;

            SetupCamera(_position, _target, 1.0f, 3500.0f);
        }

        /// <summary>
        /// Update the camera.
        /// </summary>
        /// <param name="gameTime"></param>
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
