// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics3D.Camera
{
    /// <summary>
    /// Define a basic camera used to view a 3D scene
    /// </summary>
    public abstract class BaseCamera : YnBasicEntity
    {
        #region Protected declarations

        // Position and Direction
        protected Vector3 _position;
        protected Vector3 _direction;
        protected Vector3 _previousDirection;
        protected Vector3 _previousPosition;
        protected bool _isDynamic;

        // Matrix
        protected Matrix _world;
        protected Matrix _projection;
        protected Matrix _view;

        // Bounding volume
        protected BoundingSphere _boundingSphere;
        protected float _boundingRadius;
        protected BoundingBox _boundingBox;
        protected BoundingFrustum _boundingFrustrum;

        // Rotation X/Y/Z
        protected float _yaw;
        protected float _pitch;
        protected float _roll;

        // View parameters
        protected float _fieldOfView;
        protected float _aspectRatio;
        protected float _nearClip;
        protected float _farClip;

        // target, placement
        protected Vector3 _reference;
        protected Vector3 _target;
        protected Vector3 _vectorUp;

        #endregion

        #region Porperties for position and direction

        /// <summary>
        /// Get or Set the position value
        /// </summary>
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Get or Set the position on X axis
        /// </summary>
        public float X
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        /// <summary>
        /// Get or Set the position on Y axis
        /// </summary>
        public float Y
        {
            get { return _position.Y; }
            set { _position.Y = value; }
        }

        /// <summary>
        /// Get or Set the position on Z axis
        /// </summary>
        public float Z
        {
            get { return _position.Z; }
            set { _position.Z = value; }
        }

        /// <summary>
        /// Get the last position
        /// </summary>
        public Vector3 PreviousPosition
        {
            get { return _previousPosition; }
        }

        /// <summary>
        /// Get the direction of the camera
        /// </summary>
        public Vector3 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        /// <summary>
        /// Get the last direction
        /// </summary>
        public Vector3 PreviousDirection
        {
            get { return _previousDirection; }
        }

        #endregion

        #region Properties for rotations

        /// <summary>
        /// Gets or sets the yaw value that is used to rotate the camera arround Y axis
        /// </summary>
        public float Yaw
        {
            get { return _yaw; }
            set { _yaw = value; }
        }

        /// <summary>
        /// Gets or sets the pitch value that is used to rotate the camera arround X axis
        /// </summary>
        public float Pitch
        {
            get { return _pitch; }
            set { _pitch = value; }
        }

        /// <summary>
        /// Gets or sets the roll value that is used to rotate the camera arround Z axis
        /// </summary>
        public float Roll
        {
            get { return _roll; }
            set { _roll = value; }
        }

        #endregion

        #region Properties for Matrix

        /// <summary>
        /// Get or Set the World matrix of the object
        /// </summary>
        public Matrix World
        {
            get { return _world; }
            set { _world = value; }
        }

        /// <summary>
        /// Gets or sets the projection matrix
        /// </summary>
        public Matrix Projection
        {
            get { return _projection; }
            set { _projection = value; }
        }

        /// <summary>
        /// Gets or sets the view matrix
        /// </summary>
        public Matrix View
        {
            get { return _view; }
            set { _view = value; }
        }

        /// <summary>
        /// Gets the view project matrix
        /// </summary>
        public Matrix MatrixViewProjection
        {
            get { return _view * _projection; }
        }

        /// <summary>
        /// Gets the world view projection matrix
        /// </summary>
        public Matrix MatrixWorldViewProject
        {
            get { return _world * (_view * _projection); }
        }

        /// <summary>
        /// Define if the camera is dynamic or not, if dynamic it will be updated on each frame
        /// </summary>
        public bool IsDynamic
        {
            get { return _isDynamic; }
            set { _isDynamic = value; }
        }

        #endregion

        #region Properties for view and target

        /// <summary>
        /// Gets or sets the nearest value that the camera can look
        /// </summary>
        public float Near
        {
            get { return _nearClip; }
            set { _nearClip = value; }
        }

        /// <summary>
        /// Gets or sets the value closest to the camera can see
        /// </summary>
        public float Far
        {
            get { return _farClip; }
            set { _farClip = value; }
        }

        /// <summary>
        /// Gets or sets the reference point of the camera
        /// </summary>
        public Vector3 Reference
        {
            get { return _reference; }
            set { _reference = value; }
        }

        /// <summary>
        /// Gets or sets the up vector
        /// </summary>
        public Vector3 VectorUp
        {
            get { return _vectorUp; }
            set { _vectorUp = value; }
        }

        /// <summary>
        /// Gets or sets the target point of the camera
        /// </summary>
        public Vector3 Target
        {
            get { return _target; }
            set { _target = value; }
        }

        /// <summary>
        /// Gets or sets the aspect ratio
        /// </summary>
        public float AspectRatio
        {
            get { return _aspectRatio; }
            set { _aspectRatio = value; }
        }

        /// <summary>
        /// Gets or sets the field of view
        /// </summary>
        public float FieldOfView
        {
            get { return _fieldOfView; }
            set { _fieldOfView = value; }
        }

        #endregion

        #region Properties bounding volume

        /// <summary>
        /// Get the boundingSphere of the camera
        /// </summary>
        public BoundingSphere BoundingSphere
        {
            get { return _boundingSphere; }
            set { _boundingSphere = value; }
        }

        /// <summary>
        /// Gets or sets the bounding raduis of the camera
        /// </summary>
        public float BoundingRadius
        {
            get { return _boundingRadius; }
            set { _boundingRadius = value; }
        }

        /// <summary>
        /// Get the boundingBox of the camera
        /// </summary>
        public BoundingBox BoundingBox
        {
            get { return _boundingBox; }
            set { _boundingBox = value; }
        }

        /// <summary>
        /// Get the bounding frustrum of the camera
        /// </summary>
        public BoundingFrustum BoundingFrustrum
        {
            get { return _boundingFrustrum; }
            set { _boundingFrustrum = value; }
        }

        #endregion

        public BaseCamera()
        {
            // Position & direction
            _position = Vector3.Zero;
            _previousPosition = _position;
            _direction = Vector3.Zero;
            _previousDirection = Vector3.Zero;

            // Rotations
            _yaw = 0.0f;
            _pitch = 0.0f;
            _roll = 0.0f;

            // References
            _target = Vector3.Zero;
            _reference = Vector3.Zero;
            _vectorUp = Vector3.Up;

            // Screen view
            _aspectRatio = (float)((float)YnG.Width / (float)YnG.Height);
            _fieldOfView = MathHelper.PiOver4;
            _nearClip = 1.0f;
            _farClip = 3500.0f;

            // Bounding volumes
            _boundingRadius = 5;
            _boundingSphere = new BoundingSphere(Vector3.Zero, _boundingRadius);

            _boundingBox = new BoundingBox(
                new Vector3(X - _boundingRadius, Y - _boundingRadius, Z - _boundingRadius),
                new Vector3(X + _boundingRadius, Y + _boundingRadius, Z + _boundingRadius));

            _boundingFrustrum = new BoundingFrustum(Matrix.Identity);

            // Basic matrix init
            _projection = Matrix.Identity;
            _view = Matrix.Identity;
            _world = Matrix.Identity;
        }

        /// <summary>
        /// Setup the camera with default values
        /// </summary>
        public virtual void SetupCamera()
        {
            SetupCamera(new Vector3(0.0f, 0.0f, 5.0f), Vector3.Zero, 1.0f, 3500.0f);
        }

        /// <summary>
        /// Setup the camera.
        /// </summary>
        /// <param name="position">World position</param>
        /// <param name="target">Target position</param>
        /// <param name="nearClip">Near vision</param>
        /// <param name="farClip">Far vision</param>
        public virtual void SetupCamera(Vector3 position, Vector3 target, float nearClip, float farClip)
        {
            _position = position;
            _reference = new Vector3(0.0f, 0.0f, 10.0f);
            _target = target;
            _yaw = 0.0f;
            _pitch = 0.0f;
            _roll = 0.0f;
            _nearClip = nearClip;
            _farClip = farClip;

            _view = Matrix.CreateLookAt(_position, _target, _vectorUp);
            _world = Matrix.Identity;

            UpdateProjection();

            _boundingFrustrum.BuildBoundingFrustum(this);
        }

        /// <summary>
        /// Update project matrix and frustrum matrix
        /// </summary>
        public virtual void UpdateProjection()
        {
            _projection = Matrix.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, _nearClip, _farClip);
            _boundingFrustrum.Matrix = MatrixViewProjection; 
        }

        /// <summary>
        /// Rotate the camera around X axis.
        /// </summary>
        /// <param name="angle">An angle in degree.</param>
        public virtual void RotateX(float angle)
        {
            _pitch += MathHelper.ToRadians(angle);
            _pitch = _pitch <= -1.0f ? -1.0f : _pitch;
            _pitch = _pitch >= 1.0f ? 1.0f : _pitch;
        }

        /// <summary>
        /// Rotate the camera around Y axis.
        /// </summary>
        /// <param name="angle">An angle in degree.</param>
        public virtual void RotateY(float angle)
        {
            _yaw += MathHelper.ToRadians(angle);

            if ((_yaw >= MathHelper.Pi * 2) || (_yaw <= -MathHelper.Pi * 2))
                _yaw = 0.0f;
        }

        /// <summary>
        /// Rotate the camera around Z axis.
        /// </summary>
        /// <param name="angle">An angle in degree.</param>
        public virtual void RotateZ(float angle)
        {
            _roll += MathHelper.ToRadians(angle);
        }

        /// <summary>
        /// Rotate the camera. (Degree)
        /// </summary>
        /// <param name="rx">Angle for X axis (degrees)</param>
        /// <param name="ry">Angle for Y axis (degrees)</param>
        /// <param name="rz">Angle for Z axis (degrees)</param>
        public virtual void Rotate(float rx, float ry, float rz)
        {
            _pitch += MathHelper.ToRadians(rx);
            _yaw += MathHelper.ToRadians(ry);
            _roll += MathHelper.ToRadians(rz); 
        }

        /// <summary>
        /// Rotate the camera. (Radians)
        /// </summary>
        /// <param name="rotation">Vector to use for rotation.</param>
        public virtual void Rotate(Vector3 rotation)
        {
            Rotate(ref rotation);
        }

        /// <summary>
        /// Rotate the camera. (Radians)
        /// </summary>
        /// <param name="rotation">Vector to use for rotation.</param>
        public virtual void Rotate(ref Vector3 rotation)
        {
            _pitch += rotation.X;
            _yaw += rotation.Y;
            _roll += rotation.Z;
        }

        /// <summary>
        /// Sets the rotation. (degrees)
        /// </summary>
        /// <param name="rx">Angle for X axis (degrees)</param>
        /// <param name="ry">Angle for Y axis (degrees)</param>
        /// <param name="rz">Angle for Z axis (degrees)</param>
        public virtual void SetRotation(float rx, float ry, float rz)
        {
            _pitch = MathHelper.ToRadians(rx);
            _yaw = MathHelper.ToRadians(ry);
            _roll = MathHelper.ToRadians(rz); 
        }

        /// <summary>
        /// Sets the rotation. (Radians)
        /// </summary>
        /// <param name="rotation">Vector to use for rotation.</param>
        public virtual void SetRotation(Vector3 rotation)
        {
            SetRotation(ref rotation);
        }

        /// <summary>
        /// Sets the rotation. (Radians)
        /// </summary>
        /// <param name="rotation">Vector to use for rotation.</param>
        public virtual void SetRotation(ref Vector3 rotation)
        {
            _pitch = MathHelper.ToRadians(rotation.X);
            _yaw = MathHelper.ToRadians(rotation.Y);
            _roll = MathHelper.ToRadians(rotation.Z);
        }

        /// <summary>
        /// Translate the camera. Values are transformed and added to the current position.
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="z">Z value</param>
        public virtual void Translate(float x, float y, float z)
        {
            Vector3 move = new Vector3(x, y, z);
            Matrix forwardMovement = Matrix.CreateRotationY(_yaw);
            Vector3 v = Vector3.Transform(move, forwardMovement);

            _position.X += v.X;
            _position.Y += v.Y;
            _position.Z += v.Z;
        }

        /// <summary>
        /// Translate the camera. Values are transformed and added to the current position.
        /// </summary>
        /// <param name="translation">Translation vector to use.</param>
        public virtual void Translate(Vector3 translation)
        {
            Translate(ref translation);
        }

        /// <summary>
        /// Translate the camera. Values are transformed and added to the current position.
        /// </summary>
        /// <param name="translation">Translation vector to use.</param>
        public virtual void Translate(ref Vector3 translation)
        {
            Translate(translation.X, translation.Y, translation.Z);
        }

        /// <summary>
        /// Move the camera. Values are transformed and replace the current position.
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="z">Z value</param>
        public virtual void Move(float x, float y, float z)
        {
            Vector3 move = new Vector3(x, y, z);
            Matrix forwardMovement = Matrix.CreateRotationY(_yaw);
            Vector3 v = Vector3.Transform(move, forwardMovement);

            _position.X = v.X;
            _position.Y = v.Y;
            _position.Z = v.Z;
        }

        /// <summary>
        /// Move the camera. Values are transformed and replace the current position.
        /// </summary>
        /// <param name="moveVector">Vector to use.</param>
        public virtual void Move(Vector3 moveVector)
        {
            Move(ref moveVector);
        }

        /// <summary>
        /// Move the camera. Values are transformed and replace the current position.
        /// </summary>
        /// <param name="moveVector">Vector to use.</param>
        public virtual void Move(ref Vector3 moveVector)
        {
            Move(moveVector.X, moveVector.Y, moveVector.Z);
        }

        public virtual void AddToReference(float rx, float ry, float rz)
        {
            _reference.X += rx;
            _reference.Y += ry;
            _reference.Z += rz;
        }

        /// <summary>
        /// Sets the camera to front view.
        /// </summary>
		public virtual void FrontView()
		{
			_pitch = 0;
			_yaw = 0;
			_roll = 0;
		}

        /// <summary>
        /// Sets the camera to back view.
        /// </summary>
		public virtual void BackView()
		{
			_pitch = 0;
			_yaw = (float)MathHelper.PiOver2;
			_roll = 0;
		}

        /// <summary>
        /// Sets the camera to left view.
        /// </summary>
		public virtual void LeftView()
		{
			_pitch = 0;
			_yaw = (float)-MathHelper.PiOver2;
			_roll = 0;
		}

        /// <summary>
        /// Sets the camera to right view.
        /// </summary>
		public virtual void RightView()
		{
			_pitch = 0;
			_yaw = (float)MathHelper.PiOver2;
			_roll = 0;
		}

        /// <summary>
        /// Sets the camera to top view.
        /// </summary>
		public virtual void TopView()
		{
			_pitch = (float)-MathHelper.PiOver2;
			_yaw = 0;
			_roll = 0;
		}

        /// <summary>
        /// Sets the camera to bottom view.
        /// </summary>
		public virtual void BottomView()
		{
			_pitch = (float)MathHelper.PiOver2;
			_yaw = 0;
			_roll = 0;
		}

        /// <summary>
        /// Update BoundingBox, BoundingSphere and BoundingFrustrum. It's called by Update method.
        /// </summary>
        public virtual void UpdateBoundingVolumes()
        {
            // Update BoudingSphere
            _boundingSphere.Center = _position;
            _boundingSphere.Radius = _boundingRadius;

            // Update BoundingBox
            _boundingBox.Min.X = X - _boundingRadius;
            _boundingBox.Min.Y = Y - _boundingRadius;
            _boundingBox.Min.Z = Z - _boundingRadius;
            _boundingBox.Max.X = X + _boundingRadius;
            _boundingBox.Max.Y = Y + _boundingRadius;
            _boundingBox.Max.Z = Z + _boundingRadius;

            // Update Frustrum
            _boundingFrustrum.Matrix = MatrixWorldViewProject;
        }

        /// <summary>
        /// Update the camera.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (_isDynamic)
            {
                UpdateBoundingVolumes();
                _previousDirection = _direction;
                _direction = _position - _previousPosition;
                _previousPosition = _position;
            }
        }
    }
}
