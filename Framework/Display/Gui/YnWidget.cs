using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display.Event;
using Yna.Input;
using Microsoft.Xna.Framework.Input;

namespace Yna.Display.Gui
{
    /// <summary>
    /// This enum represents widget children orientation in their parent (Horizontal : from left to right; 
    /// Vertical   : from top to bottom)
    /// </summary>
    public enum YnOrientation
    {
        Horizontal = 0, Vertical
    }

    /// <summary>
    /// Base class for GUI widgets
    /// </summary>
    public abstract class YnWidget
    {
        // Number of widget created, used for set the default name
        private static int WidgetCounter = 0;

        #region Attributes

        protected string _name;
        protected Rectangle _bounds;
        protected bool _withBackground;
        protected bool _withBorders;
        protected bool _visible;
        protected bool _active;
        protected YnWidget _parent;
        protected List<YnWidget> _children;
        protected YnOrientation _orientation;
        protected YnSkin _skin;
        protected int _padding;
        protected bool _pack;
        protected bool _hovered;

        #endregion

        #region Properties

        /// <summary>
        /// Get or Set the name identifier of the widget
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int X
        {
            get { return _bounds.X; }
            set { _bounds.X = value; }
        }

        public int Y
        {
            get { return _bounds.Y; }
            set { _bounds.Y = value; }
        }
        
        /// <summary>
        /// The widget relative position within it's parent if it has one, absolute otherwise (relative to the screen).
        /// </summary>
        public Vector2 Position
        {
            get { return new Vector2(_bounds.X, _bounds.Y); }
            set
            {
                _bounds.X = (int)value.X;
                _bounds.Y = (int)value.Y;
            }
        }

        /// <summary>
        /// The absolute position of the component on screen
        /// </summary>
        public Vector2 AbsolutePosition
        {
            get
            {
                Vector2 pos = Position;
                if (Parent != null)
                {
                    // The component has a parent so we grab it's relative position
                    pos += Parent.AbsolutePosition;
                }
                return pos;
            }
        }

        /// <summary>
        /// Set to true to render the default background image as background of this widget
        /// </summary>
        public bool WithBackground
        {
            get { return _withBackground; }
            set { _withBackground = value; }
        }

        /// <summary>
        /// Set to true to render the default border images
        /// </summary>
        public bool WithBorder
        {
            get { return _withBorders; }
            set { _withBorders = value; }
        }

        /// <summary>
        /// Set to false to stop rendering
        /// </summary>
        public bool IsVisible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        /// <summary>
        /// Set to false to stop updating
        /// </summary>
        public bool IsActive
        {
            get { return _active; }
            set { _active = value; }
        }

        /// <summary>
        /// The parent widget. If left empty, position will be considered as absolute. otherwise, 
        /// they will be considered as relative to the parent's bounds
        /// </summary>
        public YnWidget Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// This list contains all children contained in the widget
        /// </summary>
        public List<YnWidget> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        /// <summary>
        /// The widget bounds
        /// </summary>
        public Rectangle Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        /// <summary>
        /// The widget inner bounds : space left for children.
        /// </summary>
        public Rectangle InnerBounds { get; set; }

        /// <summary>
        /// The children rendering orientation
        /// </summary>
        public YnOrientation Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }

        /// <summary>
        /// Sets the vertical overflow
        /// </summary>
        public bool VerticalOverflow { get; set; }

        /// <summary>
        /// Sets the horizontal overflow
        /// </summary>
        public bool HorizontalOverflow { get; set; }

        /// <summary>
        /// The associated skin
        /// </summary>
        public YnSkin Skin
        {
            get { return _skin; }
            set { _skin = value; }
        }

        /// <summary>
        /// The padding between borders and inner widgets
        /// </summary>
        public int Padding
        {
            get { return _padding; }
            set { _padding = value; }
        }

        /// <summary>
        /// Pack the component to be the smallest possible according to it's children sizes
        /// </summary>
        public bool Pack
        {
            get { return _pack; }
            set { _pack = value; }
        }

        /// <summary>
        /// True if the widget is currently hovered by the mouse
        /// </summary>
        public bool IsHovered
        {
            get { return _hovered; }
            set { _hovered = value; }
        }

        /// <summary>
        /// Set to true to adjust children sizes to fit in the container
        /// </summary>
        public bool AdjustChildren { get; set; }

        public int Width
        {
            get { return _bounds.Width; }
            set { _bounds.Width = value; UpdateBounds(); }
        }

        public int Height
        {
            get { return _bounds.Height; }
            set { _bounds.Height = value; UpdateBounds(); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor. Initializes values
        /// </summary>
        protected YnWidget()
        {
            _withBackground = false;
            _withBorders = false;
            _visible = true;
            _active = true;
            _parent = null;
            _hovered = false;
            _children = new List<YnWidget>();
            _orientation = YnOrientation.Vertical;
            _bounds = Rectangle.Empty;
            Position = Vector2.Zero;

            _name = String.Format("YnWidget_{0}", YnWidget.WidgetCounter++);
        }

        #endregion

        #region Draw methods

        /// <summary>
        /// Draw the widget
        /// </summary>
        /// <param name="gameTime">Elasped time since last draw</param>
        /// <param name="spriteBatch">The sprite batch</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // If the widget is invisible, don't render it nor it's children
            if (_visible)
            {
                // Draw the borders
                if (_withBorders)
                    DrawBorders(gameTime, spriteBatch, _skin.PanelBorder);

                // Draw the background
                if (_withBackground)
                    DrawBackground(gameTime, spriteBatch, _skin.PanelBackground);

                // Draw the component itself
                DrawWidget(gameTime, spriteBatch);

                // Draw the child components
                DrawChildren(gameTime, spriteBatch);
            }
        }

        /// <summary>
        /// Renders the widget background. This method ay be overridden to customise the way the background is 
        /// drawn
        /// </summary>
        /// <param name="gameTime">Elasped time since last draw</param>
        /// <param name="spriteBatch">The sprite batch</param>
        /// <param name="skin">The rendering skin used</param>
        protected virtual void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch, Texture2D background)
        {
            Rectangle source = new Rectangle(0, 0, background.Width, background.Height);
            Rectangle dest;
            if (Skin.PanelBorder != null)
            {
                dest = new Rectangle(
                    (int)AbsolutePosition.X + _skin.PanelBorder.TopLeft.Width,
                    (int)AbsolutePosition.Y + _skin.PanelBorder.TopLeft.Height,
                    _bounds.Width - _skin.PanelBorder.TopLeft.Width - _skin.PanelBorder.TopRight.Width,
                    _bounds.Height - _skin.PanelBorder.BottomLeft.Height - _skin.PanelBorder.BottomRight.Height
                );
            }
            else
            {
                dest = new Rectangle((int)AbsolutePosition.X, (int)AbsolutePosition.Y, _bounds.Width, _bounds.Height);
            }

            spriteBatch.Draw(background, dest, source, Color.White);
        }

        /// <summary>
        /// Renders the widget borders. This method ay be overridden to customise the way the borders are 
        /// drawn
        /// </summary>
        /// <param name="gameTime">Elasped time since last draw</param>
        /// <param name="spriteBatch">The sprite batch</param>
        /// <param name="skin">The rendering skin used</param>
        protected virtual void DrawBorders(GameTime gameTime, SpriteBatch spriteBatch, YnBorder border)
        {
            Vector2 pos = Vector2.Zero;
            Rectangle source = Rectangle.Empty;
            Rectangle dest = Rectangle.Empty;

            // Draw corners
            // Top left
            pos.X = AbsolutePosition.X;
            pos.Y = AbsolutePosition.Y;
            spriteBatch.Draw(border.TopLeft, pos, Color.White);

            // top right
            pos.X = AbsolutePosition.X + _bounds.Width - border.TopRight.Width;
            pos.Y = AbsolutePosition.Y;
            spriteBatch.Draw(border.TopRight, pos, Color.White);

            // Bottom right
            pos.X = AbsolutePosition.X + _bounds.Width - border.TopRight.Width;
            pos.Y = AbsolutePosition.Y + _bounds.Height - border.BottomRight.Height;
            spriteBatch.Draw(border.TopRight, pos, Color.White);

            // Bottom left
            pos.X = AbsolutePosition.X;
            pos.Y = AbsolutePosition.Y + _bounds.Height - border.BottomLeft.Height;
            spriteBatch.Draw(border.BottomLeft, pos, Color.White);


            // Draw borders
            // Top
            source = border.Top.Bounds;
            dest = new Rectangle(
                (int)AbsolutePosition.X + border.TopLeft.Width,
                (int)AbsolutePosition.Y,
                _bounds.Width - border.TopLeft.Width - border.TopRight.Width,
                border.Top.Height
            );
            spriteBatch.Draw(border.Top, dest, source, Color.White);

            // Right
            source = border.Right.Bounds;
            dest = new Rectangle(
                (int)AbsolutePosition.X + _bounds.Width - border.TopRight.Width,
                (int)AbsolutePosition.Y + border.TopRight.Height,
                border.Right.Width,
                _bounds.Height - border.TopRight.Height - border.BottomRight.Height
            );
            spriteBatch.Draw(border.Right, dest, source, Color.White);

            // Bottom
            source = border.Bottom.Bounds;
            dest = new Rectangle(
                (int)AbsolutePosition.X + border.BottomLeft.Width,
                (int)AbsolutePosition.Y + _bounds.Height - border.Bottom.Height,
                _bounds.Width - border.BottomLeft.Width - border.BottomRight.Width,
                border.Top.Height
            );
            spriteBatch.Draw(border.Top, dest, source, Color.White);

            // Left
            source = border.Left.Bounds;
            dest = new Rectangle(
                (int)AbsolutePosition.X,
                (int)AbsolutePosition.Y + border.TopLeft.Height,
                border.Left.Width,
                _bounds.Height - border.TopLeft.Height - border.BottomLeft.Height
            );
            spriteBatch.Draw(border.Right, dest, source, Color.White);
        }

        /// <summary>
        /// Renders the widget children. This method ay be overridden to customise the way the children
        /// are rendered
        /// </summary>
        /// <param name="gameTime">Elasped time since last draw</param>
        /// <param name="spriteBatch">The sprite batch</param>
        /// <param name="skin">The rendering skin used</param>
        protected virtual void DrawChildren(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_children.Count > 0)
            {
                foreach (YnWidget child in _children)
                {
                    child.Draw(gameTime, spriteBatch);
                }
            }
        }

        /// <summary>
        /// Renders the widget itself. This method must be overriden in subclasses to defind specific
        /// rendering
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        protected abstract void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch);

        #endregion

        #region Other methods

        protected virtual void UpdateBounds()
        {
            // TODO Handle padding settings here
            InnerBounds = _bounds;
        }

        /// <summary>
        /// Show or hide the widget and all it's children
        /// </summary>
        /// <param name="show"></param>
        public void Show(bool show)
        {
            // Hide / show the widget
            _visible = show;

            // Hide / show children
            foreach (YnWidget child in _children)
            {
                child.Show(show);
            }
        }

        /// <summary>
        /// Resize the widget according to it's children / orientation
        /// </summary>
        public virtual void Layout()
        {
            // Resize children first and store total width / height
            int totalWidth = 0;
            int totalHeight = 0;
            foreach (YnWidget child in _children)
            {
                child.Layout();
                child.Position = Vector2.Zero;

                // Handle positions
                switch (_orientation)
                {
                    case YnOrientation.Horizontal:
                        child.Move(totalWidth + _padding, _padding);
                        totalWidth += child.Bounds.Width + _padding;
                        break;
                    case YnOrientation.Vertical:
                        child.Move(Padding, totalHeight + _padding);
                        totalHeight += child.Bounds.Height + _padding;
                        break;
                }

            }
            totalWidth += Padding;
            totalHeight += Padding;

            // The resize is done according to the children sizes
            switch (_orientation)
            {
                case YnOrientation.Horizontal:
                    // Resize the widget if needed
                    if (_bounds.Width < totalWidth || Pack)
                    {
                        _bounds.Width = totalWidth;
                    }

                    if (_pack)
                    {
                        // Pack the height as well
                        _bounds.Height = GetMaxChildHeight() + _padding * 2;
                    }
                    UpdateBounds();
                    
                    break;
                case YnOrientation.Vertical:
                    // Resize the widget if needed
                    if (_bounds.Height < totalHeight || Pack)
                    {
                        _bounds.Height = totalHeight;
                    }

                    if (_pack)
                    {
                        // Pack the width as well
                        _bounds.Width = GetMaxChildWidth() + _padding * 2;
                    }
                    UpdateBounds();
                    
                    break;
            }

            // New that the layout is done, we can adjust sizes
            if (AdjustChildren)
            {
                int maxHeight = GetMaxChildHeight();
                int maxWidth = GetMaxChildWidth();
                foreach (YnWidget child in Children)
                {
                    switch (_orientation)
                    {
                        case YnOrientation.Horizontal:
                            child.Height = maxHeight - _padding * 2;
                            break;
                        case YnOrientation.Vertical:
                            child.Width = maxWidth - _padding * 2;
                            break;
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            DoCustomUpdate(gameTime);

            foreach (YnWidget child in _children)
            {
                child.Update(gameTime);
            }

            // No event handling if not visible nor active
            if (_visible && _active)
            {
                Rectangle absoluteBounds = _bounds;
                Vector2 absolutePosition = AbsolutePosition;
                absoluteBounds.X = (int)absolutePosition.X;
                absoluteBounds.Y = (int)absolutePosition.Y;
                if (absoluteBounds.Contains(YnG.Mouse.X, YnG.Mouse.Y))
                {
                    // The mouse is hovering the widget
                    DoMouseOver();

                    // There is a click handler : 2 kinds of events to handle :
                    // 1. Simple click
                    // 2. Mouse button down

                    // Part (1) : Simple click
                    bool leftClick = YnG.Mouse.JustClicked(MouseButton.Left);
                    bool middleClick = YnG.Mouse.JustClicked(MouseButton.Middle);
                    bool rightClick = YnG.Mouse.JustClicked(MouseButton.Right);
                    if (leftClick || middleClick || rightClick)
                    {
                        // A mouse button was "just clicked"
                        DoMouseClick(leftClick, middleClick, rightClick, true);

                        // TODO : Get the correct button
                        if (MouseJustClicked != null)
                            MouseJustClicked(this, new MouseClickSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y, MouseButton.Left, true, false));
                    }

                    // Part (2) : Mouse button down
                    bool leftDown = YnG.Mouse.ClickOn(MouseButton.Left, ButtonState.Pressed);
                    bool middleDown = YnG.Mouse.ClickOn(MouseButton.Middle, ButtonState.Pressed);
                    bool rightDown = YnG.Mouse.ClickOn(MouseButton.Right, ButtonState.Pressed);
                    if (leftDown || middleDown || rightDown)
                    {
                        // A mouse button is down
                        DoMouseClick(leftDown, middleDown, rightDown, false);

                        // TODO : Get the correct button
                        if (MouseClick != null)
                            MouseClick(this, new MouseClickSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y, MouseButton.Left, false, false));
                    }

                    // Mouse release
                    if (YnG.Mouse.JustReleased(MouseButton.Left) && YnG.Mouse.LastMouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (MouseReleasedInside != null) 
                            MouseReleasedInside(this, new MouseClickSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y, MouseButton.Left, false, false));
                    }
                }
                else
                {
                    // The mouse left the widget
                    DoMouseLeave();

                    // Mouse release
                    if (YnG.Mouse.JustReleased(MouseButton.Left) && YnG.Mouse.LastMouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (MouseReleasedOutside != null) MouseReleasedOutside(this, new MouseClickSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y, MouseButton.Left, false, false));
                    }
                }
            }
        }

        /// <summary>
        /// Performs custom Update
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void DoCustomUpdate(GameTime gameTime)
        {
            // To override if needed
        }

        /// <summary>
        /// Add a widget ad child of this widget
        /// </summary>
        /// <typeparam name="W">The widget type</typeparam>
        /// <param name="widget">The widget</param>
        /// <returns>The widget added for ease</returns>
        public W Add<W>(W widget) where W : YnWidget
        {
            _children.Add(widget);
            widget.Parent = this;
            return widget;
        }

        /// <summary>
        /// Move the widgets with the given deltats in X and Y
        /// </summary>
        /// <param name="dx">X delta</param>
        /// <param name="dy">Y delta</param>
        public void Move(int dx, int dy)
        {
            _bounds.X += dx;
            _bounds.Y += dy;
        }

        /// <summary>
        /// Initializes skin
        /// </summary>
        public void InitSkin()
        {
            foreach (YnWidget widget in _children)
            {
                if (widget.Skin == null)
                    widget.SetSkin(_skin);

                widget.InitSkin();
            }
        }

        /// <summary>
        /// Sets the used skin by the widget
        /// </summary>
        /// <param name="skin">The skin to use</param>
        public void SetSkin(YnSkin skin)
        {
            _skin = skin;
            foreach (YnWidget widget in _children)
            {
                widget.SetSkin(skin);
            }
        }

        /// <summary>
        /// Compute the max height of the widget's children
        /// </summary>
        /// <returns>The max height used</returns>
        protected virtual int GetMaxChildHeight()
        {
            int maxHeight = Bounds.Height;

            foreach (YnWidget widget in _children)
            {
                int currentHeight = widget.GetMaxChildHeight();
                if (maxHeight < currentHeight)
                {
                    maxHeight = currentHeight;
                }
            }

            return maxHeight;
        }

        /// <summary>
        /// Compute the max height of the widget's children
        /// </summary>
        /// <returns>The max height used</returns>
        protected virtual int GetMaxChildWidth()
        {
            int maxWidth = _bounds.Width;

            foreach (YnWidget widget in _children)
            {
                int currentWidth = widget.GetMaxChildWidth();
                if (maxWidth < currentWidth)
                {
                    maxWidth = currentWidth;
                }
            }

            return maxWidth;
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

        /// <summary>
        /// Triggered when a mouse button is released with the mouse on the widget
        /// </summary>
        public event EventHandler<MouseClickSpriteEventArgs> MouseReleasedInside = null;

        /// <summary>
        /// Triggered when a mouse button is released with the mouse out of the widget
        /// </summary>
        public event EventHandler<MouseClickSpriteEventArgs> MouseReleasedOutside = null;

        /// <summary>
        /// Performs the mouse over actions and send proper the event
        /// </summary>
        protected virtual void DoMouseOver()
        {
            _hovered = true;
            if (MouseOver != null) MouseOver(this, new MouseOverSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y));
        }

        /// <summary>
        /// Performs the mouse leave actions and send the proper event
        /// </summary>
        protected virtual void DoMouseLeave()
        {
            _hovered = false;
            if (MouseLeave != null) MouseLeave(this, new MouseLeaveSpriteEventArgs(YnG.Mouse.LastMouseState.X, YnG.Mouse.LastMouseState.Y, YnG.Mouse.X, YnG.Mouse.Y));
        }

        /// <summary>
        /// Performs the mouse click actions and send the proper event
        /// </summary>
        /// <param name="leftClick">left click</param>
        /// <param name="middleClick">Middle click</param>
        /// <param name="rightClick">right click</param>
        /// <param name="justClicked">just clicked or pressed</param>
        protected virtual void DoMouseClick(bool leftClick, bool middleClick, bool rightClick, bool justClicked)
        {
            if (leftClick)
            {
                if (MouseClick != null) MouseClick(this, new MouseClickSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y, MouseButton.Left, justClicked, false));
            }
            if (middleClick)
                if (MouseClick != null) MouseClick(this, new MouseClickSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y, MouseButton.Middle, justClicked, false));

            if (rightClick)
                if (MouseClick != null) MouseClick(this, new MouseClickSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y, MouseButton.Right, justClicked, false));
        }

        #endregion
    }
}
