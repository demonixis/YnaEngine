using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display3D.Camera
{
    public abstract class BaseCamera : YnBase3D
    {
        protected Matrix _projection;

        // Rotation X/Y/Z
        protected float _yaw;
        protected float _pitch;
        protected float _roll;

        // Paramètrage de la caméra
        protected float _nearClip;
        protected float _farClip;

        // Position, cible, placement
        protected Vector3 _reference;
        protected Vector3 _target;


        #region Properties

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

        public Vector3 Target
        {
            get { return _target; }
            set { _target = value; }
        }

        public float AspectRatio
        {
            get { return YnG.Width / YnG.Height; }
        }

        public float FieldOfView
        {
            get { return MathHelper.PiOver4; }
        }

        #endregion

        public BaseCamera()
        {
            SetupCamera();
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

            _nearClip = nearClip;
            _farClip = farClip;

            _view = Matrix.CreateLookAt(_position, target, Vector3.Up);

            _projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, Near, Far);
            
            _world = Matrix.Identity;
        }

        /// <summary>
        /// Initialize camera with default parameters
        /// </summary>
        public virtual void SetupCamera()
        {
            SetupCamera(new Vector3(0.0f, 0.0f, 5.0f), Vector3.Zero, 1.0f, 3500.0f);
        }

        /// <summary>
        /// Rotate the camera around Y axis
        /// </summary>
        /// <param name="angle">An angle in degree</param>
        public void RotateY(float angle)
        {
            _yaw += MathHelper.ToRadians(angle);

            if ((_yaw >= MathHelper.Pi * 2) || (_yaw <= -MathHelper.Pi * 2))
                _yaw = 0.0f;
        }

        public void Pitch(float angle)
        {
            _pitch += MathHelper.ToRadians(angle);

        }

        public void Roll(float angle)
        {
            _roll += MathHelper.ToRadians(angle);
        }

        /// <summary>
        /// Translate the camera
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        /// <param name="z">Z value</param>
        public void Translate(float x, float y, float z)
        {
            Vector3 move = new Vector3(x, y, z);
            Matrix forwardMovement = Matrix.CreateRotationX(_pitch) * Matrix.CreateRotationY(_yaw) * Matrix.CreateRotationZ(_roll);
            Vector3 v = Vector3.Transform(move, forwardMovement);
            
            _position.X += v.X;
            _position.Y += v.Y;
            _position.Z += v.Z;
        }
    }
}
