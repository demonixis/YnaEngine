// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics.Animation;

namespace Yna.Engine.Graphics
{
    public class YnSprite : YnEntity
    {
        #region Private declarations

        // Some physics
        protected bool _enableDefaultPhysics;
		protected Vector2 _acceleration;
		protected Vector2 _velocity;
		protected float _maxVelocity;
        
        // Moving the sprite
        protected Vector2 _distance;
        protected Vector2 _direction;
        protected Vector2 _previousPosition;
        protected Vector2 _previousDistance;
        
        // Collide with screen
        protected bool _forceInsideScreen;
        protected bool _forceAllowAcrossScreen;

        // Position
        protected Rectangle? _sourceRectangle;
        protected Rectangle _gameViewport;
		
        // Animations
        protected bool _hasAnimation;
        protected SpriteAnimator _animator;

        #endregion

        #region Properties

        /// <summary>
        /// Enable or disable the default physics system
        /// </summary>
        public bool EnablePhysics
        {
            get { return _enableDefaultPhysics; }
            set { _enableDefaultPhysics = value; }
        }

        /// <summary>
        /// Gets or sets Acceleration
        /// </summary>
        public Vector2 Acceleration
		{
			get { return _acceleration; }
			set { _acceleration = value; }
		}
		
        /// <summary>
        /// Gets or sets Velocity
        /// </summary>
		public Vector2 Velocity
		{
			get { return _velocity; }
			set { _velocity = value; }
		}
		
        /// <summary>
        /// Gets or sets the X value of the Velocity
        /// </summary>
		public float VelocityX
		{
			get { return _velocity.X; }
			set { _velocity.X = value; }	
		}
		
        /// <summary>
        /// Gets or sets set Y value of the Velocity
        /// </summary>
		public float VelocityY
		{
			get { return _velocity.Y; }
			set { _velocity.Y = value; }	
		}
		
        /// <summary>
        /// Gets or sets the VelocityMax
        /// </summary>
		public float VelocityMax
		{
			get { return _maxVelocity; }
			set { _maxVelocity = value; }
		}

        /// <summary>
        /// Gets or sets the rectangle Viewport used for this sprite. Default is the size of the screen
        /// </summary>
        public Rectangle Viewport
        {
            get { return _gameViewport; }
            set { _gameViewport = value; }
        }

        /// <summary>
        /// Force or not the sprite to stay in screen
        /// </summary>
        public bool ForceInsideScreen
        {
            get { return _forceInsideScreen; }
            set
            {
                _forceInsideScreen = value;

                if (_forceInsideScreen)
                    _forceAllowAcrossScreen = false;
            }
        }

        /// <summary>
        /// Authorizes or not the object across the screen and appear on the opposite
        /// </summary>
        public bool AllowAcrossScreen
        {
            get { return _forceAllowAcrossScreen; }
            set
            {
                _forceAllowAcrossScreen = value;

                if (_forceAllowAcrossScreen)
                    _forceInsideScreen = false;
            }
        }

        /// <summary>
        /// Gets the direction.
        /// </summary>
        public Vector2 Direction
        {
            get { return _direction; }
        }

        /// <summary>
        /// Gets the previous position.
        /// </summary>
        public Vector2 LastPosition
        {
            get { return _previousPosition; }
        }

        /// <summary>
        /// Gets or sets the distance of the sprite
        /// </summary>
        public Vector2 Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }

        /// <summary>
        /// Gets the previous direction.
        /// </summary>
        public Vector2 PreviousDistance
        {
            get { return _previousDistance; }
        }

        /// <summary>
        /// Gets or sets the Source rectangle
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
            _gameViewport = new Rectangle(0, 0, YnG.Width, YnG.Height);
            _forceInsideScreen = false;
            _forceAllowAcrossScreen = false;
            
            _hasAnimation = false;
            _animator = new SpriteAnimator();
            
            _acceleration = Vector2.One;
			_velocity = Vector2.Zero;
            _maxVelocity = 1.0f;
            _enableDefaultPhysics = true;
            
            _distance = Vector2.One;
            _direction = Vector2.Zero;
            _previousPosition = Vector2.Zero;
            _previousDistance = Vector2.Zero;
        }

        private YnSprite(Vector2 position)
            : this()
        {
            _position = position;
            _previousPosition = _position;
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
            _rectangle = new Rectangle((int)X, (int)Y, width, height);

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
        /// <param name="startIndex">The start sprite index (included)</param>
        /// <param name="endIndex">The end sprite index (included)</param>
        /// <param name="frameRate">Framerate for this animation</param>
        /// <param name="reversed">Reverse or not the animation</param>
        public void AddAnimation(string name, int startIndex, int endIndex, int frameRate, bool reversed)
        {
        	// Securize the start and end index
        	if(startIndex > endIndex)
        	{
        		int temp = endIndex;
        		endIndex = startIndex;
        		startIndex = temp;
        	}
        	
        	// Build the index array
        	int count = endIndex - startIndex;
        	int[] indexes = new int[count+1];
        	int currentIntex = startIndex;

        	for(int i = 0; i <= count; i++)
        	{
        		indexes[i] = currentIntex;
        		currentIntex++;
        	}
        	
        	// Call the proper method
        	AddAnimation(name, indexes, frameRate, reversed);
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
                return _animator.Animations[animationKeyName].Count;
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
            _sourceRectangle = _animator.Play(animationName, ref _effects);
        }

        #endregion

        #region GameState patterns

        /// <summary>
        /// Load the texture of the sprite.
        /// </summary>
        public override void LoadContent()
        {
            if (!_assetLoaded)
            {
                if (_texture == null && _assetName != String.Empty)
                {
                    _texture = YnG.Content.Load<Texture2D>(_assetName);

                    // if the sprite has animations destination and source rectangle are already setted correctly
                    if (!_hasAnimation)
                    {
                        _sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
                        _rectangle = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
                    }
                }
               
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
                _previousPosition.X = _position.X;
                _previousPosition.Y = _position.Y;
                _previousDistance.X = _distance.X;
                _previousDistance.Y = _distance.Y;
                
                // Physics
                if (_enableDefaultPhysics)
                {
                    _position += _velocity * _acceleration;
                    _velocity *= _maxVelocity;
                }

                if (_hasAnimation)
                {
                    _animator.Update(gameTime);

                    if (_previousDistance == Vector2.Zero && _animator.CurrentAnimationName != String.Empty)
                        _sourceRectangle = _animator.GetCurrentAnimation().Rectangle[0];
                }
            }
        }

        /// <summary>
        /// Post updates for sprite collide and direction
        /// Called after Update() and before Draw() 
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void PostUpdate(GameTime gameTime)
        {
            if (_forceInsideScreen)
            {
                if (X - _origin.X < _gameViewport.X)
                {
                    _position.X = _gameViewport.X + _origin.X;
                    _velocity *= 0.0f;
                }
                else if (X + (Width - Origin.X) > _gameViewport.Width)
                {
                    _position.X = _gameViewport.Width - (Width - Origin.X);
                    _velocity *= 0.0f;
                }

                if (Y - _origin.Y < _gameViewport.Y)
                {
                    _position.Y = _gameViewport.Y + _origin.Y;
                    _velocity *= 0.0f;
                }
                else if (Y + (Height - Origin.Y) > _gameViewport.Height)
                {
                    _position.Y = _gameViewport.Height - (Height - _origin.Y);
                    _velocity *= 0.0f;
                }
            }
            else if (_forceAllowAcrossScreen)
            {
                if (X + (Width - Origin.X) < _gameViewport.X)
                    _position.X = _gameViewport.Width - Origin.X;
                else if (X > _gameViewport.Width)
                    _position.X = _gameViewport.X;

                if (Y + Height < _gameViewport.Y)
                    _position.Y = _gameViewport.Height;
                else if (Y > _gameViewport.Height)
                    _position.Y = _gameViewport.Y;
            }

            // Update the direction
            _distance.X = _position.X - _previousPosition.X;
            _distance.Y = _position.Y - _previousPosition.Y;
            _direction.X = _distance.X;
            _direction.Y = _distance.Y;

            _rectangle.X = (int)_position.X;
            _rectangle.Y = (int)_position.Y;
            
            if (_direction.X != 0 && _direction.Y != 0)
                _direction.Normalize();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Enabled)
                PostUpdate(gameTime);
           
            if (Visible)
                spriteBatch.Draw(_texture, _position, _sourceRectangle, _color * _alpha, _rotation, _origin, _scale, _effects, _layerDepth);
        }

        #endregion
    }
}
