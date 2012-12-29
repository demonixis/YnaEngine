using System;
using Yna.Framework;
using Microsoft.Xna.Framework;

namespace Yna.Framework.Display.Animation
{
    /// <summary>
    /// A simple rotate effect for SpriteBatchCamera
    /// </summary>
    public class YnRotateEffect : YnBase
    {
        private bool _rotate;
        private float _rotateDuration;
        private int _rotateDirection;
        private float _rotateTimer;
        private SpriteBatchCamera _camera;

        public YnRotateEffect(SpriteBatchCamera camera)
        {
            _rotate = false;
            _rotateDirection = 1;
            _rotateDuration = 0.0f;
            _rotateTimer = 0.0f;
            _camera = camera;
        }

        /// <summary>
        /// Rotate the camera
        /// </summary>
        /// <param name="direction">Direction of rotation, -1 or 1</param>
        /// <param name="duration">Desired duration</param>
        public void Rotate(int direction, float duration)
        {
            if (!_rotate)
            {
                _rotate = true;
                _rotateDirection = direction >= 1 ? 1 : -1;
                _rotateDuration = duration;
                _rotateTimer = 0.0f;
            }
        }

        /// <summary>
        /// Update the effect
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (_rotate)
            {
                _rotateTimer += (float)gameTime.ElapsedGameTime.Milliseconds;

                if (_camera.Rotation > 360)
                {
                    _rotate = false;
                    _rotateTimer = _rotateDuration;
                    _camera.Rotation = 0.0f;
                }
                else
                {
                    float progress = _rotateTimer / _rotateDuration;
                    _camera.Rotation = ((progress * 360) / (_rotateDuration / 3600)) * _rotateDirection;
                }
            }
        }
    }
}
