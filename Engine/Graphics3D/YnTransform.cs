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
            set { _position = value; }
        }

        public Vector3 Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public Vector3 Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        public YnTransform()
        {
            _position = Vector3.Zero;
            _rotation = Vector3.Zero;
            _scale = Vector3.Zero;
            _world = Matrix.Identity;
        }

        public void ApplyTransform(ref Matrix transformMatrix)
        {
            _world *= transformMatrix;
        }

        public void ApplyTransform(Matrix transformMatrix)
        {
            ApplyTransform(ref transformMatrix);
        }
    }
}
