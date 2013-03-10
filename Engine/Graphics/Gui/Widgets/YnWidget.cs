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
        
        #endregion

        #region Properties

        /// <summary>
        /// The parent widget. If left empty, position will be considered as absolute. otherwise, 
        /// they will be considered as relative to the parent's bounds
        /// </summary>
        public new YnWidget Parent
        {
            get { return _parent as YnWidget; }
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
        /// The skin name
        /// </summary>
        public string SkinName
        {
            get { return _skinName; }
            set { _skinName = value; }
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
        /// Set to true to render a background for this widget
        /// </summary>
        public bool HasBackground
        { 
        	get { return _hasBackground; }
        	set { _hasBackground = value; }
        }
        
        /// <summary>
        /// Set to true to render borders for this widget
        /// </summary>
        public bool HasBorders
        { 
        	get { return _hasBorders; }
        	set { _hasBorders = value; }
        }
        
        public Vector2 ScreenPosition
        {
        	get
        	{
        		if(_parent != null)
        		{
        			Vector2 parentPos = (_parent as YnWidget).ScreenPosition;
        			return new Vector2(
        				X + parentPos.X,
        				Y + parentPos.Y
        			);
        		}
        		return Position;
        	}
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor. Initializes values
        /// </summary>
        protected YnWidget()
        {
            _visible = true;
            _parent = null;
            _skinName = YnGui.DEFAULT_SKIN;
            _children = new List<YnWidget>();
            Position = Vector2.Zero;

            // FIXME this tweak is horrible
            // Fake the presence of a special mouse event handler
            _nbMouseEventObservers++;
        }

        #endregion

        #region Draw methods

        /// <summary>
        /// Draw the widget
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
        /// Renders the widget background. This method ay be overridden to customise the way the background is 
        /// drawn
        /// </summary>
        /// <param name="gameTime">Elasped time since last draw</param>
        /// <param name="spriteBatch">The sprite batch</param>
        /// <param name="skin">The rendering skin used</param>
        protected virtual void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            // Get the appropriate background according to the current widget state
            Texture2D background;
            if(Clicked)
            {
                // Clicked state
                background = skin.BackgroundClicked;
            }
            else if(Hovered)
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
        /// drawn
        /// </summary>
        /// <param name="gameTime">Elasped time since last draw</param>
        /// <param name="spriteBatch">The sprite batch</param>
        /// <param name="skin">The rendering skin used</param>
        protected virtual void DrawBorders(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
        	/*
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
            */
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
        /// <param name="skin"></param>
        protected abstract void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin);

        #endregion

        #region Other methods

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
        /// Gamestate pattern : update
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Update(GameTime gameTime)
        {
        	// Reset the hover flag
            _hovered = false;
            
            // Perform user input handling
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
        /// Performs custom Update
        /// </summary>
        /// <param name="gameTime">The game time</param>
        protected virtual void DoCustomUpdate(GameTime gameTime)
        {
            // To override if needed
        }

        /// <summary>
        /// Performs skin related computing on the widget bounds
        /// </summary>
        /// <param name="skin">The new skin</param>
        protected virtual void Layout(YnSkin skin)
        {
        	// Override if needed
        }
        
        /// <summary>
        /// Add a widget as child of this widget
        /// </summary>
        /// <typeparam name="W">The widget type</typeparam>
        /// <param name="widget">The widget</param>
        /// <returns>The widget added for ease</returns>
        public W Add<W>(W widget) where W : YnWidget
        {
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
        /// Move the widgets with the given deltats in X and Y
        /// </summary>
        /// <param name="dx">X delta</param>
        /// <param name="dy">Y delta</param>
        public void Move(int dx, int dy)
        {
           // _bounds.X += dx;
           // _bounds.Y += dy;
        }

        #endregion
    }
}
