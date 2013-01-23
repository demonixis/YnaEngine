﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Framework.Helpers;
using Yna.Framework.Input;
using Yna.Framework.Display.Animation;
using Yna.Framework.Display.Event;

namespace Yna.Framework.Display
{
    public class YnSprite : YnEntity
    {
        #region Private declarations

        // Some physics
		protected Vector2 _acceleration;
		protected Vector2 _velocity;
		protected float _maxVelocity;
        
        // Moving the sprite
        protected Vector2 _direction;
        protected Vector2 _lastPosition;
        protected Rectangle _viewport;

        // Collide with screen
        protected bool _InsideScreen;
        protected bool _acrossScreen;

        // Position
        protected Rectangle? _sourceRectangle;
        
		
        // Animations
        protected bool _hasAnimation;
        protected SpriteAnimator _animator;
        protected long _elapsedTime;

        #endregion

        #region Properties

        /// <summary>
        /// Get or Set Acceleration
        /// </summary>
        public Vector2 Acceleration
		{
			get { return _acceleration; }
			set { _acceleration = value; }
		}
		
        /// <summary>
        /// Get or Set Velocity
        /// </summary>
		public Vector2 Velocity
		{
			get { return _velocity; }
			set { _velocity = value; }
		}
		
        /// <summary>
        /// Get or Set the X value of the Velocity
        /// </summary>
		public float VelocityX
		{
			get { return _velocity.X; }
			set { _velocity.X = value; }	
		}
		
        /// <summary>
        /// Get or Set set Y value of the Velocity
        /// </summary>
		public float VelocityY
		{
			get { return _velocity.Y; }
			set { _velocity.Y = value; }	
		}
		
        /// <summary>
        /// Get or Set the VelocityMax
        /// </summary>
		public float VelocityMax
		{
			get { return _maxVelocity; }
			set { _maxVelocity = value; }
		}

        /// <summary>
        /// Get or Set the direction of the sprite
        /// </summary>
        public Vector2 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        /// <summary>
        /// Get the last distance traveled by the sprite
        /// </summary>
        public Vector2 LastDistance
        {
            get { return _position - _lastPosition; }
        }

        /// <summary>
        /// Get or Set the rectangle Viewport used for this sprite. Default is the size of the screen
        /// </summary>
        public Rectangle Viewport
        {
            get { return _viewport; }
            set { _viewport = value; }
        }

        /// <summary>
        /// Force or not the sprite to stay in screen
        /// </summary>
        public bool InsideScreen
        {
            get { return _InsideScreen; }
            set
            {
                _InsideScreen = value;

                if (_InsideScreen)
                    _acrossScreen = false;
            }
        }

        /// <summary>
        /// Authorizes or not the object across the screen and appear on the opposite
        /// </summary>
        public bool AcrossScreen
        {
            get { return _acrossScreen; }
            set
            {
                _acrossScreen = value;

                if (_acrossScreen)
                    _InsideScreen = false;
            }
        }

        /// <summary>
        /// Get or Set the last position of the sprite
        /// </summary>
        public Vector2 LastPosition
        {
            get { return _lastPosition; }
            set { _lastPosition = value; }
        }

        /// <summary>
        /// Get or Set the Source rectangle
        /// </summary>
        public Rectangle? SourceRectangle
        {
            get { return _sourceRectangle;  }
            set { _sourceRectangle = value; }
        }

        /// <summary>
        /// Get the animation status
        /// </summary>
        public bool HasAnimation
        {
            get { return _hasAnimation; }
        }
        #endregion

        #region constructors

        /// <summary>
        /// Create a sprite with default values
        /// </summary>
        public YnSprite() 
            : base ()
        {
            _sourceRectangle = null;
            _lastPosition = Vector2.Zero;

            _viewport = new Rectangle(0, 0, YnG.Width, YnG.Height);
            _InsideScreen = false;
            _acrossScreen = false;

            _hasAnimation = false;
            _animator = new SpriteAnimator();
            _elapsedTime = 0;

            _acceleration = Vector2.One;
			_velocity = Vector2.Zero;
            _maxVelocity = 1.0f;

            _direction = Vector2.One;
        }

        private YnSprite(Vector2 position)
            : this()
        {
            _position = position;
        }

        /// <summary>
        /// Create a sprite
        /// </summary>
        /// <param name="assetName">Image name that will loaded from the content manager</param>
        public YnSprite(string assetName)
            : this(Vector2.Zero)
        {
            _assetName = assetName;
        }

        /// <summary>
        /// Create a sprite
        /// </summary>
        /// <param name="position">Position of the sprite</param>
        /// <param name="assetName">Image name that will loaded from the content manager</param>
        public YnSprite(Vector2 position, string assetName) 
            : this(position)
        {
            _assetName = assetName;
        }

        /// <summary>
        /// Create a sprite without asset
        /// </summary>
        /// <param name="rectangle">Size of the sprite</param>
        /// <param name="color">Color of the sprite</param>
        public YnSprite(Rectangle rectangle, Color color)
            : this()
        {
            Rectangle = rectangle;
            _texture = YnGraphics.CreateTexture(color, rectangle.Width, rectangle.Height);
            _assetLoaded = true;
            _position = new Vector2(rectangle.X, rectangle.Y);
            _rectangle = rectangle;
        }

        #endregion

        #region Animation methods

        /// <summary>
        /// Prepare the sprite for animation.
        /// </summary>
        /// <param name="width">width of a sprite on the spritesheet</param>
        /// <param name="height">height of a sprite on the spritesheet</param>
        public void PrepareAnimation(int width, int height)
        {
            _animator.Initialize(width, height, _texture.Width, _texture.Height);
            
            // The sprite size is now the size of a sprite on the spritesheet
            Rectangle = new Rectangle(X, Y, width, height);

            _hasAnimation = true;
        }

        /// <summary>
        /// Add an animation
        /// </summary>
        /// <param name="name">Animation name</param>
        /// <param name="indexes">Array of index that represent images</param>
        /// <param name="frameRate">Framerate for this animation</param>
        /// <param name="reversed">Reverse or not the animation</param>
        public void AddAnimation(string name, int[] indexes, int frameRate, bool reversed)
        {
            _animator.Add(name, indexes, frameRate, reversed);
            _sourceRectangle = _animator.Animations[name].Rectangle[0];
        }

        /// <summary>
        /// Add an animation
        /// </summary>
        /// <param name="name">Animation name</param>
        /// <param name="rectangles">Array of Rectangles that represents animations on the spritesheet</param>
        /// <param name="frameRate">Framerate for this animation</param>
        /// <param name="reversed">Reverse or not the animation</param>
        public void AddAnimation(string name, Rectangle[] rectangles, int frameRate, bool reversed)
        {
            _animator.Add(name, rectangles, frameRate, reversed);
            _sourceRectangle = _animator.Animations[name].Rectangle[0];
        }

        /// <summary>
        /// Get the current animation index
        /// </summary>
        /// <param name="animationKeyName">Animation name</param>
        /// <returns>Index of the animation</returns>
        public int GetCurrentAnimationIndex(string animationKeyName)
        {
            if (_hasAnimation)           
                return _animator.Animations[animationKeyName].Index;
            else
                return 0;
        }

        /// <summary>
        /// Get the length of an animation
        /// </summary>
        /// <param name="animationKeyName">Animation name</param>
        /// <returns>Length of the animation</returns>
        public int GetAnimationLenght(string animationKeyName)
        {
            if (_hasAnimation)
                return _animator.Animations[animationKeyName].Length;
            else
                return 0;
        }

        /// <summary>
        /// Get the SpriteAnimation object for a specified animation
        /// </summary>
        /// <param name="animationKeyName">Animation name</param>
        /// <returns>The SpriteAnimation object</returns>
        public SpriteAnimation GetAnimation(string animationKeyName)
        {
            if (_hasAnimation)
                return _animator.Animations[animationKeyName];
            else
                return null;
        }

        /// <summary>
        /// Play an animation
        /// </summary>
        /// <param name="animationName">Animation name</param>
        public void Play(string animationName)
        {
            _sourceRectangle = _animator.Animations[animationName].Next(ref _effects, _elapsedTime);
        }

        #endregion

        #region GameState patterns

        public override void Initialize() { }

        public override void LoadContent()
        {
            if (!_assetLoaded)
            {
                if (_texture == null)
                {
                    if (_assetName != string.Empty)
                        _texture = YnG.Content.Load<Texture2D>(_assetName);
                    else
                        throw new Exception("[Sprite] Impossible de charger la texture");
                }

                SourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
                Rectangle = new Rectangle((int)X, (int)Y, _texture.Width, _texture.Height);

                _assetLoaded = true;
            }
        }

        /// <summary>
        /// Load content with a specific texture
        /// if a texture is already loaded, it's replaced by the new
        /// </summary>
        /// <param name="textureName">Texture name</param>
        public virtual void LoadContent(string textureName)
        {
            _assetName = textureName;
            _assetLoaded = false;
            
            _texture = null;
            
            LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Enabled)
            {
                // Sauvegarde de la dernière position
                LastPosition = Position;

                _elapsedTime = gameTime.ElapsedGameTime.Milliseconds;

                // Physics
                _position += _velocity * _acceleration;
                _velocity *= _maxVelocity;

                if (_hasAnimation)
                    _animator.Update(gameTime, LastDistance);
            }
        }

        /// <summary>
        /// Post updates for sprite collide and direction
        /// Called after Update() and before Draw() 
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void PostUpdate(GameTime gameTime)
        {
            // Update the direction
            Direction = new Vector2(LastDistance.X, LastDistance.Y);
            Direction.Normalize();

            if (_InsideScreen)
            {
                if (X < _viewport.X)
                {
                    Position = new Vector2(_viewport.X, Y);
                    _velocity *= 0.0f;
                }
                else if (X + Width > _viewport.Width)
                {
                    Position = new Vector2(_viewport.Width - Width, Y);
                    _velocity *= 0.0f;
                }

                if (Y < _viewport.Y)
                {
                    Position = new Vector2(X, _viewport.Y);
                    _velocity *= 0.0f;
                }
                else if (Y + Height > _viewport.Height)
                {
                    Position = new Vector2(X, _viewport.Height - Height);
                    _velocity *= 0.0f;
                }
            }
            else if (_acrossScreen)
            {
                if (X + Width < _viewport.X)
                    Position = new Vector2(_viewport.Width, Y);
                else if (X > _viewport.Width)
                    Position = new Vector2(_viewport.X, Y);

                if (Y + Height < _viewport.Y)
                    Position = new Vector2(X, _viewport.Height);
                else if (Y > _viewport.Height)
                    Position = new Vector2(X, _viewport.Y);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Enabled)
                PostUpdate(gameTime);
           
            if (Visible)
                spriteBatch.Draw(_texture, Position, SourceRectangle, Color * Alpha, Rotation, Origin, Scale, _effects, LayerDepth);
        }

        #endregion
    }
}
