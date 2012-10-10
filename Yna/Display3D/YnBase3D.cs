using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display3D
{
    public abstract class YnBase3D
    {
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

        public YnBase3D()
        {
            _position = Vector3.Zero;
            _rotation = Vector3.Zero;
            _scale = Vector3.One;
        }

        /// <summary>
        /// Translate on X, Y and Z axis
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public virtual void Translate(float x, float y, float z)
        {
            Vector3 move = new Vector3(x, y, z);
            Matrix forwardMovement = Matrix.CreateRotationY(_rotation.Y);
            Vector3 v = Vector3.Transform(move, forwardMovement);
            _position.X += v.X;
            _position.Y += v.Y;
            _position.Z += v.Z;
        }

        public virtual void RotateY(float angle)
        {
            _rotation.Y += MathHelper.ToRadians(angle);

            if ((_rotation.Y >= MathHelper.Pi * 2) || (_rotation.Y <= -MathHelper.Pi * 2))
                _rotation.Y = 0.0f;
        }
    }
}
