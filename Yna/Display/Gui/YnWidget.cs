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
    public enum YnOrientation { Horizontal, Vertical }

    /// <summary>
    /// Base class for GUI widgets
    /// </summary>
    public abstract class YnWidget
    {
        #region Properties

        /// <summary>
        /// The widget relative position within it's parent if it has one, absolute otherwise (relative to the screen).
        /// </summary>
        public Vector2 Position
        {
            get { return new Vector2(Bounds.X, Bounds.Y); }
            set
            {
                bounds.X = (int) value.X;
                bounds.Y = (int) value.Y;
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
                if(Parent != null)
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
        public bool WithBackground { get; set; }

        /// <summary>
        /// Set to true to render the default border images
        /// </summary>
        public bool WithBorder { get; set; }

        /// <summary>
        /// Set to false to stop rendering
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Set to false to stop updating
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The parent widget. If left empty, position will be considered as absolute. otherwise, 
        /// they will be considered as relative to the parent's bounds
        /// </summary>
        public YnWidget Parent { get; set; }

        /// <summary>
        /// This list contains all children contained in the widget
        /// </summary>
        public List<YnWidget> Children { get; set; }

        /// <summary>
        /// The widget bounds
        /// </summary>
        public Rectangle Bounds {
            get { return bounds; }
            set { bounds = value; }
        }
        protected Rectangle bounds;

        /// <summary>
        /// The widget inner bounds : space left for children.
        /// </summary>
        public Rectangle InnerBounds { get; set; }

        /// <summary>
        /// The children rendering orientation
        /// </summary>
        public YnOrientation Orientation { get; set; }

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
        public YnSkin Skin { get; set; }

        /// <summary>
        /// The padding between borders and inner widgets
        /// </summary>
        public int Padding { get; set; }

        /// <summary>
        /// Pack the component to be the smallest possible according to it's children sizes
        /// </summary>
        public bool Pack { get; set; }

        /// <summary>
        /// True if the widget is currently hovered by the mouse
        /// </summary>
        public bool IsHovered { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor. Initializes values
        /// </summary>
        protected YnWidget()
        {
            Position = Vector2.Zero;
            WithBackground = false;
            WithBorder = false;
            IsVisible = true;
            IsActive = true;
            Parent = null;
            IsHovered = false;
            Children = new List<YnWidget>();
            Orientation = YnOrientation.Vertical;
            Bounds = Rectangle.Empty;

        }

        #endregion

        #region Draw methods

        /// <summary>
        /// Draw the widget
        /// </summary>
        /// <param name="gameTime">Elasped time since last draw</param>
        /// <param name="spriteBatch">The sprite batch</param>
        /// <param name="skin">The rendering skin used</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            // If the widget is invisible, don't render it nor it's children
            if(IsVisible)
            {
                // Draw the borders
                if (WithBorder)
                    DrawBorders(gameTime, spriteBatch, skin.BoxBorder);

                // Draw the background
                if(WithBackground)
                    DrawBackground(gameTime, spriteBatch, skin.BoxBackground);

                // Draw the component itself
                DrawWidget(gameTime, spriteBatch, skin);

                // Draw the child components
                DrawChildren(gameTime, spriteBatch, skin);
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
            Rectangle dest = new Rectangle(
                (int) AbsolutePosition.X + Skin.BoxBorder.TopLeft.Width,
                (int) AbsolutePosition.Y + Skin.BoxBorder.TopLeft.Height,
                Bounds.Width - Skin.BoxBorder.TopLeft.Width - Skin.BoxBorder.TopRight.Width,
                Bounds.Height - Skin.BoxBorder.BottomLeft.Height - Skin.BoxBorder.BottomRight.Height
            );

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
            pos.X = AbsolutePosition.X + Bounds.Width - border.TopRight.Width;
            pos.Y = AbsolutePosition.Y;
            spriteBatch.Draw(border.TopRight, pos, Color.White);

            // Bottom right
            pos.X = AbsolutePosition.X + Bounds.Width - border.TopRight.Width;
            pos.Y = AbsolutePosition.Y + Bounds.Height - border.BottomRight.Height;
            spriteBatch.Draw(border.TopRight, pos, Color.White);

            // Bottom left
            pos.X = AbsolutePosition.X;
            pos.Y = AbsolutePosition.Y + Bounds.Height - border.BottomLeft.Height;
            spriteBatch.Draw(border.BottomLeft, pos, Color.White);


            // Draw borders
            // Top
            source = border.Top.Bounds;
            dest = new Rectangle(
                (int) AbsolutePosition.X + border.TopLeft.Width,
                (int) AbsolutePosition.Y,
                Bounds.Width - border.TopLeft.Width - border.TopRight.Width,
                border.Top.Height
            );
            spriteBatch.Draw(border.Top, dest, source, Color.White);

            // Right
            source = border.Right.Bounds;
            dest = new Rectangle(
                (int) AbsolutePosition.X + Bounds.Width - border.TopRight.Width,
                (int) AbsolutePosition.Y + border.TopRight.Height,
                border.Right.Width,
                Bounds.Height - border.TopRight.Height - border.BottomRight.Height
            );
            spriteBatch.Draw(border.Right, dest, source, Color.White);

            // Bottom
            source = border.Bottom.Bounds;
            dest = new Rectangle(
                (int) AbsolutePosition.X + border.BottomLeft.Width,
                (int) AbsolutePosition.Y + Bounds.Height - border.Bottom.Height,
                Bounds.Width - border.BottomLeft.Width - border.BottomRight.Width,
                border.Top.Height
            );
            spriteBatch.Draw(border.Top, dest, source, Color.White);

            // Left
            source = border.Left.Bounds;
            dest = new Rectangle(
                (int) AbsolutePosition.X,
                (int) AbsolutePosition.Y + border.TopLeft.Height,
                border.Left.Width,
                Bounds.Height - border.TopLeft.Height - border.BottomLeft.Height
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
        protected virtual void DrawChildren(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            if(Children.Count > 0)
            {
                foreach(YnWidget child in Children)
                {
                    child.Draw(gameTime, spriteBatch, skin);
                }
            }
        }

        /// <summary>
        /// Renders the widget itself. This method must be overriden in subclasses to defind specific
        /// rendering
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="skin"></param>
        protected abstract void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin);

        #endregion

        #region Other methods

        protected virtual void UpdateBounds()
        {
            // TODO Handle padding settings here
            InnerBounds = Bounds;
        }

        /// <summary>
        /// Resize the widget according to it's children / orientation
        /// </summary>
        public virtual void Layout()
        {
            // Resize children first and store total width / height
            int totalWidth = 0;
            int totalHeight = 0;
            foreach (YnWidget child in Children)
            {
                child.Layout();

                // Handle positions
                switch (Orientation)
                {
                    case YnOrientation.Horizontal:
                        child.Move(totalWidth + Padding, Padding);
                        totalWidth += child.Bounds.Width + Padding;
                        break;
                    case YnOrientation.Vertical:
                        child.Move(Padding, totalHeight + Padding);
                        totalHeight += child.Bounds.Height + Padding;
                        break;
                }

            }
            totalWidth += Padding;
            totalHeight += Padding;

            // The resize is done according to the children sizes
            switch(Orientation)
            {
                case YnOrientation.Horizontal :
                    // Horizontal layout : left to right
                    if (HorizontalOverflow)
                    {
                        // Don't modify the widget size but add scroll bars id needed
                        // TODO
                    }
                    else
                    {
                        // Resize the widget if needed
                        if (bounds.Width < totalWidth || Pack)
                        {
                            bounds.Width = totalWidth;
                        }

                        if (Pack)
                        {
                            // Pack the height as well
                            bounds.Height = GetMaxChildHeight() + Padding * 2;
                        }
                        UpdateBounds();
                    }
                    break;
                case YnOrientation.Vertical :
                    // Vertical layout : top to bottom
                    if (VerticalOverflow)
                    {
                        // Don't modify the widget size but add scroll bars id needed
                        // TODO
                    }
                    else
                    {
                        // Resize the widget if needed
                        if (bounds.Height < totalHeight || Pack)
                        {
                            bounds.Height = totalHeight;
                        }

                        if(Pack)
                        {
                            // Pack the width as well
                            bounds.Width = GetMaxChildWidth() + Padding * 2;
                        }
                        UpdateBounds();
                    }
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (YnWidget child in Children)
            {
                child.Update(gameTime);
            }

            // No event handling if not visible nor active
            if(IsVisible && IsActive)
            {
                Rectangle absoluteBounds = bounds;
                Vector2 absolutePosition = AbsolutePosition;
                absoluteBounds.X = (int) absolutePosition.X;
                absoluteBounds.Y = (int) absolutePosition.Y;
                if (absoluteBounds.Contains(YnG.Mouse.X, YnG.Mouse.Y))
                {
                    // The mouse is hovering the widget
                    DoMouseOver();

                    // No need to perform tests if there is no click handler
                    if (MouseClick != null)
                    {
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
                        }

                        // Part (2) : Mouse button down
                        bool leftDown = YnG.Mouse.ClickOn(MouseButton.Left, ButtonState.Pressed);
                        bool middleDown = YnG.Mouse.ClickOn(MouseButton.Middle, ButtonState.Pressed);
                        bool rightDown = YnG.Mouse.ClickOn(MouseButton.Right, ButtonState.Pressed);
                        if (leftDown || middleDown || rightDown)
                        {
                            // A mouse button is down
                            DoMouseClick(leftClick, middleClick, rightClick, false);
                        }
                    }
                }
                else
                {
                    // The mouse left the widget
                    DoMouseLeave();
                }
            }
        }

        /// <summary>
        /// Add a widget ad child of this widget
        /// </summary>
        /// <typeparam name="W">The widget type</typeparam>
        /// <param name="widget">The widget</param>
        /// <returns>The widget added for ease</returns>
        public W Add<W>(W widget) where W : YnWidget
        {
            Children.Add(widget);
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
            bounds.X += dx;
            bounds.Y += dy;
        }

        /// <summary>
        /// Sets the used skin by the widget
        /// </summary>
        /// <param name="skin">The skin to use</param>
        public void InitSkin(YnSkin skin)
        {
            Skin = skin;
            foreach(YnWidget widget in Children)
            {
                widget.InitSkin(skin);
            }
        }

        /// <summary>
        /// Compute the max height of the widget's children
        /// </summary>
        /// <returns>The max height used</returns>
        protected virtual int GetMaxChildHeight()
        {
            int maxHeight = Bounds.Height;

            foreach (YnWidget widget in Children)
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
            int maxWidth = Bounds.Width;

            foreach (YnWidget widget in Children)
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
        /// Performs the mouse over actions and send proper the event
        /// </summary>
        protected virtual void DoMouseOver()
        {
            IsHovered = true;
            if (MouseOver != null) MouseOver(this, new MouseOverSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y));
        }

        /// <summary>
        /// Performs the mouse leave actions and send the proper event
        /// </summary>
        protected virtual void DoMouseLeave()
        {
            IsHovered = false;
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
                MouseClick(this, new MouseClickSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y, MouseButton.Left, justClicked));

            if (middleClick)
                MouseClick(this, new MouseClickSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y, MouseButton.Middle, justClicked));

            if (rightClick)
                MouseClick(this, new MouseClickSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y, MouseButton.Right, justClicked));
        }

        #endregion
    }
}
