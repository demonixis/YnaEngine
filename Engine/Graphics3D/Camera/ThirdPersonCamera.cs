// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics3D.Camera
{
    public class ThirdPersonCamera : BaseCamera
    {
        private YnEntity3D _followedObject;

        /// <summary>
        /// Gets or sets the objects followed by the camera.
        /// </summary>
        public YnEntity3D FollowedObject
        {
            get { return _followedObject; }
            set { _followedObject = value; }
        }

        public float Angle
        {
            get { return _reference.Y; }
            set { _reference.Y = value; }
        }

        public float Distance
        {
            get { return _reference.Z; }
            set { _reference.Z = value; }
        }

        #region Constructors

        /// <summary>
        /// Create a new third person camera without entity3D to follow. Use Follow property to attach an object to follow.
        /// </summary>
        public ThirdPersonCamera()
            : this(null)
        {

        }

        /// <summary>
        /// Create a third person camera with an entity3D and default values
        /// </summary>
        /// <param name="entity3D">The object to follow.</param>
        public ThirdPersonCamera(YnEntity3D entity3D)
            : this(entity3D, new Vector3(0, 10, 80))
        {

        }

        /// <summary>
        /// Create a third person camera with an entity3D to follow and values for distance.
        /// </summary>
        /// <param name="entity3D">The object to follow</param>
        /// <param name="reference">Initial reference.</param>
        public ThirdPersonCamera(YnEntity3D entity3D, Vector3 reference)
        {
            _reference = reference;
            _followedObject = entity3D;
            _isDynamic = true;
            SetupCamera();
        }

        #endregion

        /// <summary>
        /// Setup the camera with default values
        /// </summary>
        public override void SetupCamera()
        {
            SetupCamera(new Vector3(0.0f, 0.0f, 5.0f), Vector3.Zero, 1.0f, 3500.0f);
        }

        /// <summary>
        /// Setup the camera
        /// </summary>
        /// <param name="position">Initiale position</param>
        /// <param name="target">Target</param>
        /// <param name="nearClip">Near value</param>
        /// <param name="farClip">Far value</param>
        public override void SetupCamera(Vector3 position, Vector3 target, float nearClip, float farClip)
        {
            _position = position;
            _target = target;
            _yaw = 0.0f;
            _pitch = 0.0f;
            _nearClip = nearClip;
            _farClip = farClip;

            _view = Matrix.CreateLookAt(_position, _target, Vector3.Up);
            _projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, Near, Far);
            _world = Matrix.Identity;
        }

        /// <summary>
        /// Update the camera.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Matrix matRotation = Matrix.CreateFromYawPitchRoll(_yaw, _pitch, _roll) * Matrix.CreateRotationY(_followedObject.Rotation.Y);
            Vector3 transformedReference = Vector3.Transform(_reference, matRotation);
            Vector3 position = transformedReference + _followedObject.Position;

            _view = Matrix.CreateLookAt(position, _followedObject.Position, Vector3.Up);
            _projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, Near, Far);
            _world = Matrix.Identity;
        }
    }
}
