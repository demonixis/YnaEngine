using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display3D
{
    public abstract class YnBase3D : YnBase
    {
        protected Vector3 _position;
        protected Vector3 _rotation;
        protected Vector3 _scale;
        protected Matrix _view;
        protected Matrix _world;

        #region Properties for position/rotation/scale

        /// <summary>
        /// Get or Set the position value
        /// </summary>
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Get or Set the rotation value
        /// </summary>
        public Vector3 Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        /// <summary>
        /// Get or Set the scale value
        /// </summary>
        public Vector3 Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        /// <summary>
        /// Get or Set the rotation value on X axis
        /// </summary>
        public float RotationX
        {
            get { return _rotation.X; }
            set { _rotation.X = value; }
        }

        /// <summary>
        /// Get or Set the rotation value on Y axis
        /// </summary>
        public float RotationY
        {
            get { return _rotation.Y; }
            set { _rotation.Y = value; }
        }

        /// <summary>
        /// Get or Set the rotation value on Z axis
        /// </summary>
        public float RotationZ
        {
            get { return _rotation.Z; }
            set { _rotation.Z = value; }
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
        /// Get or Set the view matrix
        /// </summary>
        public Matrix View
        {
            get { return _view; }
            set { _view = value; }
        }

        /// <summary>
        /// Get or Set the World matrix of the object
        /// </summary>
        public Matrix World
        {
            get { return _world; }
            set { _world = value; }
        }

        #endregion

        public YnBase3D()
        {
            _position = Vector3.Zero;
            _rotation = Vector3.Zero;
            _scale = Vector3.One;
            _view = Matrix.Identity;
            _world = Matrix.Identity;
        }

        public override void Update(GameTime gameTime)
        {
           
        }
    }
}
