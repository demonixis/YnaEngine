// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Yna.Engine.Graphics.Event;
using Yna.Engine.Input;

namespace Yna.Engine.Graphics
{
    #region Enumuration definition

    /// <summary>
    /// Define the origin of an object
    /// </summary>
    public enum SpriteOrigin
    {
        TopLeft = 0, Top, TopRight, Left, Center, Right, BottomLeft, Bottom, BottomRight
    }

    #endregion

    /// <summary>
    /// A basic drawable object
    /// </summary>
    public class YnEntity : YnGameEntity, ICollidable2
    {
        #region Protected and private declarations

        protected bool _dirty;

        // Sprite position and rectangle
        protected Vector2 _position;
        protected Vector2 _screenPosition;
        protected Rectangle _rectangle;

        // Texture
        protected Texture2D _texture;
        protected string _assetName;

        // Draw params
        protected Color _color;
        protected float _rotation;
        protected Vector2 _origin;
        protected Vector2 _scale;
        protected SpriteEffects _effects;
        protected float _layerDepth;
        protected float _alpha;

        // Define the position of the sprite relative to its parent
        protected YnEntity _parent;
        protected int _nbMouseEventObservers;

        private Rectangle _testEventRectangle;
        protected bool _clicked;
        protected bool _hovered;

        #endregion

        #region Base Properties

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
        /// Flag indicating that the entity is currently hovered (touch or mouse)
        /// </summary>
        public bool Hovered
        {
            get { return _hovered; }
        }

        /// <summary>
        /// Flag indicating that the entity is currently beeing clicked (touch or mouse)
        /// </summary>
        public bool Clicked
        {
            get { return _clicked; }
        }

        public Vector2 ScreenPosition
        {
            get { return _screenPosition; }
        }

        #endregion

        #region Transform fields

        /// <summary>
        /// Gets or sets the parent object of this object (null if don't have a parent)
        /// </summary>
        public YnEntity Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Gets or sets the X position of the object.
        /// Note: The rectangle is updated when a value is setted
        /// </summary>
        public float X
        {
            get { return (int)_position.X; }
            set
            {
                _position.X = value;
                _rectangle.X = (int)value;
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
                _position.Y = value;
                _rectangle.Y = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the height of the rectangle
        /// Note: who is the texture2D.Height value
        /// </summary>
        public int Height
        {
            get { return _rectangle.Height; }
            set
            {
                if (_texture != null)
                    _scale.Y = (float)((float)value / (float)_texture.Height);

                _rectangle.Height = value;
            }
        }

        /// <summary>
        /// Gets or sets the width of the rectangle
        /// Note: who is the texture2D.Width value
        /// </summary>
        public int Width
        {
            get { return _rectangle.Width; }
            set
            {
                if (_texture != null)
                    _scale.X = (float)((float)value / (float)_texture.Width);

                _rectangle.Width = value;
            }
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
                _position.X = value.X;
                _position.Y = value.Y;
                _rectangle.X = (int)value.X;
                _rectangle.Y = (int)value.Y;
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
                if (_texture != null)
                {
                    _scale.X = (float)((float)value.Width / (float)_texture.Width);
                    _scale.Y = (float)((float)value.Height / (float)_texture.Height);
                }

                _rectangle = value;
                _position.X = value.X;
                _position.Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the rotation of the object in radians
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        /// <summary>
        /// Gets or sets the scale of the object.Default is Vector2.One
        /// Note: The rectangle of the object is updated
        /// </summary>
        public Vector2 Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        public float ScaledWidth
        {
            get { return _rectangle.Width * _scale.X; }
        }

        public float ScaledHeight
        {
            get { return _rectangle.Height * _scale.Y; }
        }

        #endregion

        #region Batch fields

        /// <summary>
        /// Gets or sets the origin of the object. Default is Vector2.Zero
        /// </summary>
        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value; }
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
        /// Gets or sets the Alpha applied to the object. Value between 0.0f and 1.0f
        /// </summary>
        public float Alpha
        {
            get { return _alpha; }
            set
            {
                _alpha = value > 1.0f ? 1.0f : value;
                _alpha = _alpha < 0.0f ? 0.0f : _alpha;
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

        #endregion

        #region Asset fields

        /// <summary>
        /// Gets or sets the Texture2D used by the object
        /// </summary>
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        /// <summary>
        /// Gets or sets the texture name used when the content is loaded
        /// </summary>
        public string AssetName
        {
            get { return _assetName; }
            set
            {
                if (_assetName != value)
                {
                    _assetLoaded = false;
                    _assetName = value;
                }
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

        #endregion

        #region Mouse events

        // Mouse events
        private event EventHandler<MouseOverEntityEventArgs> _mouseOver = null;
        private event EventHandler<MouseLeaveEntityEventArgs> _mouseLeave = null;
        private event EventHandler<MouseClickEntityEventArgs> _mouseClicked = null;
        private event EventHandler<MouseClickEntityEventArgs> _mouseClick = null;
        private event EventHandler<MouseReleaseEntityEventArgs> _mouseRelease = null;

        /// <summary>
        /// Triggered when the mouse is over the object
        /// </summary>
        public event EventHandler<MouseOverEntityEventArgs> MouseOver
        {
            add
            {
                _mouseOver += value;
                _nbMouseEventObservers++;
            }
            remove
            {
                _mouseOver -= value;
                _nbMouseEventObservers--;
            }
        }

        /// <summary>
        /// Triggered when the mouse leave the object
        /// </summary>
        public event EventHandler<MouseLeaveEntityEventArgs> MouseLeave
        {
            add
            {
                _mouseLeave += value;
                _nbMouseEventObservers++;
            }
            remove
            {
                _mouseLeave -= value;
                _nbMouseEventObservers--;
            }
        }

        /// <summary>
        /// Triggered when a click (and just one) is detected over the object
        /// </summary>
        public event EventHandler<MouseClickEntityEventArgs> MouseClicked
        {
            add
            {
                _mouseClicked += value;
                _nbMouseEventObservers++;
            }
            remove
            {
                _mouseClicked -= value;
                _nbMouseEventObservers--;
            }
        }

        /// <summary>
        /// Triggered when click are detected over the object
        /// </summary>
        public event EventHandler<MouseClickEntityEventArgs> MouseClick
        {
            add
            {
                _mouseClick += value;
                _nbMouseEventObservers++;
            }
            remove
            {
                _mouseClick -= value;
                _nbMouseEventObservers--;
            }
        }

        /// <summary>
        /// Triggered when click are detected over the object
        /// </summary>
        public event EventHandler<MouseReleaseEntityEventArgs> MouseReleased
        {
            add
            {
                _mouseRelease += value;
                _nbMouseEventObservers++;
            }
            remove
            {
                _mouseRelease -= value;
                _nbMouseEventObservers--;
            }
        }

        private void MouseOverSprite(MouseOverEntityEventArgs e)
        {
            if (_mouseOver != null)
                _mouseOver(this, e);
        }

        private void MouseLeaveSprite(MouseLeaveEntityEventArgs e)
        {
            if (_mouseLeave != null)
                _mouseLeave(this, e);
        }

        private void MouseJustClickedSprite(MouseClickEntityEventArgs e)
        {
            if (_mouseClicked != null)
                _mouseClicked(this, e);
        }

        private void MouseClickSprite(MouseClickEntityEventArgs e)
        {
            if (_mouseClick != null)
                _mouseClick(this, e);
        }

        private void MouseReleaseSprite(MouseReleaseEntityEventArgs e)
        {
            if (_mouseRelease != null)
                _mouseRelease(this, e);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// A basic entity who represent a graphic object on the 2D Scene
        /// </summary>
        public YnEntity()
            : base()
        {
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
            _nbMouseEventObservers = 0;
        }

        /// <summary>
        /// Create a new <see cref="Yna.Engine.Graphics.YnEntity"/> with a procedural texture.
        /// </summary>
        /// <param name='rectangle'>Rectangle of the texture</param>
        /// <param name='color'>Color of the texture</param>
        public YnEntity(Rectangle rectangle, Color color)
            : this()
        {
            _rectangle = rectangle;
            _position = rectangle.ToVector2();
            _texture = YnGraphics.CreateTexture(color, rectangle.Width, rectangle.Height);
            _assetLoaded = true;
        }

        /// <summary>
        /// A basic entity who represent a graphic object on the 2D Scene
        /// </summary>
        /// <param name="assetName">The asset name to use</param>
        public YnEntity(string assetName)
            : this()
        {
            _assetName = assetName;
        }

        /// <summary>
        /// A basic entity who represent a graphic object on the 2D Scene
        /// </summary>
        /// <param name="assetName">The asset name to use</param>
        /// <param name="position">The position for the entity</param>
        public YnEntity(string assetName, Vector2 position)
            : this(assetName)
        {
            Position = position;
        }

        #endregion

        #region GameState pattern

        /// <summary>
        /// Load asset.
        /// </summary>
        public override void LoadContent()
        {
            if (!_assetLoaded && _assetName != String.Empty)
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
        public override void UnloadContent()
        {
            if (_texture != null && _dirty)
                _texture.Dispose();
        }

        /// <summary>
        /// Update entity's logic and test mouse/touch events.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            ComputeScreenPosition();
            _rectangle.X = (int)_position.X;
            _rectangle.Y = (int)_position.Y;

            // Reset flags
            _clicked = false;
            _hovered = false;

            // Check mouse events
            if (_enabled)
            {
                _rectangle.X = (int)(ScreenPosition.X - _origin.X);
                _rectangle.Y = (int)(ScreenPosition.Y - _origin.Y);

                _testEventRectangle.X = (int)(ScreenPosition.X - _origin.X);
                _testEventRectangle.Y = (int)(ScreenPosition.Y - _origin.Y);
                _testEventRectangle.Width = (int)(_rectangle.Width * _scale.X);
                _testEventRectangle.Height = (int)(_rectangle.Height * _scale.Y);

                #region Mouse events

                // We check if the mouse events only if an event handler exists for one of mouse events
                if (_nbMouseEventObservers > 0)
                {
                    if (_testEventRectangle.Contains(YnG.Mouse.X, YnG.Mouse.Y))
                    {
                        _hovered = true;
                        // Mouse Over
                        MouseOverSprite(new MouseOverEntityEventArgs(YnG.Mouse.X, YnG.Mouse.Y));

                        // Just clicked
                        if (YnG.Mouse.JustClicked(MouseButton.Left) || YnG.Mouse.JustClicked(MouseButton.Middle) || YnG.Mouse.JustClicked(MouseButton.Right))
                        {
                            _clicked = true;
                            MouseButton mouseButton;

                            if (YnG.Mouse.JustClicked(MouseButton.Left))
                                mouseButton = MouseButton.Left;
                            else if (YnG.Mouse.JustClicked(MouseButton.Middle))
                                mouseButton = MouseButton.Middle;
                            else
                                mouseButton = MouseButton.Right;

                            MouseJustClickedSprite(new MouseClickEntityEventArgs(YnG.Mouse.X, YnG.Mouse.Y, mouseButton, true, false));
                        }

                        // One click
                        else if (YnG.Mouse.ClickOn(MouseButton.Left, ButtonState.Pressed) || YnG.Mouse.ClickOn(MouseButton.Middle, ButtonState.Pressed) || YnG.Mouse.ClickOn(MouseButton.Right, ButtonState.Pressed))
                        {
                            MouseButton mouseButton;

                            if (YnG.Mouse.ClickOn(MouseButton.Left, ButtonState.Pressed))
                                mouseButton = MouseButton.Left;
                            else if (YnG.Mouse.ClickOn(MouseButton.Middle, ButtonState.Pressed))
                                mouseButton = MouseButton.Middle;
                            else
                                mouseButton = MouseButton.Right;

                            MouseClickSprite(new MouseClickEntityEventArgs(YnG.Mouse.X, YnG.Mouse.Y, mouseButton, false, false));
                        }
                        else
                        {
                            MouseReleaseSprite(new MouseReleaseEntityEventArgs(YnG.Mouse.X, YnG.Mouse.Y));
                        }
                    }
                    // Mouse leave
                    else if (Rectangle.Contains(YnG.Mouse.LastMouseState.X, YnG.Mouse.LastMouseState.Y))
                    {
                        MouseLeaveSprite(new MouseLeaveEntityEventArgs(YnG.Mouse.LastMouseState.X, YnG.Mouse.LastMouseState.Y, YnG.Mouse.X, YnG.Mouse.Y));
                    }
                    else
                    {
                        MouseReleaseSprite(new MouseReleaseEntityEventArgs(YnG.Mouse.X, YnG.Mouse.Y));
                    }
                }
                #endregion
            }
        }

        private void ComputeScreenPosition()
        {
            if (_parent != null)
            {
                // Relative position to it's parent
                _screenPosition = Position + _parent.ScreenPosition;
            }
            else
            {
                // Absolute position
                _screenPosition = Position;
            }
        }

        /// <summary>
        /// Draw on screen.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_visible)
                spriteBatch.Draw(_texture, _position, null, _color * _alpha, _rotation, _origin, _scale, _effects, _layerDepth);
        }

        #endregion

        #region Life cycle

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

        #region Transform methods

        /// <summary>
        /// Ease the positionning. Sets the position of the entity.
        /// </summary>
        /// <param name="x">Position on X axis.</param>
        /// <param name="y">Position on Y axis.</param>
        public virtual void Move(float x, float y)
        {
            _position.X = x;
            _position.Y = y;
            _rectangle.X = (int)x;
            _rectangle.Y = (int)y;
        }

        public virtual void Move(ref Vector2 position)
        {
            Move(position.X, position.Y);
        }

        public virtual void Move(Vector2 position)
        {
            Move(ref position);
        }

        public virtual void Translate(Vector2 position)
        {
            Translate(ref position);
        }

        public virtual void Translate(ref Vector2 position)
        {
            Translate(position.X, position.Y);
        }

        public virtual void Translate(float x, float y)
        {
            _position.X += x;
            _position.Y += y;
            _rectangle.X += (int)x;
            _rectangle.Y += (int)y;
        }

        public virtual void SetSize(float width, float height)
        {
            _rectangle.Width = (int)width;
            _rectangle.Height = (int)height;
        }

        /// <summary>
        /// Change the origin of the object. Note that when initializing the object origin
        /// with this method, the origin point will be computed once. If you change the object's
        /// bounds afterwards, old origin will be kept as is and may not reflect the initially wanted 
        /// origin point.
        /// </summary>
        /// <param name="spriteOrigin">Determinated point of origin</param>
        public void SetOrigin(SpriteOrigin spriteOrigin)
        {
            switch (spriteOrigin)
            {
                case SpriteOrigin.TopLeft: _origin = Vector2.Zero; break;
                case SpriteOrigin.Top: _origin = new Vector2(ScaledWidth / 2, 0); break;
                case SpriteOrigin.TopRight: _origin = new Vector2(ScaledWidth, 0); break;
                case SpriteOrigin.Left: _origin = new Vector2(0, ScaledHeight / 2); break;
                case SpriteOrigin.Center: _origin = new Vector2(ScaledWidth / 2, ScaledHeight / 2); break;
                case SpriteOrigin.Right: _origin = new Vector2(ScaledWidth, ScaledHeight / 2); break;
                case SpriteOrigin.BottomLeft: _origin = new Vector2(0, ScaledHeight); break;
                case SpriteOrigin.Bottom: _origin = new Vector2(ScaledWidth / 2, ScaledHeight); break;
                case SpriteOrigin.BottomRight: _origin = new Vector2(ScaledWidth, ScaledHeight); break;
            }
        }

        /// <summary>
        /// Adapt the scale of the entity for taking fullscreen
        /// </summary>
        public void SetFullScreen()
        {
            Rectangle = new Rectangle(0, 0, YnG.Width, YnG.Height);
        }

        #endregion
    }
}