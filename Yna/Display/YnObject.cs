using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Helpers;
using Yna.Input;
using Yna.Display.Event;

namespace Yna.Display
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

        protected YnObject _parent;

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

        #region Properties

        /// <summary>
        /// Get or Set the status of the object
        /// If true the object is not paused and is visible
        /// Else it's paused and not visible
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
        /// Get or Set the visibility status of the object
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }


        /// <summary>
        /// Determine if the object must be purged (hard remove)
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
        /// Get or set the parent object of this object (null if don't have a parent)
        /// </summary>
        public YnObject Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Get or Set the color applied to the object
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Get or Set the rotation of the object (deg)
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        /// <summary>
        /// Get or Set the position of the object
        /// Note: The rectangle values are updated
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set 
            { 
                _position = value;

                _rectangle.X = (int)_position.X;
                _rectangle.Y = (int)_position.Y;
            }
        }

        /// <summary>
        /// Get or Set the Rectangle (Bounding box) of the object
        /// Note: The position values are updated
        /// </summary>
        public Rectangle Rectangle
        {
            get { return _rectangle; }
            set 
            { 
                _rectangle = value;

                _position.X = _rectangle.X;
                _position.Y = _rectangle.Y;
            }
        }

        /// <summary>
        /// Get or Set the Texture2D used by the object
        /// </summary>
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        /// <summary>
        /// Get or Set the texture name used when the content is loaded
        /// </summary>
        public string TextureName
        {
            get { return _textureName; }
            set { _textureName = value; }
        }

        /// <summary>
        /// Get or Set the origin of the object. Default is Vector2.Zero
        /// </summary>
        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        /// <summary>
        /// Get or Set the scale of the object.Default is Vector2.One
        /// Note: The rectangle of the object is updated
        /// </summary>
        public Vector2 Scale
        {
            get { return _scale; }
            set 
            { 
                _scale = value;
                Rectangle = new Rectangle(X, Y, (int)(_scale.X * Width), (int)(_scale.Y * Height));
            }
        }
        
        /// <summary>
        /// Get or Set the Alpha applied to the object. Value between 0.0f and 1.0f
        /// </summary>
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

        /// <summary>
        /// Get or Set the X position of the object.
        /// Note: The rectangle is updated when a value is setted
        /// </summary>
        public int X
        {
            get { return (int)_position.X; }
            set 
            {
                _position.X = value;
                _rectangle.X = value;
            }
        }

        /// <summary>
        /// Get or Set the Y position of the object.
        /// Note: The rectangle value is updated when a value is setted
        /// </summary>
        public int Y
        {
            get { return (int)_position.Y; }
            set 
            { 
                _position = new Vector2(X, value);
                _rectangle.Y = value;
            }
        }

        /// <summary>
        /// Get or Set the height of the rectangle
        /// Note: who is the texture2D.Height value
        /// </summary>
        public int Height
        {
            get { return _rectangle.Height; }
            set { _rectangle.Height = value; }
        }

        /// <summary>
        /// Get or Set the width of the rectangle
        /// Note: who is the texture2D.Width value
        /// </summary>
        public int Width
        {
            get { return _rectangle.Width; }
            set { _rectangle.Width = value; }
        }

        /// <summary>
        /// Get the width with scale
        /// </summary>
        public float ScaledWidth
        {
            get { return Width * Scale.X; }
        }

        /// <summary>
        /// Get the height with scale
        /// </summary>
        public float ScaledHeight
        {
            get { return Height * Scale.Y; }
        }

        /// <summary>
        /// Get the status of the texture2D
        /// True if loaded
        /// </summary>
        public bool TextureLoaded
        {
            get { return _textureLoaded; }
        }
        #endregion

        #region Events

        /// <summary>
        /// Triggered when the mouse is over the object
        /// </summary>
        public event EventHandler<MouseOverSpriteEventArgs> MouseOver = null;

        /// <summary>
        /// Triggered when the mouse leave the object
        /// </summary>
        public event EventHandler<MouseLeaveSpriteEventArgs> MouseLeave = null;

        /// <summary>
        /// Triggered when a click (and just one) is detected over the object
        /// </summary>
        public event EventHandler<MouseClickSpriteEventArgs> MouseJustClicked = null;

        /// <summary>
        /// Triggered when click are detected over the object
        /// </summary>
        public event EventHandler<MouseClickSpriteEventArgs> MouseClick = null;

        private void MouseOverSprite(MouseOverSpriteEventArgs e)
        {
            if (MouseOver != null)
                MouseOver(this, e);
        }

        private void MouseLeaveSprite(MouseLeaveSpriteEventArgs e)
        {
            if (MouseLeave != null)
                MouseLeave(this, e);
        }

        private void MouseJustClickedSprite(MouseClickSpriteEventArgs e)
        {
            if (MouseJustClicked != null)
                MouseJustClicked(this, e);
        }

        private void MouseClickSprite(MouseClickSpriteEventArgs e)
        {
            if (MouseClick != null)
                MouseClick(this, e);
        }

        #endregion

        /// <summary>
        /// A basic graphic object
        /// </summary>
        public YnObject() 
            : base ()
        {
            _visible = true;
            _parent = null;
            
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

        public override void Update(GameTime gameTime)
        {
            // Check mouse events
            if (!Pause)
            {
                #region Mouse events

                // Souris
                if (Rectangle.Contains(YnG.Mouse.X, YnG.Mouse.Y))
                {
                    // Mouse Over
                    MouseOverSprite(new MouseOverSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y));

                    // Just clicked
                    if (YnG.Mouse.JustClicked(MouseButton.Left) || YnG.Mouse.JustClicked(MouseButton.Middle) || YnG.Mouse.JustClicked(MouseButton.Right))
                    {
                        MouseButton mouseButton;

                        if (YnG.Mouse.JustClicked(MouseButton.Left))
                            mouseButton = MouseButton.Left;
                        else if (YnG.Mouse.JustClicked(MouseButton.Middle))
                            mouseButton = MouseButton.Middle;
                        else
                            mouseButton = MouseButton.Right;

                        MouseJustClickedSprite(new MouseClickSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y, mouseButton, true));
                    }

                    // one click
                    if (YnG.Mouse.ClickOn(MouseButton.Left, ButtonState.Pressed) || YnG.Mouse.ClickOn(MouseButton.Middle, ButtonState.Pressed) || YnG.Mouse.ClickOn(MouseButton.Right, ButtonState.Pressed))
                    {
                        MouseButton mouseButton;

                        if (YnG.Mouse.ClickOn(MouseButton.Left, ButtonState.Pressed))
                            mouseButton = MouseButton.Left;
                        else if (YnG.Mouse.ClickOn(MouseButton.Middle, ButtonState.Pressed))
                            mouseButton = MouseButton.Middle;
                        else
                            mouseButton = MouseButton.Right;

                        MouseClickSprite(new MouseClickSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y, mouseButton, false));
                    }
                }
                // Mouse leave
                else if (Rectangle.Contains(YnG.Mouse.LastMouseState.X, YnG.Mouse.LastMouseState.Y))
                {
                    MouseLeaveSprite(new MouseLeaveSpriteEventArgs(YnG.Mouse.LastMouseState.X, YnG.Mouse.LastMouseState.Y, YnG.Mouse.X, YnG.Mouse.Y));
                }

                #endregion
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, ref Rectangle rectangle)
        {
            if (Active)
                spriteBatch.Draw(Texture, Rectangle, Color);
        }

        /// <summary>
        /// Flag the object for a purge action
        /// </summary>
        public virtual void Die()
        {
            Dirty = true;
        }

        /// <summary>
        /// Recycle object
        /// </summary>
        public virtual void Recycle()
        {
            Dirty = false;
            Revive();
        }

        /// <summary>
        /// Kill the object, it's no more updated and drawed
        /// Pause is setted on true and Visible is setted to false
        /// </summary>
        public virtual void Kill()
        {
            Active = false;
            Visible = false;
        }

        /// <summary>
        /// Revive the object, it's updated and drawed
        /// Pause is setted on false and Visible is setted to true
        /// </summary>
        public virtual void Revive()
        {
            Active = true;
            Visible = true;
        }
        
        /// <summary>
        /// Change the origin of the object
        /// </summary>
        /// <param name="spriteOrigin">Determinated point of origin</param>
        public void SetOriginTo(ObjectOrigin spriteOrigin)
        {
            switch (spriteOrigin)
            {
                case ObjectOrigin.TopLeft:      _origin = Vector2.Zero;                       break;
                case ObjectOrigin.Top:          _origin = new Vector2(Width / 2, 0);          break;
                case ObjectOrigin.TopRight:     _origin = new Vector2(Width, 0);              break;
                case ObjectOrigin.Left:         _origin = new Vector2(0, Height / 2);         break;
                case ObjectOrigin.Center:       _origin = new Vector2(Width / 2, Height / 2); break;
                case ObjectOrigin.Right:        _origin = new Vector2(Width, Height / 2);     break;
                case ObjectOrigin.BottomLeft:   _origin = new Vector2(0, Height);             break;
                case ObjectOrigin.Bottom:       _origin = new Vector2(Width / 2, Height);     break;
                case ObjectOrigin.BottomRight:  _origin = new Vector2(Width, Height);         break;
            } 		
        }
    }
}
