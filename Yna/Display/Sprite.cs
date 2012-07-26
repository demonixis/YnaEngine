using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Helpers;
using Yna.Display.Animation;
using Yna.Display.Event;

namespace Yna.Display
{
    public enum MovementState
    {
        JumpingUp, JumpingDown, Walking
    }

    public class Sprite : YnObject
    {
        // Physique appliquée au Sprite
		protected Vector2 _acceleration;
		protected Vector2 _velocity;
		protected float _maxVelocity;
        
        // Déplacements
        protected bool _canMove;
        protected bool _isFollowed;
        protected Vector2 _direction;
        protected Vector2 _lastPosition;
        protected Rectangle _viewport;

        // Gestion des collisions
        protected bool _forceInsideScreen;
        protected bool _forceInsideOutsideScreen;
        protected bool _forceBounce;

        // Position et rendu
        protected Rectangle? _sourceRectangle;
        protected SpriteEffects _effects;
        protected float _layerDepth;
		
        // Gestion des animations
        protected bool _hasAnimation;
        protected SpriteAnimator _animator;
        protected long _elapsedTime;

        // Gestion du saut
        protected MovementState _movementState;
        protected int _jumpHeight;              // Hauteur du saut
        protected Vector2 _initialJumpPosition; // Position initiale
        protected float _jumpSpeed;             // Vitesse du saut
        
        #region Propriétés
		
		public Vector2 Acceleration
		{
			get { return _acceleration; }
			set { _acceleration = value; }
		}
		
		public Vector2 Velocity
		{
			get { return _velocity; }
			set { _velocity = value; }
		}
		
		public float VelocityX
		{
			get { return _velocity.X; }
			set { _velocity = new Vector2(_velocity.X = value, _velocity.Y); }	
		}
		
		public float VelocityY
		{
			get { return _velocity.Y; }
			set { _velocity = new Vector2(_velocity.X, _velocity.Y = value); }	
		}
		
		public float VelocityMax
		{
			get { return _maxVelocity; }
			set { _maxVelocity = value; }
		}
		
        public bool CanMove
        {
            get { return _canMove; }
            set { _canMove = value; }
        }

        public Vector2 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public Vector2 LastDistance
        {
            get { return _position - _lastPosition; }
        }

        public Rectangle Viewport
        {
            get { return _viewport; }
            set { _viewport = value; }
        }

        /// <summary>
        /// Oblige le Sprite à ne pas quitter l'écran
        /// </summary>
        public bool ForceInsideScreen
        {
            get { return _forceInsideScreen; }
            set
            {
                _forceInsideScreen = value;

                if (_forceInsideScreen)
                {
                    _forceInsideOutsideScreen = false;
                    _forceBounce = false;
                }
            }
        }

        /// <summary>
        /// Oblige le sprite à ne pas quitter l'écran. Si il sort de l'écran, il est déplacé à sa position inverse
        /// </summary>
        public bool ForceInsideOutsideScreen
        {
            get { return _forceInsideOutsideScreen; }
            set
            {
                _forceInsideOutsideScreen = value;

                if (_forceInsideOutsideScreen)
                {
                    _forceInsideScreen = false;
                    _forceBounce = false;
                }
            }
        }

        /// <summary>
        /// Dernière position du Sprite
        /// </summary>
        public Vector2 LastPosition
        {
            get { return _lastPosition; }
            set { _lastPosition = value; }
        }

        public Rectangle? SourceRectangle
        {
            get { return _sourceRectangle;  }
            set { _sourceRectangle = value; }
        }

        public SpriteEffects Effects
        {
            get { return _effects; }
            set { _effects = value; }
        }

        public float LayerDepth
        {
            get { return _layerDepth; }
            set { _layerDepth = value; }
        }

        public bool HasAnimation
        {
            get { return _hasAnimation; }
        }

        public int JumpHeight
        {
            get { return _jumpHeight; }
            set { _jumpHeight = value; }
        }

        public float JumpSpeed
        {
            get { return _jumpSpeed; }
            set { _jumpSpeed = value; }
        }

        public MovementState MovementState
        {
            get { return _movementState; }
            set { _movementState = value; }
        }
        #endregion
        
        #region Evenements
        public event EventHandler<SpriteScreenCollideEventArgs> ScreenCollide = null; 
        
        private void SpriteColliding(SpriteScreenCollideEventArgs arg)
        {
        	if (ScreenCollide != null)
        		ScreenCollide(this, arg);
        }
        #endregion

        #region constructeurs
        public Sprite() 
            : base ()
        {
            _sourceRectangle = null;
            _lastPosition = Vector2.Zero;
            _rectangle = Rectangle.Empty;
            _color = Color.White;
            _alpha = 1.0f;
            _rotation = 0.0f;
            _origin = Vector2.Zero;
            _scale = Vector2.One;
            _effects = SpriteEffects.None;
            _layerDepth = 1.0f;
            _textureName = string.Empty;

            _viewport = new Rectangle(0, 0, YnG.Width, YnG.Height);
            _forceInsideScreen = false;
            _forceInsideOutsideScreen = false;

            _hasAnimation = false;
            _animator = null;
            _elapsedTime = 0;

            _textureLoaded = false;
			
            _acceleration = Vector2.One;
			_velocity = Vector2.Zero;
			_maxVelocity = 0.95f;

            _direction = Vector2.One;

            // Saut
            _movementState = MovementState.Walking;
            _jumpHeight = 90;
            _jumpSpeed = 6.5f;
            _initialJumpPosition = Vector2.Zero;
        }

        public Sprite(Vector2 position)
            : this()
        {
            _position = position;
        }

        public Sprite(Vector2 position, string assetName) 
            : this(position)
        {
            _textureName = assetName;
        }

        public Sprite(Rectangle rectangle, Color color)
            : this()
        {
            Rectangle = rectangle;
            _texture = GraphicsHelper.CreateTexture(color, rectangle.Width, rectangle.Height);
            _textureLoaded = true;
        }
        #endregion

        /// <summary>
        /// Indique que la texture utilisée est une feuille de sprite et que le Sprite
        /// pourra être animé
        /// </summary>
        /// <param name="width">Largeur du sprite sur la feuille de sprite</param>
        /// <param name="height">Hauteur du sprite sur la feuille de sprite</param>
        public void PrepareAnimation(int width, int height)
        {
            _hasAnimation = true;
            _animator = new SpriteAnimator(width, height, _texture.Width, _texture.Height);
            
            // Le sprite fait désormais la taille d'une animation
            Rectangle = new Rectangle(X, Y, width, height);
        }

        /// <summary>
        /// Ajoute une animation au Sprite
        /// </summary>
        /// <param name="name">Nom de l'animation</param>
        /// <param name="indexes">Tableau d'indices</param>
        /// <param name="frameRate">Temps entre chaque frame</param>
        /// <param name="reversed">Retourner l'animation horizontalement</param>
        public void AddAnimation(string name, int[] indexes, int frameRate, bool reversed)
        {
            _animator.Add(name, indexes, frameRate, reversed);
            _sourceRectangle = _animator.Animations[name].Rectangle[0];
        }

        /// <summary>
        /// Ajoute une animation au Sprite
        /// </summary>
        /// <param name="name">Nom de l'animation</param>
        /// <param name="rectangles">Tableau de rectangles encadrant les animations</param>
        /// <param name="frameRate">Temps entre chaque frame</param>
        /// <param name="reversed">Retourner l'animation horizontalement</param>
        public void AddAnimation(string name, Rectangle[] rectangles, int frameRate, bool reversed)
        {
            _animator.Add(name, rectangles, frameRate, reversed);
            _sourceRectangle = _animator.Animations[name].Rectangle[0];
        }

        /// <summary>
        /// Joue une animation.
        /// </summary>
        /// <param name="animationName">Nom de l'animation</param>
        public void Play(string animationName)
        {
            _sourceRectangle = _animator.Animations[animationName].Next(ref _effects, _elapsedTime);
        }

        public void Jump()
        {
            if (_movementState == MovementState.Walking)
            {
                _movementState = MovementState.JumpingUp;
                _initialJumpPosition = new Vector2(X, Y);
            }
        }
        
        public override void Initialize() { }

        public override void LoadContent()
        {
            if (!_textureLoaded)
            {
                if (_texture == null)
                {
                    if (_textureName != string.Empty)
                        _texture = YnG.Content.Load<Texture2D>(_textureName);
                    else
                        throw new Exception("[Sprite] Impossible de charger la texture");
                }

                SourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
                Rectangle = new Rectangle((int)X, (int)Y, _texture.Width, _texture.Height);

                _textureLoaded = true;
            }
        }

        public virtual void LoadContent(string textureName)
        {
            _textureName = textureName;
            _textureLoaded = false;

            if (_texture != null)
                _texture.Dispose();
            
            _texture = null;
            
            LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Pause)
            {
                // Sauvegarde de la dernière position
                LastPosition = Position;

                _elapsedTime = gameTime.ElapsedGameTime.Milliseconds;

                // Physique
                _position += _velocity * _acceleration;
                _velocity *= _maxVelocity;

                // Saut
                if (_movementState != MovementState.Walking)
                {
                    // Le Sprite est en train de sautter
                    if (_movementState == MovementState.JumpingUp)
                    {
                        _position = new Vector2(X, _position.Y - _jumpSpeed);

                        if (Y < (_initialJumpPosition.Y - _jumpHeight))
                            _movementState = MovementState.JumpingDown;
                    }

                    // Le Sprite à terminé de sauter et il redescent
                    if (_movementState == MovementState.JumpingDown)
                        _position = new Vector2(X, _position.Y + _jumpSpeed);

                    // Le saut est terminé on replace la position Y comme au départ
                    if (Y >= _initialJumpPosition.Y)
                    {
                        //_position = new Vector2(X, _initialJumpPosition.Y);
                        _movementState = MovementState.Walking;
                    }
                }

                if (_hasAnimation)
                    _animator.Update(gameTime, LastDistance);
            }
        }

        /// <summary>
        /// Cette méthode est appelée juste après Update() et juste avant Draw()
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void PostUpdate(GameTime gameTime)
        {
            // On garde les mêmes positions à l'écran
            if (!_scrollFactor.X)
                X += YnG.Camera.X;

            if (!_scrollFactor.Y)
                Y += YnG.Camera.Y;

            // Gestion de la caméra si le Sprite est suivie
            if (_isFollowed)
            {
                YnG.Camera.X += X;
                YnG.Camera.Y += Y;
            }

            if (_forceInsideScreen)
            {
                if (X < _viewport.X) 
                    Position = new Vector2(_viewport.X, Y);
                else if (X + Width > _viewport.Width)
                    Position = new Vector2(_viewport.Width - Width, Y);

                if (Y < _viewport.Y)
                    Position = new Vector2(X, _viewport.Y);
                else if (Y + Height > _viewport.Height)
                    Position = new Vector2(X, _viewport.Height - Height);
            }
            else if (_forceInsideOutsideScreen)
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
            if (!Pause)
                PostUpdate(gameTime);
            
            if (Visible)
                spriteBatch.Draw(_texture, Rectangle, SourceRectangle, Color, Rotation, Origin, _effects, LayerDepth);
        }

        public override void UnloadContent()
        {
            if (_texture != null && Dirty)
                _texture.Dispose();
        }
    }
}
