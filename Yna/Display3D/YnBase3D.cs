using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display3D
{
    public abstract class YnBase3D
    {
        protected Game _game;
        protected Vector3 _position;
        protected Vector3 _rotation;
        protected Vector3 _scale;

        protected Matrix _view;

        #region Properties for position/rotation/scale

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

        public float RotationX
        {
            get { return _rotation.X; }
            set { _rotation.X = value; }
        }

        public float RotationY
        {
            get { return _rotation.Y; }
            set { _rotation.Y = value; }
        }

        public float RotationZ
        {
            get { return _rotation.Z; }
            set { _rotation.Z = value; }
        }

        public float PositionX
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        public float PositionY
        {
            get { return _position.Y; }
            set { _position.Y = value; }
        }

        public float PositionZ
        {
            get { return _position.Z; }
            set { _position.Z = value; }
        }

#endregion

        public Matrix View
        {
            get { return _view; }
            set { _view = value; }
        }

        public Game Game
        {
            get { return _game; }
            set { _game = value; }
        }

        public YnBase3D()
        {
            _position = Vector3.Zero;
            _rotation = Vector3.Zero;
            _scale = Vector3.One;
        }
    }
}
