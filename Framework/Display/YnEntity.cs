﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Framework.Helpers;
using Yna.Framework.Input;
using Yna.Framework.Display.Event;

namespace Yna.Framework.Display
{
    #region Enumuration definition

    /// <summary>
    /// Define the origin of an object
    /// </summary>
    public enum ObjectOrigin
    {
        TopLeft = 0, Top, TopRight, Left, Center, Right, BottomLeft, Bottom, BottomRight
    }

    /// <summary>
    /// Define the type of position used by an object
    /// </summary>
    public enum PositionType
    {
        Absolute = 0, Relative
    }

    #endregion

    /// <summary>
    /// A basic drawable object
    /// </summary>
    public class YnEntity : YnBase
    {
        #region Protected and private declarations

        protected bool _dirty;

        // Define if the object is updated and drawn
        protected bool _visible;

        // Sprite default values
        protected Vector2 _position;
        protected Rectangle _rectangle;
        protected Texture2D _texture;
        protected string _assetName;
        protected bool _assetLoaded;
        protected Color _color;
        protected float _rotation;
        protected Vector2 _origin;
        protected Vector2 _scale;
        protected SpriteEffects _effects;
        protected float _layerDepth;
        protected float _alpha;

        // Define the position of the sprite relative to its parent
        protected YnEntity _parent;
        protected PositionType _positionType;

        #endregion

        #region Base properties

        /// <summary>
        /// Flags who determine if this object must be cleaned and removed
        /// </summary>
        public bool Dirty
        {
            get { return _dirty; }
            set
            {
                _dirty = value;
                _enabled = !value;
                _visible = !value;
            }
        }

        /// <summary>
        /// Gets or sets the status of the object
        /// If true the object is not paused and is visible
        /// Else it's paused and not visible
        /// </summary>
        public new bool Active
        {
            get { return _enabled && _visible && !_dirty; }
            set
            {
                _visible = value;
                _enabled = value;
                _dirty = !value;
            }
        }

        /// <summary>
        /// Gets or sets the visibility status of the object
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        #endregion

        #region Properties for position/rotation/scale/etc..

        /// <summary>
        /// Gets or sets the positionment type for this object
        /// Relative to its parent or absolute
        /// </summary>
        public PositionType PositionType
        {
            get { return _positionType; }
            set { _positionType = value; }
        }

        /// <summary>
        /// Gets or sets the parent object of this object (null if don't have a parent)
        /// </summary>
        public YnEntity Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Gets or sets the position of the object
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
        /// Gets or sets the Rectangle (Bounding box) of the object
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
        /// Gets or sets the origin of the object. Default is Vector2.Zero
        /// </summary>
        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        /// <summary>
        /// Gets or sets the scale of the object.Default is Vector2.One
        /// Note: The rectangle of the object is updated
        /// </summary>
        public Vector2 Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                _rectangle = new Rectangle((int)_position.X, (int)_position.Y, (int)(_scale.X * _rectangle.Width), (int)(_scale.Y * _rectangle.Height));
            }
        }

        /// <summary>
        /// Gets or sets the color applied to the object
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Gets or sets the rotation of the object (deg)
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        /// <summary>
        /// Gets or sets the Alpha applied to the object. Value between 0.0f and 1.0f
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
        /// Gets or sets the SpriteEffect used for drawing the sprite
        /// </summary>
        public SpriteEffects Effects
        {
            get { return _effects; }
            set { _effects = value; }
        }

        /// <summary>
        /// Gets or sets the layer depth
        /// </summary>
        public float LayerDepth
        {
            get { return _layerDepth; }
            set { _layerDepth = value; }
        }

        /// <summary>
        /// Gets or sets the X position of the object.
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
        /// Gets or sets the Y position of the object.
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
        /// Gets or sets the height of the rectangle
        /// Note: who is the texture2D.Height value
        /// </summary>
        public int Height
        {
            get { return _rectangle.Height; }
            set { _rectangle.Height = value; }
        }

        /// <summary>
        /// Gets or sets the width of the rectangle
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
            get { return _rectangle.Width * Scale.X; }
        }

        /// <summary>
        /// Get the height with scale
        /// </summary>
        public float ScaledHeight
        {
            get { return _rectangle.Height * Scale.Y; }
        }

        #endregion

        #region Properties for asset

        /// <summary>
        /// Gets or sets the Texture2D used by the object
        /// </summary>
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        /// <summary>
        /// Get the status of the texture2D
        /// True if loaded
        /// </summary>
        public bool AssetLoaded
        {
            get { return _assetLoaded; }
        }

        /// <summary>
        /// Gets or sets the texture name used when the content is loaded
        /// </summary>
        public string AssetName
        {
            get { return _assetName; }
            set
            {
                _assetLoaded = false;
                _assetName = value;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Triggered when the object was killed
        /// </summary>
        public event EventHandler<EventArgs> Killed = null;

        /// <summary>
        /// Triggered when the object was revived
        /// </summary>
        public event EventHandler<EventArgs> Revived = null;

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
        public event EventHandler<MouseClickSpriteEventArgs> JustClicked = null;

        /// <summary>
        /// Triggered when click are detected over the object
        /// </summary>
        public event EventHandler<MouseClickSpriteEventArgs> Click = null;

        private void KillSprite(EventArgs e)
        {
            if (Killed != null)
                Killed(this, e);
        }

        private void ReviveSprite(EventArgs e)
        {
            if (Revived != null)
                Revived(this, e);
        }

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
            if (JustClicked != null)
                JustClicked(this, e);
        }

        private void MouseClickSprite(MouseClickSpriteEventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }

        #endregion

        /// <summary>
        /// A basic graphic object
        /// </summary>
        public YnEntity()
            : base()
        {
            _visible = true;
            _dirty = false;
            
            _position = Vector2.Zero;
            _rectangle = Rectangle.Empty;
            _texture = null;
            _assetName = String.Empty;
            _assetLoaded = false;
            _color = Color.White;
            _rotation = 0.0f;
            _origin = Vector2.Zero;
            _scale = Vector2.One;
            _alpha = 1.0f;
            _effects = SpriteEffects.None;
            _layerDepth = 1.0f;

            _parent = null;
            _positionType = PositionType.Absolute;
        }

        public YnEntity(string assetName)
            : this()
        {
            _assetName = assetName;
        }

        public YnEntity(string assetName, Vector2 position)
            : this(assetName)
        {
            _position = position;
        }

        #region GameState pattern

        public virtual void Initialize()
        {

        }

        public virtual void LoadContent()
        {
            if (_texture == null && _assetName != String.Empty)
            {
                _texture = YnG.Content.Load<Texture2D>(_assetName);
                _rectangle.Width = _texture.Width;
                _rectangle.Height = _texture.Height;
                _assetLoaded = true;
            }
        }

        /// <summary>
        /// Dispose the Texture2D object if the Dirty property is set to true
        /// </summary>
        public virtual void UnloadContent()
        {
            if (_texture != null && Dirty)
                _texture.Dispose();
        }

        public override void Update(GameTime gameTime)
        {
            // Check mouse events
            if (_enabled)
            {
                Rectangle = new Rectangle(X, Y, Width, Height);

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

                        MouseJustClickedSprite(new MouseClickSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y, mouseButton, true, false));
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

                        MouseClickSprite(new MouseClickSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y, mouseButton, false, false));
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

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_visible)
                spriteBatch.Draw(_texture, _rectangle, null, _color * _alpha, _rotation, _origin, _effects, _layerDepth);
        }

        #endregion

        #region Live and die methods

        /// <summary>
        /// Flag the object for a purge action
        /// </summary>
        public virtual void Die()
        {
            Dirty = true;
        }

        /// <summary>
        /// Kill the object, it's no more updated and drawed
        /// Pause is setted on true and Visible is setted to false
        /// </summary>
        public virtual void Kill()
        {
            Active = false;
            KillSprite(EventArgs.Empty);
        }

        /// <summary>
        /// Revive the object, it's updated and drawed
        /// Pause is setted on false and Visible is setted to true
        /// </summary>
        public virtual void Revive()
        {
            Active = true;
            ReviveSprite(EventArgs.Empty);
        }

        #endregion

        #region Change origin

        /// <summary>
        /// Change the origin of the object
        /// </summary>
        /// <param name="spriteOrigin">Determinated point of origin</param>
        public void SetOriginTo(ObjectOrigin spriteOrigin)
        {
            switch (spriteOrigin)
            {
                case ObjectOrigin.TopLeft:     _origin = Vector2.Zero;                        break;
                case ObjectOrigin.Top:         _origin = new Vector2(Width / 2, 0);           break;
                case ObjectOrigin.TopRight:    _origin = new Vector2(Width, 0);               break;
                case ObjectOrigin.Left:        _origin = new Vector2(0, Height / 2);          break;
                case ObjectOrigin.Center:      _origin = new Vector2(Width / 2, Height / 2);  break;
                case ObjectOrigin.Right:       _origin = new Vector2(Width, Height / 2);      break;
                case ObjectOrigin.BottomLeft:  _origin = new Vector2(0, Height);              break;
                case ObjectOrigin.Bottom:      _origin = new Vector2(Width / 2, Height);      break;
                case ObjectOrigin.BottomRight: _origin = new Vector2(Width, Height);          break;
            }
        }

        #endregion

        public void SetFullScreen()
        {
            Rectangle = new Rectangle(0, 0, YnG.Width, YnG.Height);
        }
    }
}