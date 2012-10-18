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
        /// The widget position within it's parent if it has one, absolute otherwise
        /// </summary>
        public Vector2 Position { get; set; }

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
        public Rectangle Bounds { get; set; }

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
            // TODO
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
            // TODO
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
                totalWidth += child.Bounds.Width;
                totalHeight += child.Bounds.Height;
            }

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
                        // Resize the widget
                        Rectangle newBounds = Bounds;
                        newBounds.Width = totalWidth;
                        Bounds = newBounds;
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
                        // Resize the widget
                        Rectangle newBounds = Bounds;
                        newBounds.Height = totalHeight;
                        Bounds = newBounds;
                        UpdateBounds();
                    }
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            // TODO Handle events here
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
