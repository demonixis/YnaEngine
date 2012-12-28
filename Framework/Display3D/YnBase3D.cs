using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Framework.Display3D
{
    public abstract class YnBase3D : YnBase
    {
        protected Vector3 _position;
        protected Matrix _world;

        #region Properties for position/rotation/scale and world matrix

        /// <summary>
        /// Get or Set the position value
        /// </summary>
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
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
        /// Get or Set the World matrix of the object
        /// </summary>
        public Matrix World
        {
            get { return _world; }
            set { _world = value; }
        }

        #endregion

        public YnBase3D()
            : base()
        {
            _position = Vector3.Zero;
            _world = Matrix.Identity;
        }

        public override void Update(GameTime gameTime)
        {
           
        }
    }
}
