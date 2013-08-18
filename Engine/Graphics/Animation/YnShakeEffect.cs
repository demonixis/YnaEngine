// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics.Animation
{
    /// <summary>
    /// A Shake effect for a SpriteBatchCamera
    /// </summary>
    public class YnShakeEffect : YnBasicEntity, IEffectAnimation
    {
        protected static readonly Random random = new Random();
        protected bool _shaking;
        protected float _shakeMagnitude;
        protected float _shakeDuration;
        protected float _shakeTimer;
        protected Vector3 _shakeOffset;
        protected YnCamera2D _camera;

        public YnShakeEffect(YnCamera2D camera)
        {
            _shaking = false;
            _shakeMagnitude = 0.0f;
            _shakeDuration = 0.0f;
            _shakeTimer = 0.0f;
            _shakeOffset = Vector3.Zero;
            _camera = camera;
        }

        /// <summary>
        /// Get a float in a range of -1.0f / 1.0f
        /// </summary>
        /// <returns></returns>
        private float NextFloat()
        {
            return (float)random.NextDouble() * 2.0f - 1.0f;
        }

        /// <summary>
        /// Shake the camera
        /// </summary>
        /// <param name="magnitude">Desired magnitude of the effect</param>
        /// <param name="duration">Desired duration</param>
        public void Shake(float magnitude, float duration)
        {
            if (!_shaking)
            {
                _shaking = true;

                _shakeMagnitude = magnitude;
                _shakeDuration = duration;

                _shakeTimer = 0.0f;
            }
        }

        /// <summary>
        /// Update the effect
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (_shaking)
            {
                _shakeTimer += (float)gameTime.ElapsedGameTime.Milliseconds;

                if (_shakeTimer >= _shakeDuration)
                {
                    _shaking = false;
                    _shakeTimer = _shakeDuration;
                    _camera.X = 0;
                    _camera.Y = 0;
                }
                else
                {
                    float progress = _shakeTimer / _shakeDuration;

                    float magnitude = _shakeMagnitude * (1.0f - (progress * progress));

                    _shakeOffset = new Vector3(NextFloat(), NextFloat(), NextFloat()) * magnitude;

                    _camera.X += (int)_shakeOffset.X;
                    _camera.Y += (int)_shakeOffset.Y;
                }
            }
        }
    }
}
