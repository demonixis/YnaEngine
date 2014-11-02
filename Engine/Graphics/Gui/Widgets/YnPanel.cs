// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics.Gui.Widgets
{
    /// <summary>
    /// Define an orientation display for the children of a YnPanel.
    /// </summary>
    public enum YnOrientation
    {
        Horizontal, Vertical
    }

    /// <summary>
    /// Container widget. Useful for multiple widget placement like a menu.
    /// Because this class is only a container or some kind of layout manager,
    /// there is no constructor with a YnWidgetProperties. 
    /// </summary>
    public class YnPanel : YnWidget
    {
        #region Attributes

        protected YnOrientation _orientation;

        #endregion

        #region Properties

        /// <summary>
        /// The panel orientation.
        /// </summary>
        public YnOrientation Orientation
        {
            get { return _orientation; }
            set { _orientation = value;}
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public YnPanel()
            : base()
        {
            // Default orientation
            _orientation = YnOrientation.Horizontal;

            // Add a little padding for children
            _padding = 5;

            // A panel is not animated on mouse / touch over nor click / tap
            _animated = false;
        }

        /// <summary>
        /// Constructor with an orientation.
        /// </summary>
        /// <param name="orientation">The panel orientation</param>
        public YnPanel(YnOrientation orientation)
            : this()
        {
            _orientation = orientation;
        }

        /// <summary>
        /// Constructor with a YnWidgetProperties.
        /// </summary>
        /// <param name="properties">The properties</param>
        public YnPanel(YnWidgetProperties properties)
            : this()
        {
            SetProperties(properties);
        }

        #endregion

        /// <summary>
        /// As YnPanels are juste containers, this method does nothing.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The sprite batch</param>
        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            // Nothing here, just draw the children in super class
        }

        /// <summary>
        /// Compute children relative coordinates. This method must be called after all children
        /// were added to the panel. If new widgets are added afterwards, you will have to call
        /// this method again to recompute the layout. Otherwise, newly added widgets will not 
        /// have proper coordinates.
        /// </summary>
        public void Layout()
        {
            int delta = 0;
            int totalWidth = _padding;
            int totalHeight = _padding;;
            for (int i = 0; i < _children.Count; i++)
            {
                if(_orientation == YnOrientation.Horizontal)
                {
                    // Horizontal layout
                    _children[i].Move(delta, _children[i].Y);
                    totalWidth += _children[i].Width + _padding;
                    delta += _children[i].Width + _padding;
                }
                else
                {
                    // Vertical layout
                    _children[i].Move(_children[i].X, delta);
                    totalHeight += _children[i].Height + _padding;
                    delta += _children[i].Height + _padding;
                }
            }

            // Adjust the panel width and height according to it's children
            Width = totalWidth;
            Height = totalHeight;
        }
    }
}
