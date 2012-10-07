using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display3D.Camera
{
    public abstract class BaseCamera : YnBase3D
    {
        protected Matrix _projection;
        protected Matrix _world;

        // Rotation sur Y et X
        protected float _yaw;
        protected float _pich;

        // Paramètrage de la caméra
        protected float _nearClip;
        protected float _farClip;

        // Position, cible, placement
        protected Vector3 _reference;
        protected Vector3 _position;
        protected Vector3 _target;


        #region Propriétés
        public Matrix Projection
        {
            get { return _projection; }
            set { _projection = value; }
        }

        public Matrix World
        {
            get { return _world; }
            set { _world = value; }
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

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
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

        public void SetupCamera(Vector3 position, Vector3 target, float nearClip, float farClip)
        {
            _position = position;
            _reference = new Vector3(0.0f, 0.0f, 10.0f); // fix that
            _target = target;

            _yaw = 0.0f;
            _pich = 0.0f;

            _nearClip = nearClip;
            _farClip = farClip;

            _view = Matrix.CreateLookAt(_position, _target, Vector3.Up);

            _projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, Near, Far);
            
            _world = Matrix.Identity;
        }

        public void SetupCamera()
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

        public void PitchUp(float angle)
        {
            _pich -= MathHelper.ToRadians(angle);

        }

        public void PitchDown(float angle)
        {
            _pich += MathHelper.ToRadians(angle);
        }

        /// <summary>
        /// Translate the camera on X, Y and Z axis
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void Translate(float x, float y, float z)
        {
            Vector3 move = new Vector3(x, y, z);
            Matrix forwardMovement = Matrix.CreateRotationY(_yaw);
            Vector3 v = Vector3.Transform(move, forwardMovement);
            _position.X += v.X;
            _position.Y += v.Y;
            _position.Z += v.Z;
        }

        public abstract void Update(GameTime gameTime);
    }
}
