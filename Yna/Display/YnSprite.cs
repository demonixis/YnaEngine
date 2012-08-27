using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Helpers;
using Yna.Input;
using Yna.Display.Animation;
using Yna.Display.Event;

namespace Yna.Display
{
    public class YnSprite : YnObject
    {
        #region Private declarations

        // Some physics
		protected Vector2 _acceleration;
		protected Vector2 _velocity;
		protected float _maxVelocity;
        
        // Moving the sprite
        protected bool _canMove;
        protected bool _isFollowed;
        protected Vector2 _direction;
        protected Vector2 _lastPosition;
        protected Rectangle _viewport;

        // Collide with screen
        protected bool _InsideScreen;
        protected bool _acrossScreen;

        // Position
        protected Rectangle? _sourceRectangle;
        protected SpriteEffects _effects;
        protected float _layerDepth;
		
        // Animations
        protected bool _hasAnimation;
        protected SpriteAnimator _animator;
        protected long _elapsedTime;

        #endregion

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
        /// Force or not the sprite to 
        /// </summary>
        public bool InsideScreen
        {
            get { return _InsideScreen; }
            set
            {
                _InsideScreen = value;

                if (_InsideScreen)
                {
                    _acrossScreen = false;
                }
            }
        }

        /// <summary>
        /// Oblige le sprite à ne pas quitter l'écran. Si il sort de l'écran, il est déplacé à sa position inverse
        /// </summary>
        public bool AcrossScreen
        {
            get { return _acrossScreen; }
            set
            {
                _acrossScreen = value;

                if (_acrossScreen)
                {
                    _InsideScreen = false;
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
        #endregion
        
        #region Evenements

        public event EventHandler<ScreenCollideSpriteEventArgs> ScreenCollide = null;
        
        private void CollideScreenSprite(ScreenCollideSpriteEventArgs arg)
        {
        	if (ScreenCollide != null)
        		ScreenCollide(this, arg);
        }
        
        #endregion

        #region constructeurs
        public YnSprite() 
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
            _InsideScreen = false;
            _acrossScreen = false;

            _hasAnimation = false;
            _animator = null;
            _elapsedTime = 0;

            _textureLoaded = false;
			
            _acceleration = Vector2.One;
			_velocity = Vector2.Zero;
            _maxVelocity = 1.0f;

            _direction = Vector2.One;
        }

        public YnSprite(string assetName)
            : this(Vector2.Zero)
        {
            _textureName = assetName;
        }

        public YnSprite(Vector2 position)
            : this()
        {
            _position = position;
        }

        public YnSprite(Vector2 position, string assetName) 
            : this(position)
        {
            _textureName = assetName;
        }

        /// <summary>
        /// DEPREACTED : Use YnSprite(YnRectangle, Color) instead
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="color"></param>
        public YnSprite(Rectangle rectangle, Color color)
            : this()
        {
            Rectangle = rectangle;
            _texture = GraphicsHelper.CreateTexture(color, rectangle.Width, rectangle.Height);
            _textureLoaded = true;
            _position = new Vector2(rectangle.X, rectangle.Y);
            _rectangle = rectangle;
        }

        public YnSprite(YnRectangle rectangle, Color color)
        {
            Rectangle = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            _texture = GraphicsHelper.CreateTexture(color, rectangle.Width, rectangle.Height);
            _textureLoaded = true;
        }

        public YnSprite(int x, int y, string assetName)
            : this(new Vector2(x, y), assetName) { }

        #endregion

        #region Animation methods

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

        public int GetCurrentAnimationIndex(string animationKeyName)
        {
            if (_hasAnimation)
            {
                return _animator.Animations[animationKeyName].Index;
            }
            else
                return 0;
        }

        public int GetAnimationLenght(string animationKeyName)
        {
            if (_hasAnimation)
            {
                return _animator.Animations[animationKeyName].Length;
            }
            else
                return 0;
        }

        public SpriteAnimation GetAnimation(string animationKeyName)
        {
            if (_hasAnimation)
            {
                return _animator.Animations[animationKeyName];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Joue une animation.
        /// </summary>
        /// <param name="animationName">Nom de l'animation</param>
        public void Play(string animationName)
        {
            _sourceRectangle = _animator.Animations[animationName].Next(ref _effects, _elapsedTime);
        }

        #endregion

        /// <summary>
        /// Set Rectangle & SourceRectangle at the same value
        /// </summary>
        /// <param name="rectangle">Rectangle</param>
        public void SetRectangles(Rectangle rectangle)
        {
            _sourceRectangle = rectangle;
            _rectangle = rectangle;
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
            
            _texture = null;
            
            LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!Pause)
            {
                // Sauvegarde de la dernière position
                LastPosition = Position;

                _elapsedTime = gameTime.ElapsedGameTime.Milliseconds;

                // Physique
                _position += _velocity * _acceleration;
                _velocity *= _maxVelocity;

                #region Events handlers

                // Screen
                if (X < Viewport.X)
                    CollideScreenSprite(new ScreenCollideSpriteEventArgs(SpriteScreenCollide.Left));
                else if (X + Width > Viewport.Width)
                    CollideScreenSprite(new ScreenCollideSpriteEventArgs(SpriteScreenCollide.Right));

                if (Y < Viewport.Y)
                    CollideScreenSprite(new ScreenCollideSpriteEventArgs(SpriteScreenCollide.Top));
                else if (Y + Height > Viewport.Height)
                    CollideScreenSprite(new ScreenCollideSpriteEventArgs(SpriteScreenCollide.Bottom));

                #endregion

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
            if (!Pause)
                PostUpdate(gameTime);
           
            if (Visible)
                spriteBatch.Draw(_texture, Position, SourceRectangle, Color * Alpha, Rotation, Origin, Scale, _effects, LayerDepth);
        }

        public override void UnloadContent()
        {
            if (_texture != null && Dirty)
                _texture.Dispose();
        }

        /// <summary>
        /// Naïve test collision with rectangle
        /// </summary>
        /// <param name="spriteA"></param>
        /// <param name="spriteB"></param>
        /// <returns></returns>
        public static bool RectCollide(YnSprite spriteA, YnSprite spriteB)
        {
            return spriteA.Rectangle.Intersects(spriteB.Rectangle);
        }

        /// <summary>
        /// Perfect pixel test collision
        /// </summary>
        /// <param name="spriteA"></param>
        /// <param name="spriteB"></param>
        /// <returns></returns>
        public static bool PerfectPixelCollide(YnSprite spriteA, YnSprite spriteB)
        {
            int top = Math.Max(spriteA.Rectangle.Top, spriteB.Rectangle.Top);
            int bottom = Math.Min(spriteA.Rectangle.Bottom, spriteB.Rectangle.Bottom);
            int left = Math.Max(spriteA.Rectangle.Left, spriteB.Rectangle.Left);
            int right = Math.Min(spriteA.Rectangle.Right, spriteB.Rectangle.Right);

            for (int y = top; y < bottom; y++)  // De haut en bas
            {
                for (int x = left; x < right; x++)  // de gauche à droite
                {
                    int index_A = (x - spriteA.Rectangle.Left) + (y - spriteA.Rectangle.Top) * spriteA.Rectangle.Width;
                    int index_B = (x - spriteB.Rectangle.Left) + (y - spriteB.Rectangle.Top) * spriteB.Rectangle.Width;

                    Color[] colorsSpriteA = GraphicsHelper.GetTextureData(spriteA);
                    Color[] colorsSpriteB = GraphicsHelper.GetTextureData(spriteB);

                    Color colorSpriteA = colorsSpriteA[index_A];
                    Color colorSpriteB = colorsSpriteB[index_B];

                    if (colorSpriteA.A != 0 && colorSpriteB.A != 0)
                        return true;
                }
            }
            return false;
        }
    }
}
