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
        private YnTransform _parent;

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
            _parent = null;
        }

        public YnTransform(YnTransform parent)
            : this()
        {
            _parent = parent;
        }

        public void Translate(float x, float y, float z)
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

        public void Update()
        {
            // Set the matrix world to identify
            _world = Matrix.Identity;

            _world *= Matrix.CreateScale(_scale);

            // If a parent exists
            if (_parent != null)
            {
                _world = _parent.World;
                _world *= Matrix.CreateFromAxisAngle(_parent.World.Right, _rotation.X);
                _world *= Matrix.CreateFromAxisAngle(_parent.World.Up, _rotation.Y);
                _world *= Matrix.CreateFromAxisAngle(_parent.World.Forward, _rotation.Z);
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
