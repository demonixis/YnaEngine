using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics.Gui.Widgets
{
    public enum YnOrientation
    {
        Horizontal, Vertical
    }

    /// <summary>
    /// Container widget. Useful for multiple widget placement
    /// </summary>
    public class YnPanel : YnWidget
    {
        protected YnOrientation _orientation;

        public YnOrientation Orientation
        {
            get { return _orientation; }
            set { _orientation = value;}
        }

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
        /// Does nothing. YnPanels are juste containers
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The sprite batch</param>
        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            // Nothing here, just draw the children in super class
        }

        /// <summary>
        /// Compute children relative coordinates
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
                    _children[i].X = delta;
                    totalWidth += _children[i].Width + _padding;
                    delta += _children[i].Width + _padding;
                }
                else
                {
                    // Vertical layout
                    _children[i].Y = delta;
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
