using System;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics3D
{
    public class YnTransform
    {
        private Matrix _world;
        private Vector3 _rotation;
        private Vector3 _position;
        private Vector3 _scale;

        public Matrix World
        {
            get { return _world; }
            set { _world = value; }
        }

        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                UpdatePosition();
            }
        }

        public Vector3 Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                UpdateRotation();
            }
        }

        public Vector3 Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                UpdateScale();
            }
        }

        public YnTransform()
        {
            _position = Vector3.Zero;
            _rotation = Vector3.Zero;
            _scale = Vector3.Zero;
            _world = Matrix.Identity;
        }

        public void Translate(float x, float y, float z)
        {

        }

        public void Rotate(float rx, float ry, float rz)
        {

        }

        protected void UpdatePosition()
        {
            _world *= Matrix.CreateTranslation(_position);
        }

        protected void UpdateRotation()
        {
            _world *= Matrix.CreateFromYawPitchRoll(_rotation.Y, _rotation.X, _rotation.Z);
        }

        protected void UpdateScale()
        {
            _world *= Matrix.CreateScale(_scale);
        }

        public void Update(YnTransform parent)
        {
            // Set the matrix world to identify
            _world = Matrix.Identity;

            _world *= Matrix.CreateScale(_scale);

            // If a parent exists
            if (parent != null)
            {
                _world = parent.World;
                _world *= Matrix.CreateFromAxisAngle(parent.World.Right, _rotation.X);
                _world *= Matrix.CreateFromAxisAngle(parent.World.Up, _rotation.Y);
                _world *= Matrix.CreateFromAxisAngle(parent.World.Forward, _rotation.Z);
            }
            // Local transforms
            else
            {
                _world *= Matrix.CreateFromYawPitchRoll(_rotation.Y, _rotation.X, _rotation.Z);
            }

            _world *= Matrix.CreateTranslation(_position);
        }
    }
}
