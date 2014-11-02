// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics.Particle
{
    /// <summary>
    /// An particle emitter.
    /// </summary>
    public class YnEmitter : YnEntity
    {
        private Vector2 _direction;
        private int _maxParticles;
        private int _intensity;
        private List<YnParticle> _particles;
        private long _elapsedTime;
        private int _nbParticlePerEmission;
        private int _activeParticleIndex;
        private bool _repeat;
        private bool _canRestart;
        private ParticleConfiguration _particleConfiguration;

        /// <summary>
        /// Gets or sets the direction of the emitter.
        /// </summary>
        public Vector2 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        /// <summary>
        /// Gets or sets the maximum particles can be emitted.
        /// </summary>
        public int MaxParticles
        {
            get { return _maxParticles; }
            set
            {
                Stop();
                _maxParticles = value;
                Initialize();
            }
        }

        /// <summary>
        /// Gets or sets the intensity of emission.
        /// </summary>
        public int Intensity
        {
            get { return _intensity; }
            set { _intensity = value; }
        }

        /// <summary>
        /// Gets or sets the number of particles emitted in one emission.
        /// </summary>
        public int NumberOfParticlePerEmission
        {
            get { return _nbParticlePerEmission; }
            set { _nbParticlePerEmission = value; }
        }

        /// <summary>
        /// Enable or disable the loop
        /// </summary>
        public bool Repeat
        {
            get { return _repeat; }
            set { _repeat = value; }
        }

        /// <summary>
        /// Gets or sets the particle texture.
        /// </summary>
        public new Texture2D Texture
        {
            get { return _texture; }
            set
            {
                Active = false;
                _texture = value;
                Active = true;
            }
        }

        public YnEmitter(Vector2 position, Vector2 direction, float angle, int maxParticles)
        {
            _maxParticles = maxParticles;
            _rotation = angle;
            _direction = direction;
            _position = position;
            _particles = new List<YnParticle>(_maxParticles);
            _elapsedTime = 0;
            _intensity = 100;
            _activeParticleIndex = 0;
            _nbParticlePerEmission = 3;
            _repeat = true;
            _canRestart = false;
            Active = false;

            _particleConfiguration = new ParticleConfiguration()
            {
                EnabledRotation = false,
                Height = 4,
                Width = 4,
                LifeTime = 400,
                Speed = 8.5f,
                RotationIncrement = 0
            };
        }

        public YnEmitter(Vector2 position, Vector2 direction)
            : this(position, direction, MathHelper.PiOver4, 50)
        {

        }

        /// <summary>
        /// Sets the configuration used for particles.
        /// </summary>
        /// <param name="configuration">Particle configuration.</param>
        /// <returns>True if the particles are already initialized otherwise return false.</returns>
        public bool SetParticleConfiguration(ParticleConfiguration configuration)
        {
            _particleConfiguration = configuration;

            if (_particles.Count > 0)
            {
                foreach (YnParticle particle in _particles)
                    particle.SetConfiguration(_particleConfiguration);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Initialize the emitter with a texture for particles.
        /// </summary>
        /// <param name="particleTexture">Texture used for particles.</param>
        public void Initialize(Texture2D particleTexture)
        {
            _texture = particleTexture;
            _particleConfiguration.Width = _texture.Width;
            _particleConfiguration.Height = _texture.Height;

            YnParticle particle = null;
            Vector2 direction = Vector2.Zero;
            Rectangle rectangle = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);

            for (int i = 0; i < _maxParticles; i++)
            {
                direction = YnRandom.GetVector2(
                    _direction.X - _rotation / 2,
                    _direction.X + _rotation / 2,
                    _direction.Y - _rotation / 2,
                    _direction.Y + _rotation / 2);

                particle = new YnParticle(_texture, rectangle, direction, 500);
                _particles.Add(particle);
            }
        }

        /// <summary>
        /// Initialize the emitter and setup the particles.
        /// </summary>
        /// <param name="particleColor">Particle color</param>
        /// <param name="particleWidth">Particle width</param>
        /// <param name="particleHeight">Particle height</param>
        public void Initialize(Color particleColor, int particleWidth, int particleHeight)
        {
            Initialize(YnGraphics.CreateTexture(particleColor, particleWidth, particleHeight));
        }

        /// <summary>
        /// Initialize the emitter with a default particle collection. 
        /// Created particles are a size of 4x4 and a random color.
        /// </summary>
        public override void Initialize()
        {
            Color color = YnRandom.GetColor();
            Initialize(YnGraphics.CreateTexture(color, 4, 4));
        }

        /// <summary>
        /// Start emitter.
        /// </summary>
        public void Start()
        {
            Active = false;
            _elapsedTime = 0;
            _canRestart = false;

            foreach (YnParticle particle in _particles)
            {
                particle.Revive();
                particle.SetPosition(_position);
                particle.Active = false;
            }
            _activeParticleIndex = 0;
            _particles[_activeParticleIndex].Active = true;
            Active = true;
        }

        /// <summary>
        /// Stop emission.
        /// </summary>
        public void Stop()
        {
            Active = false;
        }

        /// <summary>
        /// Sets the emitter position.
        /// </summary>
        /// <param name="x">X coordinate on screen.</param>
        /// <param name="y">Y coordinate on screen.</param>
        public override void Move(float x, float y)
        {
            int rx = (int)(x - _position.X);
            int ry = (int)(y - _position.Y);

            _position.X = x;
            _position.Y = y;

            foreach (YnParticle particle in _particles)
                particle.AddPosition(rx, ry);
        }

        private int GetNextParticleIndex()
        {
            if (_activeParticleIndex >= _maxParticles)
            {
                _canRestart = true;
                return 0;
            }
            return _activeParticleIndex++;
        }

        public override void Update(GameTime gameTime)
        {
            if (_enabled)
            {
                _elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

                if (_elapsedTime >= _intensity)
                {
                    _elapsedTime = 0;
                    if (_nbParticlePerEmission > 1)
                    {
                        for (int i = 0; i < _nbParticlePerEmission; i++)
                            _particles[GetNextParticleIndex()].Active = true;
                    }
                    else
                    {
                        _particles[GetNextParticleIndex()].Active = true;
                    }
                }

                if (_repeat && _canRestart)
                {
                    Start();
                }

                foreach (YnParticle particle in _particles)
                    particle.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_visible)
            {
                foreach (YnParticle particle in _particles)
                    particle.Draw(gameTime, spriteBatch);
            }
        }
    }
}
