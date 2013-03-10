using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Yna.Engine.Collision;
using Yna.Engine.Graphics.Event;
using Yna.Engine.Input;

namespace Yna.Engine.Graphics
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
    public class YnEntity : YnBase, IDrawableEntity, ICollidable2
    {
        #region Protected and private declarations

        protected bool _dirty;

        // Define if the object is updated and drawn
        protected bool _visible;

        protected bool _hovered;
        protected bool _clicked;

        // Sprite position and rectangle
        protected Vector2 _position;
        protected Rectangle _rectangle;

        // Texture
        protected Texture2D _texture;
        protected string _assetName;
        protected bool _assetLoaded;

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
        protected PositionType _positionType;
        protected int _nbMouseEventObservers;
        protected int _nbTouchEventObservers;

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

        #endregion

        #region Properties for position/rotation/scale/etc..

        /// <summary>
        /// Gets or sets the positionment type for this object
        /// Relative to its parent or absolute
        /// </summary>
        public PositionType PositionType
        {
            get { return _positionType; }
            set 
            {
                _positionType = value;
                if (value == Graphics.PositionType.Relative)
                {
                    if (_parent != null)
                        Position += _parent.Position;
                }
                else
                {
                    if (_parent != null)
                        Position -= _parent.Position;
                }
            }
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
            get
            {
                return _position;
            }
            set
            {
                NormalizePositionType(ref value);

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
                NormalizePositionType(ref value);

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
                if (_positionType == PositionType.Relative && _parent != null)
                    value += _parent.X;

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
                if (_positionType == PositionType.Relative && _parent != null)
                    value += _parent.Y;

                _position.Y = value;
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

        #region Global events

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

        #region Touch events

        private event EventHandler<TouchActionEntityEventArgs> _touchPress = null;
        private event EventHandler<TouchActionEntityEventArgs> _touchOver = null;
        private event EventHandler<TouchLeaveEntityEventArgs> _touchLeave = null;
        private event EventHandler<TouchActionEntityEventArgs> _touchOut = null;

        /// <summary>
        /// Triggered on first finger press
        /// </summary>
        public event EventHandler<TouchActionEntityEventArgs> TouchPress
        {
            add
            {
                _touchPress += value;
                _nbTouchEventObservers++;
            }
            remove
            {
                _touchPress -= value;
                _nbTouchEventObservers--;
            }
        }

        /// <summary>
        /// Triggered on first finger is over an entity.
        /// </summary>
        public event EventHandler<TouchActionEntityEventArgs> TouchOver
        {
            add
            {
                _touchOver += value;
                _nbTouchEventObservers++;
            }
            remove
            {
                _touchOver -= value;
                _nbTouchEventObservers--;
            }
        }

        /// <summary>
        /// Triggered on first finger leave an entity.
        /// </summary>
        public event EventHandler<TouchLeaveEntityEventArgs> TouchLeave
        {
            add
            {
                _touchLeave += value;
                _nbTouchEventObservers++;
            }
            remove
            {
                _touchLeave -= value;
                _nbTouchEventObservers--;
            }
        }

        /// <summary>
        /// Triggered on first finger is out of the entity.
        /// </summary>
        public event EventHandler<TouchActionEntityEventArgs> TouchOut
        {
            add
            {
                _touchOut += value;
                _nbTouchEventObservers++;
            }
            remove
            {
                _touchOut -= value;
                _nbTouchEventObservers--;
            }
        }

        private void OnTouchPress(TouchActionEntityEventArgs e)
        {
            if (_touchPress != null)
                _touchPress(this, e);
        }

        private void OnTouchOver(TouchActionEntityEventArgs e)
        {
            if (_touchOver != null)
                _touchOver(this, e);
        }

        private void OnTouchLeave(TouchLeaveEntityEventArgs e)
        {
            if (_touchLeave != null)
                _touchLeave(this, e);
        }

        public void OnTouchOut(TouchActionEntityEventArgs e)
        {
            if (_touchOut != null)
                _touchOut(this, e);
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
            _visible = true;
            _dirty = false;
            _hovered = false;

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
            _nbMouseEventObservers = 0;
            _nbTouchEventObservers = 0;
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

        public virtual void Initialize()
        {

        }

        public virtual void LoadContent()
        {
            if (!_assetLoaded && _assetName != String.Empty)
            {
                _texture = YnG.Content.Load<Texture2D>(_assetName);
                _rectangle.Width = _texture.Width;
                _rectangle.Height = _texture.Height;
                _assetLoaded = true;
            }
        }

        public virtual void LoadContent(bool forceReload)
        {
            _assetLoaded = forceReload;
            LoadContent();
        }

        /// <summary>
        /// Dispose the Texture2D object if the Dirty property is set to true
        /// </summary>
        public virtual void UnloadContent()
        {
            if (_texture != null && _dirty)
                _texture.Dispose();
        }

        public override void Update(GameTime gameTime)
        {
        	// Reset the flags
        	_hovered = false;
            _clicked = false;
        	
            // Check mouse events
            if (_enabled)
            {
                _rectangle.X = (int)_position.X;
                _rectangle.Y = (int)_position.Y;

                #region Touch events

                // We check if the mouse events only if an event handler exists for one of mouse events
                if (_nbTouchEventObservers > 0)
                {
                    int fingerId = 0;
                    Vector2 touchPosition = YnG.Touch.GetPosition(fingerId); // TODO : Add a solution for all fingers
                    Vector2 lastTouchPosition = YnG.Touch.GetLastPosition(fingerId);

                    if (_rectangle.Contains((int)touchPosition.X, (int)touchPosition.Y))
                    {
                    	_hovered = true;
                        OnTouchOver(new TouchActionEntityEventArgs((int)touchPosition.X, (int)touchPosition.Y, fingerId, YnG.Touch.Tapped, YnG.Touch.Moved, YnG.Touch.Released, YnG.Touch.GetPressureLevel(fingerId)));

                        if (YnG.Touch.Tapped)
                        {
                            _clicked = true;
                            OnTouchPress(new TouchActionEntityEventArgs((int)touchPosition.X, (int)touchPosition.Y, fingerId, YnG.Touch.Tapped, YnG.Touch.Moved, YnG.Touch.Released, YnG.Touch.GetPressureLevel(fingerId)));
                        }
                    }
                    else if (Rectangle.Contains((int)lastTouchPosition.X, (int)lastTouchPosition.Y))
                        OnTouchLeave(new TouchLeaveEntityEventArgs((int)lastTouchPosition.X, (int)lastTouchPosition.Y, fingerId, (int)touchPosition.X, (int)touchPosition.Y));
                    else
                        OnTouchOut(new TouchActionEntityEventArgs((int)touchPosition.X, (int)touchPosition.Y, fingerId, YnG.Touch.Tapped, YnG.Touch.Moved, YnG.Touch.Released, YnG.Touch.GetPressureLevel(fingerId)));
                }

                #endregion

                #region Mouse events

                // We check if the mouse events only if an event handler exists for one of mouse events
                if (_nbMouseEventObservers > 0)
                {
                    if (_rectangle.Contains(YnG.Mouse.X, YnG.Mouse.Y))
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
                            _clicked = true;
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

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_visible)
                spriteBatch.Draw(_texture, _rectangle, null, _color * _alpha, _rotation, _origin, _effects, _layerDepth);
        }

        #endregion

        #region Live, Die, Revive methods

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

        #region Other methods

        public virtual void AddAbsolutePosition(int x, int y)
        {
            _position.X += x;
            _position.Y += y;
            _rectangle.X += x;
            _rectangle.Y += y;
        }

        public virtual void SetAbsolutePosition(int x, int y)
        {
            _position.X = x;
            _position.Y = y;
            _rectangle.X = x;
            _rectangle.Y = y;
        }

        /// <summary>
        /// Gets an adapted position relative to the position type of the sprite
        /// </summary>
        /// <param name="position">The position must be set</param>
        protected virtual void NormalizePositionType(ref Vector2 position)
        {
            if (_positionType == PositionType.Relative && _parent != null)
            {
                position.X += _parent.X;
                position.Y += _parent.Y;
            }
        }

        /// <summary>
        /// Gets an adapted position relative to the position type of the entity
        /// </summary>
        /// <param name="position">The position must be set</param>
        protected virtual void NormalizePositionType(ref Rectangle rectangle)
        {
            if (_positionType == PositionType.Relative && _parent != null)
            {
                rectangle.X += _parent.X;
                rectangle.Y += _parent.Y;
            }
        }

        /// <summary>
        /// Change the origin of the object
        /// </summary>
        /// <param name="spriteOrigin">Determinated point of origin</param>
        public void SetOriginTo(ObjectOrigin spriteOrigin)
        {
            switch (spriteOrigin)
            {
                case ObjectOrigin.TopLeft: _origin = Vector2.Zero; break;
                case ObjectOrigin.Top: _origin = new Vector2(Width / 2, 0); break;
                case ObjectOrigin.TopRight: _origin = new Vector2(Width, 0); break;
                case ObjectOrigin.Left: _origin = new Vector2(0, Height / 2); break;
                case ObjectOrigin.Center: _origin = new Vector2(Width / 2, Height / 2); break;
                case ObjectOrigin.Right: _origin = new Vector2(Width, Height / 2); break;
                case ObjectOrigin.BottomLeft: _origin = new Vector2(0, Height); break;
                case ObjectOrigin.Bottom: _origin = new Vector2(Width / 2, Height); break;
                case ObjectOrigin.BottomRight: _origin = new Vector2(Width, Height); break;
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