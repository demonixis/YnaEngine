using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display3D.Camera
{
    public abstract class BaseCamera : YnBase3D
    {
        // Direction
        protected Vector3 _lastPosition;
        protected Vector3 _direction;
        protected Vector3 _lastDirection;
        private bool _dynamic;

        protected Matrix _projection;

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

        #region Properties

        /// <summary>
        /// Get the last position
        /// </summary>
        public Vector3 LastPosition
        {
            get { return _lastPosition; }
        }

        public Vector3 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public Vector3 LastDirection
        {
            get { return _lastDirection; }
        }

        public bool Dynamic
        {
            get { return _dynamic; }
            set { _dynamic = value; }
        }

        public Matrix Projection
        {
            get { return _projection; }
            set { _projection = value; }
        }

        public float Yaw
        {
            get { return _yaw; }
            set { _yaw = value; }
        }

        public float Near
        {
            get { return _nearClip; }
            set { _nearClip = value; }
        }

        public float Far
        {
            get { return _farClip; }
            set { _farClip = value; }
        }

        public Vector3 Reference
        {
            get { return _reference; }
            set { _reference = value; }
        }

        public Vector3 VectorUp
        {
            get { return _vectorUp; }
            set { _vectorUp = value; }
        }

        public Vector3 Target
        {
            get { return _target; }
            set { _target = value; }
        }

        public float AspectRatio
        {
            get { return _aspectRatio; }
            set { _aspectRatio = value; }
        }

        public float FieldOfView
        {
            get { return MathHelper.PiOver4; }
        }

        public BoundingSphere BoundingSphere
        {
            get { return _boundingSphere; }
            protected set { _boundingSphere = value; }
        }

        public float BoundingRadius
        {
            get { return _boundingRadius; }
            set { _boundingRadius = value; }
        }

        public BoundingBox BoundingBox
        {
            get { return _boundingBox; }
            protected set { _boundingBox = value; }
        }

        public BoundingFrustum BoundingFrustrum
        {
            get { return _boundingFrustrum; }
            protected set { _boundingFrustrum = value; }
        }

        #endregion

        public BaseCamera()
        {
            _aspectRatio = (float)((float)YnG.Width / (float)YnG.Height);
            _fieldOfView = MathHelper.PiOver4;
            _nearClip = 1.0f;
            _farClip = 3500.0f;

            _yaw = 0.0f;
            _pitch = 0.0f;
            _roll = 0.0f;

            _position = Vector3.Zero;
            _lastDirection = Vector3.Zero;
            _target = Vector3.Zero;
            _reference = Vector3.Zero;
            _vectorUp = Vector3.Up;

            _boundingRadius = 5;
            _boundingSphere = new BoundingSphere(Vector3.Zero, _boundingRadius);

            _boundingBox = new BoundingBox(
                new Vector3(X - _boundingRadius, Y - _boundingRadius, Z - _boundingRadius),
                new Vector3(X + _boundingRadius, Y + _boundingRadius, Z + _boundingRadius));

            _boundingFrustrum = new BoundingFrustum(Matrix.Identity);

            _lastPosition = _position;
            _direction = Vector3.Zero;
            _lastDirection = Vector3.Zero;
        }

        /// <summary>
        /// Initialize camera with default parameters
        /// </summary>
        public virtual void SetupCamera()
        {
            SetupCamera(new Vector3(0.0f, 0.0f, 5.0f), Vector3.Zero, 1.0f, 3500.0f);
        }

        /// <summary>
        /// Initialize camera
        /// </summary>
        /// <param name="position">World position</param>
        /// <param name="target">Targeted position</param>
        /// <param name="nearClip">Near vision</param>
        /// <param name="farClip">Far vision</param>
        public virtual void SetupCamera(Vector3 position, Vector3 target, float nearClip, float farClip)
        {
            _position = position;
            _reference = new Vector3(0.0f, 0.0f, 10.0f); // fix that
            _target = target;

            _yaw = 0.0f;
            _pitch = 0.0f;
            _roll = 0.0f;

            _nearClip = nearClip;
            _farClip = farClip;

            _view = Matrix.CreateLookAt(_position, _target, _vectorUp);
            
            _world = Matrix.Identity;

            UpdateProjection();
        }

        public void UpdateProjection()
        {
            _projection = Matrix.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, _nearClip, _farClip);
            _boundingFrustrum = new BoundingFrustum(_view * _projection);
        }

        /// <summary>
        /// Rotate the camera around Y axis
        /// </summary>
        /// <param name="angle">An angle in degree</param>
        public virtual void RotateY(float angle)
        {
            _yaw += MathHelper.ToRadians(angle);

            if ((_yaw >= MathHelper.Pi * 2) || (_yaw <= -MathHelper.Pi * 2))
                _yaw = 0.0f;
        }

        public virtual void Pitch(float angle)
        {
            _pitch += MathHelper.ToRadians(angle);

        }

        public virtual void Roll(float angle)
        {
            _roll += MathHelper.ToRadians(angle);
        }

        /// <summary>
        /// Translate the camera
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

        public virtual void UpdateBoundingVolumes()
        {
            // Update BoudingSphere
            _boundingSphere.Center = _position;
            _boundingSphere.Radius = _boundingRadius;

            // Update BoundingBox
            _boundingBox.Min.X = X - _boundingRadius;
            _boundingBox.Min.Y = Y - _boundingRadius;
            _boundingBox.Min.Z = Z - _boundingRadius;
            _boundingBox.Min.X = X + _boundingRadius;
            _boundingBox.Min.Y = Y + _boundingRadius;
            _boundingBox.Min.Z = Z + _boundingRadius;

            // Update Frustrum
            _boundingFrustrum.Matrix = View * Projection;
        }

        public override void Update(GameTime gameTime)
        {
            UpdateBoundingVolumes();

            _lastDirection = (_position - _lastPosition);
            _lastDirection.Normalize();

            _lastPosition = _position;
        }
    }
}
