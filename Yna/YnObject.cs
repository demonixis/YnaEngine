using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display;
using Yna.Helpers;

namespace Yna
{
    public enum ObjectOrigin
    {
    	TopLeft = 0, Top, TopRight, 
        Left, Center, Right,
        BottomLeft, Bottom, BottomRight
    }
    
    public abstract class YnObject : YnBase
    {
        protected bool _visible;
        protected ScrollFactor _scrollFactor;

        protected Vector2 _position;
        protected Rectangle _rectangle;
        protected Texture2D _texture;
        protected string _textureName;
        protected Color _color;
        protected float _rotation;
        protected Vector2 _origin;
        protected Vector2 _scale;
        protected float _alpha;

        protected bool _textureLoaded;

        #region Propriétés
        /// <summary>
        /// Active ou désactive l'objet (Influance la mise à jour et l'affichage)
        /// </summary>
        public new bool Active
        {
            get { return !_paused && _visible && !_dirty; }
            set 
            { 
                if (value)
                {
                    _visible = true;
                    _paused = false;
                    _dirty = false;
                }
                else
                {
                    _visible = false;
                    _paused = true;
                    _dirty = false;
                }
            }
        }

        /// <summary>
        /// Active ou désactive l'affichage de l'objet (Influance l'affichage)
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }


        /// <summary>
        /// Indique si l'objet doit être détruit totalement
        /// </summary>
        public new bool Dirty
        {
            get { return _dirty; }
            set 
            { 
                _dirty = value;

                if (value)
                {
                    _paused = true;
                    _visible = false;
                }
                else
                {
                    _paused = false;
                    _visible = true;
                }
            }
        }

        /// <summary>
        /// Indique si l'objet doit suivre la caméra sur X et/ou Y
        /// </summary>
        public ScrollFactor ScrollFactor
        {
            get { return _scrollFactor; }
            set { _scrollFactor = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Rectangle Rectangle
        {
            get { return _rectangle; }
            set { _rectangle = value; }
        }

        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        public string TextureName
        {
            get { return _textureName; }
            set { _textureName = value; }
        }

        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        public Vector2 Scale
        {
            get { return _scale; }
            set 
            { 
                _scale = value;
                Rectangle = new Rectangle(X, Y, (int)(_scale.X * Width), (int)(_scale.Y * Height));
            }
        }
        
        public float Alpha
        {
        	get { return _alpha; }
        	set 
        	{
        		_alpha = value;

        		if (_alpha > 1.0)
        			_alpha = 1.0f;
        		else if (_alpha < 0)
        			_alpha = 0.0f;
        	}
        }

        public int X
        {
            get { return (int)_position.X; }
            set 
            {
                _position = new Vector2(value, Y);
                _rectangle = new Rectangle(value, Y, Width, Height);
            }
        }

        public int Y
        {
            get { return (int)_position.Y; }
            set 
            { 
                _position = new Vector2(X, value);
                _rectangle = new Rectangle(X, value, Width, Height);
            }
        }

        public int Height
        {
            get { return _rectangle.Height; }
            set { _rectangle = new Rectangle(X, Y, Width, value); }
        }

        public int Width
        {
            get { return _rectangle.Width; }
            set { _rectangle = new Rectangle(X, Y, value, Height); }
        }

        public float ScaledWidth
        {
            get { return Width * Scale.X; }
        }

        public float ScaledHeight
        {
            get { return Height * Scale.Y; }
        }

        /// <summary>
        /// Indique si la texture est chargée
        /// </summary>
        public bool TextureLoaded
        {
            get { return _textureLoaded; }
        }
        #endregion

        /// <summary>
        /// Construction d'un objet graphique de base
        /// </summary>
        public YnObject() 
            : base ()
        {
            _visible = true;
            _scrollFactor = new ScrollFactor();

            _position = new Vector2(0, 0);
            _rectangle = Rectangle.Empty;
            _texture = null;
            _textureName = String.Empty;
            _color = Color.White;
            _rotation = 0.0f;
            _origin = Vector2.Zero;
            _scale = Vector2.One;
            _alpha = 1.0f;
        }

        public abstract void Initialize();

        public abstract void LoadContent();

        public abstract void UnloadContent();

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        /// <summary>
        /// Flag qui permet d'indiquer que cet objet doit être détruit
        /// La propriété Active est passée à false, l'objet est candidat à la destruction
        /// </summary>
        public virtual void Die()
        {
            Dirty = true;
        }

        /// <summary>
        /// Permet de recycler l'objet en lui enlevant le flag de candidat à la destruction
        /// </summary>
        public virtual void Recycle()
        {
            Dirty = false;
            Revive();
        }

        /// <summary>
        /// Permet de désactiver un objet 
        /// Les propriétés Active, Pause et Visible sont mise à false 
        /// </summary>
        public virtual void Kill()
        {
            Active = false;
            Visible = false;
        }

        /// <summary>
        /// Permet de faire revivre l'objet
        /// Les propriétés Active, Pause et Visibles sont mise à true
        /// </summary>
        public virtual void Revive()
        {
            Active = true;
            Visible = true;
        }
        
        /// <summary>
        /// Change le point d'origine de l'objet
        /// </summary>
        /// <param name="spriteOrigin">Enumeration SpriteOrigin définissant les différents points prédeterminés</param>
        public void SetOriginTo(ObjectOrigin spriteOrigin)
        {
            switch (spriteOrigin)
            {
                case ObjectOrigin.TopLeft:
                    _origin = Vector2.Zero;
                    break;
                case ObjectOrigin.Top:
                    _origin = new Vector2(Width / 2, 0);
                    break;
                case ObjectOrigin.TopRight:
                    _origin = new Vector2(Width, 0);
                    break;
                case ObjectOrigin.Left:
                    _origin = new Vector2(0, Height / 2);
                    break;
                case ObjectOrigin.Center:
                    _origin = new Vector2(Width / 2, Height / 2);
                    break;
                case ObjectOrigin.Right:
                    _origin = new Vector2(Width, Height / 2);
                    break;
                case ObjectOrigin.BottomLeft:
                    _origin = new Vector2(0, Height);
                    break;
                case ObjectOrigin.Bottom:
                    _origin = new Vector2(Width / 2, Height);
                    break;
                case ObjectOrigin.BottomRight:
                    _origin = new Vector2(Width, Height);
                    break;
            } 		
        }
    }
}
