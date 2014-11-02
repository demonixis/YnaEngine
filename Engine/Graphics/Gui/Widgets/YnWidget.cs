// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics.Event;
using Yna.Engine.Input;
using Microsoft.Xna.Framework.Input;

namespace Yna.Engine.Graphics.Gui.Widgets
{
    /// <summary>
    /// Base class for GUI widgets
    /// </summary>
    public abstract class YnWidget : YnEntity
    {
        #region Attributes

        protected List<YnWidget> _children;
        protected string _skinName;
        protected int _padding;
		protected bool _hasBackground;
		protected bool _hasBorders;
        protected Vector2 _relativePosition;
        protected int _depth;
        protected bool _animated;
        
        #endregion

        #region Properties

        /// <summary>
        /// The parent widget. If left empty, position will be considered as absolute. otherwise, 
        /// they will be considered as relative to the parent's bounds.
        /// </summary>
        public new YnWidget Parent
        {
            get { return _parent as YnWidget; }
            set { _parent = value; }
        }

        /// <summary>
        /// This list contains all children contained in the widget.
        /// </summary>
        public List<YnWidget> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        /// <summary>
        /// The skin name.
        /// </summary>
        public string SkinName
        {
            get { return _skinName; }
            set { _skinName = value; }
        }

        /// <summary>
        /// The padding between borders and inner widgets.
        /// </summary>
        public int Padding
        {
            get { return _padding; }
            set { _padding = value; }
        }
        
        /// <summary>
        /// Set to true to render a background for this widget.
        /// </summary>
        public bool HasBackground
        { 
        	get { return _hasBackground; }
        	set { _hasBackground = value; }
        }
        
        /// <summary>
        /// Set to true to render borders for this widget.
        /// </summary>
        public bool HasBorders
        { 
        	get { return _hasBorders; }
        	set { _hasBorders = value; }
        }

        /// <summary>
        /// The widget depth in the GUI layer. This property should not be modified 
        /// manually. In order to change the depth of a widget, use methods from YnGui
        /// instead.
        /// </summary>
        public int Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor. Initializes default values.
        /// </summary>
        protected YnWidget()
            : base()
        {
            _children = new List<YnWidget>();
            Position = Vector2.Zero;
            _visible = true;
            _padding = 0;
            _hasBackground = false;
            _hasBorders = false;
            _parent = null;
            _animated = true;

            // The skin is mandatory so the default one is used
            _skinName = YnGui.DEFAULT_SKIN;

            // Depth is initialized at a negative value. This means it has no depth
            _depth = -1;

            // FIXME this tweak is horrible
            // Fake the presence of event handlers for the YnEntity to manage
            // mouse and touch routines
            _nbMouseEventObservers++;
        }

        /// <summary>
        /// Create a widget with the properties defined in the parameter.
        /// </summary>
        /// <param name="properties">The widget properties container</param>
        public YnWidget(YnWidgetProperties properties)
            : this()
        {
            SetProperties(properties);
        }

        #endregion

        #region Draw methods

        /// <summary>
        /// Gamestate pattern : Draw the widget.
        /// </summary>
        /// <param name="gameTime">Elasped time since last draw</param>
        /// <param name="spriteBatch">The sprite batch</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // If the widget is invisible, don't render it nor it's children
            if (_visible)
            {
            	// Retreive the used skin
            	YnSkin skin = YnGui.GetSkin(_skinName);
            	
                // Draw the borders if needed
                if (_hasBorders)
                    DrawBorders(gameTime, spriteBatch, skin);

                // Draw the background id needed
                if (_hasBackground)
                    DrawBackground(gameTime, spriteBatch, skin);

                // Draw the component itself
                DrawWidget(gameTime, spriteBatch, skin);

                // Draw the child components
                DrawChildren(gameTime, spriteBatch);
            }
        }

        /// <summary>
        /// Renders the widget background. This method may be overridden to customise the way the background is 
        /// drawn.
        /// </summary>
        /// <param name="gameTime">Elasped time since last draw</param>
        /// <param name="spriteBatch">The sprite batch</param>
        /// <param name="skin">The rendering skin used</param>
        protected virtual void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            // Get the appropriate background according to the current widget's state (if it's animated)
            Texture2D background;
            if (Clicked && _animated)
            {
                // Clicked state
                background = skin.BackgroundClicked;
            }
            else if (Hovered && _animated)
            {
                // Hovered state
                background = skin.BackgroundHover;
            }
            else
            {
                // Default state
                background = skin.BackgroundDefault;
            }

            Rectangle source = new Rectangle(0, 0, background.Width, background.Height);
            Rectangle dest = new Rectangle((int)ScreenPosition.X, (int)ScreenPosition.Y, Width, Height);

            spriteBatch.Draw(background, dest, source, Color.White);
        }

        /// <summary>
        /// Renders the widget borders. This method may be overridden to customise the way the borders are 
        /// drawn.
        /// </summary>
        /// <param name="gameTime">Elasped time since last draw</param>
        /// <param name="spriteBatch">The sprite batch</param>
        /// <param name="skin">The rendering skin used</param>
        protected virtual void DrawBorders(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
        	// Used vars
            Vector2 pos = Vector2.Zero;
            Rectangle source = Rectangle.Empty;
            Rectangle dest = Rectangle.Empty;
            YnBorder border;

            // Get the appropriate border definition according to the widget's state (if it's animated)
            if (Clicked && _animated)
            {
                // Clicked state
                border = skin.BorderClicked;
            }
            else if (Hovered && _animated)
            {
                // Hovered state
                border = skin.BorderHover;
            }
            else
            {
                // Default state
                border = skin.BorderDefault;
            }

            // Draw corners
            // Top left
            pos.X = ScreenPosition.X - border.TopLeft.Width;
            pos.Y = ScreenPosition.Y - border.TopLeft.Height;
            spriteBatch.Draw(border.TopLeft, pos, Color.White);

            // top right
            pos.X = ScreenPosition.X + Width;
            pos.Y = ScreenPosition.Y - border.TopRight.Height;
            spriteBatch.Draw(border.TopRight, pos, Color.White);

            // Bottom right
            pos.X = ScreenPosition.X + Width;
            pos.Y = ScreenPosition.Y + Height;
            spriteBatch.Draw(border.BottomRight, pos, Color.White);

            // Bottom left
            pos.X = ScreenPosition.X - border.BottomLeft.Width;
            pos.Y = ScreenPosition.Y + Height;
            spriteBatch.Draw(border.BottomLeft, pos, Color.White);

            
            // Draw borders
            // Top
            source = border.Top.Bounds;
            dest = new Rectangle(
                (int)ScreenPosition.X,
                (int)ScreenPosition.Y - border.Top.Height,
                Width,
                border.Top.Height
            );
            spriteBatch.Draw(border.Top, dest, source, Color.White);

            // Right
            source = border.Right.Bounds;
            dest = new Rectangle(
                (int)ScreenPosition.X + Width,
                (int)ScreenPosition.Y,
                border.Right.Width,
                Height
            );
            spriteBatch.Draw(border.Right, dest, source, Color.White);

            // Bottom
            source = border.Bottom.Bounds;
            dest = new Rectangle(
                (int)ScreenPosition.X,
                (int)ScreenPosition.Y + Height,
                Width,
                border.Bottom.Height
            );
            spriteBatch.Draw(border.Bottom, dest, source, Color.White);

            // Left
            source = border.Left.Bounds;
            dest = new Rectangle(
                (int)ScreenPosition.X - border.Left.Width,
                (int)ScreenPosition.Y,
                border.Left.Width,
                Height
            );
            spriteBatch.Draw(border.Left, dest, source, Color.White);
            
        }

        /// <summary>
        /// Renders the widget children. This method ay be overridden to customise the way the children
        /// are rendered.
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
        /// Renders the widget itself. This method must be overriden in subclasses to define specific
        /// rendering.
        /// </summary>
        /// <param name="gameTime">Elasped time since last draw</param>
        /// <param name="spriteBatch">The sprite batch</param>
        /// <param name="skin">The rendering skin used</param>
        protected abstract void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin);

        #endregion

        #region Other methods

        /// <summary>
        /// Initializes a bunch of common properties defined in the parameter.
        /// </summary>
        /// <param name="properties">The widget properties container</param>
        public void SetProperties(YnWidgetProperties properties)
        {
            if (properties.X != null) 
                Translate(properties.X.Value, 0);

            if (properties.Y != null) 
                Translate(0, properties.Y.Value);

            if (properties.Width != null) 
                Width = (int)properties.Width;

            if (properties.Height != null) 
                Height = (int)properties.Height;

            if (properties.SkinName != null) 
                SkinName = (string)properties.SkinName;

            if (properties.HasBorders != null) 
                HasBorders = (bool)properties.HasBorders;

            if (properties.HasBackground != null) 
                HasBackground = (bool)properties.HasBackground;

            if (properties.Padding != null) 
                Padding = (int)properties.Padding;

        }

        /// <summary>
        /// Show the widget.
        /// </summary>
        public void Show()
        {
            //Show the widget
            _visible = true;
        }

        /// <summary>
        /// Hide the widget.
        /// </summary>
        public void Hide()
        {
            _visible = false;
        }
        
        /// <summary>
        /// Gamestate pattern : update
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Update(GameTime gameTime)
        {
            // Perform user input handling in parent
        	base.Update(gameTime);
        	
            // Update children
            foreach (YnWidget child in _children)
            {
                child.Update(gameTime);
            }

        	// Do custom widget update
            DoCustomUpdate(gameTime);
        }

        /// <summary>
        /// Performs a custom Update. This method may be overidden in subclass to do specific
        /// update. Do not override the Update method.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        protected virtual void DoCustomUpdate(GameTime gameTime)
        {
            // To override if needed
        }

        /// <summary>
        /// Apply the current skin to the widget. Bounds will be recomputed according to the
        /// skin definition.
        /// </summary>
        public void ApplySkin()
        {
            ApplySkin(GetSkin());
        }

        /// <summary>
        /// Performs skin related computing on the widget bounds.
        /// </summary>
        /// <param name="skin">The new skin</param>
        protected virtual void ApplySkin(YnSkin skin)
        {
        	// Override if needed
        }
        
        /// <summary>
        /// Add a widget as child of this widget.
        /// </summary>
        /// <typeparam name="W">The widget type</typeparam>
        /// <param name="widget">The widget</param>
        /// <returns>The widget added for ease</returns>
        public W Add<W>(W widget) where W : YnWidget
        {
            // Initializes the widget added
            widget.ApplySkin();

            _children.Add(widget);
            widget.Parent = this;

            // If the added widget's skin is not set, then use it's new parent one
            if (widget.SkinName == null)
            {
            	widget.SkinName = _skinName;
            }

            return widget;
        }

        /// <summary>
        /// Ease method to retreive the widget's current skin.
        /// </summary>
        /// <returns>The skin</returns>
        protected YnSkin GetSkin()
        {
            return YnGui.GetSkin(_skinName);
        }

        #endregion
    }
}
