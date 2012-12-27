﻿using System;
using Microsoft.Xna.Framework;

namespace Yna.Framework.Display
{
    /// <summary>
    /// A simple camera used on the scene to make different type of effects.
    /// Position, Rotation and Zoom can be applied on the scene.
    /// </summary>
    public class SpriteBatchCamera
    {
        protected Matrix _view;
        protected int _x;
        protected int _y;
        protected float _rotation;
        protected float _zoom;
        protected Vector2 _centerScreen;

        #region Properties

        /// <summary>
        /// Get or set the position on X axis
        /// </summary>
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Get or set the position on Y axis
        /// </summary>
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Get or set the rotation angle (in degree)
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        /// <summary>
        /// Get or set the zoom factor
        /// </summary>
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a camera for the scene
        /// </summary>
        public SpriteBatchCamera()
        {
            _view = Matrix.Identity;
            _x = 0;
            _y = 0;
            _rotation = 0.0f;
            _zoom = 1.0f;

            _centerScreen = new Vector2(YnG.Width / 2, YnG.Height / 2);
        }

        #endregion

        /// <summary>
        /// Get the transformed matrix
        /// </summary>
        /// <returns></returns>
        public Matrix GetTransformMatrix()
        {
            Matrix translateToOrigin = Matrix.CreateTranslation(_x + (-YnG.Width / 2), _y + (-YnG.Height / 2), 0);
            Matrix rotation = Matrix.CreateRotationZ(MathHelper.ToRadians(_rotation));
            Matrix zoom = Matrix.CreateScale(_zoom);
            Matrix translateBackToPosition = Matrix.CreateTranslation(_x + (YnG.Width / 2), _y + (YnG.Height / 2), 0);
            Matrix composition = zoom * translateToOrigin * rotation * translateBackToPosition;

            return composition;
        }
    }
}