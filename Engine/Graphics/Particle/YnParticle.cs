// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics.Particle
{
    /// <summary>
    /// Define a configuration for a particle.
    /// </summary>
    public struct ParticleConfiguration
    {
        public float Speed;
        public bool EnabledRotation;
        public float RotationIncrement;
        public int Width;
        public int Height;
        public int LifeTime;
    }

    public class YnParticle
    {
        private Vector2 _direction;
        private Vector2 _origin;
        private Rectangle _rectangle;
        private Texture2D _texture;
        private float _rotation;
        private float _rotationIncrement;
        private bool _enableRotation;
        private float _speed;
        private float _lifeTime;
        private long _elapsedTime;
        private bool _active;

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public YnParticle(Texture2D texture, Rectangle rectangle, Vector2 direction, float lifeTime)
        {
            _direction = direction;
            _rectangle = rectangle;
            _texture = texture;
            _origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
            _rotation = 0;
            _rotationIncrement = 0;
            _enableRotation = false;
            _speed = 8.5f;
            _lifeTime = lifeTime;
            _elapsedTime = 0;
            _active = false;
        }

        /// <summary>
        /// Sets the configuration to use for the particle.
        /// </summary>
        /// <param name="configuration">Particle configuration.</param>
        public void SetConfiguration(ParticleConfiguration configuration)
        {
            _speed = configuration.Speed;
            _enableRotation = configuration.EnabledRotation;
            _rotationIncrement = configuration.RotationIncrement;
            _rectangle.Width = configuration.Width;
            _rectangle.Height = configuration.Height;
            _lifeTime = configuration.LifeTime;
        }

        public void AddPosition(int x, int y)
        {
            _rectangle.X += x;
            _rectangle.Y += y;
        }

        public void AddPosition(Vector2 position)
        {
            AddPosition((int)position.X, (int)position.Y);
        }

        public void SetPosition(int x, int y)
        {
            _rectangle.X = x;
            _rectangle.Y = y;
        }

        public void SetPosition(Vector2 position)
        {
            SetPosition((int)position.X, (int)position.Y);
        }

        /// <summary>
        /// Revive the particle.
        /// </summary>
        public void Revive()
        {
            _active = true;
            _elapsedTime = 0;
        }

        /// <summary>
        /// Update particle position if active.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (_active)
            {
                _elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

                if (_elapsedTime >= _lifeTime)
                {
                    _active = false;
                }
                else
                {
                    _rectangle.X += (int)(_direction.X * _speed);
                    _rectangle.Y += (int)(_direction.Y * _speed);

                    if (_enableRotation)
                        _rotation += _rotationIncrement;
                }
            }
        }

        /// <summary>
        /// Draw particle on screen if active.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_active)
                spriteBatch.Draw(_texture, _rectangle, null, Color.White, _rotation, _origin, SpriteEffects.None, 1.0f);
        }
    }
}
