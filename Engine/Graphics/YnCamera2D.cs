// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Yna.Engine.Graphics.Animation;

namespace Yna.Engine.Graphics
{
    /// <summary>
    /// A simple camera used on the scene to make different type of effects.
    /// Position, Rotation and Zoom can be applied on the scene.
    /// </summary>
    public class YnCamera2D : YnBasicEntity
    {
        // Avoid garbage generation because we use it on each update.
        private Matrix _originMatrix;
        private Matrix _rotationMatrix;
        private Matrix _zoomMatrix;
        private Matrix _translationMatrix;
        
        // Screen center
        protected Vector2 _centerScreen;

        // Effects
        protected YnShakeEffect _shakeEffect;

        #region Properties

        /// <summary>
        /// X position
        /// </summary>
        public int X;

        /// <summary>
        /// Y position
        /// </summary>
        public int Y;

        /// <summary>
        /// Rotation in degrees.
        /// </summary>
        public float Rotation;

        /// <summary>
        /// Zoom in/out the scene.
        /// </summary>
        public float Zoom;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a camera for the scene
        /// </summary>
        public YnCamera2D()
        {
            X = 0;
            Y = 0;
            Rotation = 0.0f;
            Zoom = 1.0f;

            _centerScreen = new Vector2(YnG.Width / 2, YnG.Height / 2);
            _shakeEffect = new YnShakeEffect(this);
        }

        #endregion

        /// <summary>
        /// Shake the camera
        /// </summary>
        /// <param name="magnitude">Desired magnitude</param>
        /// <param name="duration">Desired duration</param>
        public void Shake(float magnitude, long duration)
        {
            _shakeEffect.Shake(magnitude, duration);
        }

        /// <summary>
        /// Update the camera
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            _shakeEffect.Update(gameTime);
        }

        /// <summary>
        /// Get the transformed matrix
        /// </summary>
        /// <returns></returns>
        public Matrix GetTransformMatrix()
        {
            _originMatrix = Matrix.CreateTranslation(X + (-YnG.Width / 2), Y + (-YnG.Height / 2), 0);
            _rotationMatrix = Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation));
            _zoomMatrix = Matrix.CreateScale(Zoom);
            _translationMatrix = Matrix.CreateTranslation(X + (YnG.Width / 2), Y + (YnG.Height / 2), 0);

            return (_zoomMatrix * _originMatrix * _rotationMatrix * _translationMatrix);
        }
    }
}
