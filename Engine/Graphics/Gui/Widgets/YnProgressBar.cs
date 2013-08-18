// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Yna.Engine.Helpers;

namespace Yna.Engine.Graphics.Gui.Widgets
{
    /// <summary>
    /// Progress bar widget. Can be used horizontally (left to right) or vertically (bottom to top).
    /// Values are positive only (uint).
    /// </summary>
    public class YnProgressBar : YnPanel
    {
        #region Attributes

        protected uint _minValue;
        protected uint _maxValue;
        protected uint _currentValue;

        #endregion

        #region Properties

        /// <summary>
        /// The minimum possible value.
        /// </summary>
        public uint MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }

        /// <summary>
        /// The maximum possible value.
        /// </summary>
        public uint MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }

        /// <summary>
        /// The current progress value.
        /// </summary>
        public uint Value
        {
            get { return _currentValue; }
            set { _currentValue = (uint)MathHelper.Clamp(value, _minValue, _maxValue); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public YnProgressBar()
            : base()
        {
            // Default use as percentages
            _minValue = 0;
            _maxValue = 100;
            _currentValue = 0;
            _hasBackground = true;
        }

        /// <summary>
        /// Constructor with a YnWidgetProperties.
        /// </summary>
        /// <param name="properties">The properties</param>
        public YnProgressBar(YnWidgetProperties properties)
        {
            SetProperties(properties);
        }

        #endregion

        /// <summary>
        /// Draw the widget.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The sprite batch</param>
        /// <param name="skin">The skin to use</param>
        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            Texture2D fg = skin.BackgroundDefault;
            Rectangle source;
            Rectangle dest;
            if (_maxValue != 0)
            {
                if (_orientation == YnOrientation.Vertical)
                {
                    int height = (int)(Height * _currentValue / _maxValue);

                    source = new Rectangle(0, 0, fg.Width, fg.Height);
                    dest = new Rectangle((int)ScreenPosition.X, (int)ScreenPosition.Y + Height - height, Width, height);
                }
                else
                {
                    int width = (int)(Width * _currentValue / _maxValue);

                    source = new Rectangle(0, 0, fg.Width, fg.Height);
                    dest = new Rectangle((int)ScreenPosition.X, (int)ScreenPosition.Y, width, Height);
                }

                // Draw the bar
                spriteBatch.Draw(fg, dest, source, Color.White);
            }
        }

        /// <summary>
        /// Draw the background.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The sprite batch</param>
        /// <param name="skin">The skin to use</param>
        protected override void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            // Get the clicked background as background.
            Texture2D bg = skin.BackgroundClicked;

            // Create source and destination rectangles for rendering
            Rectangle source = new Rectangle(0, 0, bg.Width, bg.Height);
            Rectangle dest = new Rectangle((int)ScreenPosition.X, (int)ScreenPosition.Y, Width, Height);

            // Render
            spriteBatch.Draw(bg, dest, source, Color.White);
        }
    }
}
