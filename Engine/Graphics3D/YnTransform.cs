// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
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

        public Matrix Transform
        {
            get { return GetTransform(); }
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
            Vector3 move = new Vector3(x, y, z);
            Matrix forwardMovement = Matrix.CreateRotationY(_rotation.Y);
            Vector3 tVector = Vector3.Transform(move, forwardMovement);
            _position += tVector;
        }

        public void Rotate(float rx, float ry, float rz)
        {
            _rotation.X = rx;
            _rotation.Y = ry;
            _rotation.Z = rz;
        }

        public void Turn(float rx, float ry, float rz)
        {
            _rotation.X += rx;
            _rotation.Y += ry;
            _rotation.Z += rz;
        }

        public void Scale(float sx, float sy, float sz)
        {
            _scale.X = sx;
            _scale.Y = sy;
            _scale.Z = sz;
        }

        public Matrix GetTransform()
        {
            Matrix transform = Matrix.CreateScale(_scale) * 
                Matrix.CreateFromYawPitchRoll(_rotation.Y, _rotation.X, _rotation.Z) * 
                Matrix.CreateTranslation(_position);

            return transform;
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
