using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display.Event;

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
                    DrawBorders(gameTime, spriteBatch, skin);

                // Draw the background
                if(WithBackground)
                    DrawBackground(gameTime, spriteBatch, skin);

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
        protected virtual void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            Rectangle source = new Rectangle(0, 0, skin.BoxBackground.Width, skin.BoxBackground.Height);
            Rectangle dest = new Rectangle(
                (int) AbsolutePosition.X + skin.BoxBorder.TopLeft.Width,
                (int) AbsolutePosition.Y + skin.BoxBorder.TopLeft.Height,
                Bounds.Width - skin.BoxBorder.TopLeft.Width - skin.BoxBorder.TopRight.Width,
                Bounds.Height - skin.BoxBorder.BottomLeft.Height - skin.BoxBorder.BottomRight.Height
            );

            spriteBatch.Draw(skin.BoxBackground, dest, source, Color.White);
        }

        /// <summary>
        /// Renders the widget borders. This method ay be overridden to customise the way the borders are 
        /// drawn
        /// </summary>
        /// <param name="gameTime">Elasped time since last draw</param>
        /// <param name="spriteBatch">The sprite batch</param>
        /// <param name="skin">The rendering skin used</param>
        protected virtual void DrawBorders(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            Vector2 pos = Vector2.Zero;
            Rectangle source = Rectangle.Empty;
            Rectangle dest = Rectangle.Empty;

            // Draw corners
            // Top left
            pos.X = AbsolutePosition.X;
            pos.Y = AbsolutePosition.Y;
            spriteBatch.Draw(skin.BoxBorder.TopLeft, pos, Color.White);

            // top right
            pos.X = AbsolutePosition.X + Bounds.Width - skin.BoxBorder.TopRight.Width;
            pos.Y = AbsolutePosition.Y;
            spriteBatch.Draw(skin.BoxBorder.TopRight, pos, Color.White);

            // Bottom right
            pos.X = AbsolutePosition.X + Bounds.Width - skin.BoxBorder.TopRight.Width;
            pos.Y = AbsolutePosition.Y + Bounds.Height - skin.BoxBorder.BottomRight.Height;
            spriteBatch.Draw(skin.BoxBorder.TopRight, pos, Color.White);

            // Bottom left
            pos.X = AbsolutePosition.X;
            pos.Y = AbsolutePosition.Y + Bounds.Height - skin.BoxBorder.BottomLeft.Height;
            spriteBatch.Draw(skin.BoxBorder.BottomLeft, pos, Color.White);


            // Draw borders
            // Top
            source = skin.BoxBorder.Top.Bounds;
            dest = new Rectangle(
                (int) AbsolutePosition.X + skin.BoxBorder.TopLeft.Width,
                (int) AbsolutePosition.Y,
                Bounds.Width - skin.BoxBorder.TopLeft.Width - skin.BoxBorder.TopRight.Width,
                skin.BoxBorder.Top.Height
            );
            spriteBatch.Draw(skin.BoxBorder.Top, dest, source, Color.White);

            // Right
            source = skin.BoxBorder.Right.Bounds;
            dest = new Rectangle(
                (int) AbsolutePosition.X + Bounds.Width - skin.BoxBorder.TopRight.Width,
                (int) AbsolutePosition.Y + skin.BoxBorder.TopRight.Height,
                skin.BoxBorder.Right.Width,
                Bounds.Height - skin.BoxBorder.TopRight.Height - skin.BoxBorder.BottomRight.Height
            );
            spriteBatch.Draw(skin.BoxBorder.Right, dest, source, Color.White);

            // Bottom
            source = skin.BoxBorder.Bottom.Bounds;
            dest = new Rectangle(
                (int) AbsolutePosition.X + skin.BoxBorder.BottomLeft.Width,
                (int) AbsolutePosition.Y + Bounds.Height - skin.BoxBorder.Bottom.Height,
                Bounds.Width - skin.BoxBorder.BottomLeft.Width - skin.BoxBorder.BottomRight.Width,
                skin.BoxBorder.Top.Height
            );
            spriteBatch.Draw(skin.BoxBorder.Top, dest, source, Color.White);

            // Left
            source = skin.BoxBorder.Left.Bounds;
            dest = new Rectangle(
                (int) AbsolutePosition.X,
                (int) AbsolutePosition.Y + skin.BoxBorder.TopLeft.Height,
                skin.BoxBorder.Left.Width,
                Bounds.Height - skin.BoxBorder.TopLeft.Height - skin.BoxBorder.BottomLeft.Height
            );
            spriteBatch.Draw(skin.BoxBorder.Right, dest, source, Color.White);
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
            // TODO Handle events here
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

        #endregion
    }
}
