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
    /// Progress bar widget. Can be used horizontally (left to right) or vertically (bottom to top)
    /// </summary>
    public class YnProgressBar : YnWidget
    {
        #region Attributes

        protected int _minValue;
        protected int _maxValue;
        protected int _currentValue;

        #endregion

        /// <summary>
        /// The minimum possible value
        /// </summary>
        public int MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }

        /// <summary>
        /// The maximum possible value
        /// </summary>
        public int MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }

        /// <summary>
        /// The current progress value
        /// </summary>
        public int Value
        {
            get { return _currentValue; }
            set { _currentValue = (int)MathHelper.Clamp(value, _minValue, _maxValue); }
        }

        public YnProgressBar()
            : base()
        {
            // Default use as percentages
            _minValue = 0;
            _maxValue = 100;
            _currentValue = 0;
            _hasBackground = true;
        }

        protected override void DoCustomUpdate(GameTime gameTime)
        {

        }

        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
        	/*
            Texture2D fg = _skin.ButtonBackground;
            Rectangle source;
            Rectangle dest;
            if (_maxValue != 0)
            {
                if (_orientation == YnOrientation.Vertical)
                {
                    int height = Height * _currentValue / _maxValue;

                    source = new Rectangle(0, 0, fg.Width, fg.Height);
                    dest = new Rectangle((int)AbsolutePosition.X, (int)AbsolutePosition.Y + Height - height, Width, height);
                }
                else
                {
                    int width = Width * _currentValue / _maxValue;

                    source = new Rectangle(0, 0, fg.Width, fg.Height);
                    dest = new Rectangle((int)AbsolutePosition.X, (int)AbsolutePosition.Y, width, _bounds.Height);
                }
                spriteBatch.Draw(fg, dest, source, Color.White);
            }
*/
        }

        protected override void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            // TODO : store this texture instead of generating it each time
            /*
            Texture2D bg = YnGraphics.CreateTexture(Skin.DefaultTextColor, 1, 1);
            Rectangle source = new Rectangle(0, 0, bg.Width, bg.Height);
            Rectangle dest = new Rectangle((int)AbsolutePosition.X, (int)AbsolutePosition.Y, Bounds.Width, Bounds.Height);

            spriteBatch.Draw(bg, dest, source, Color.White);
            */
        }
    }
}
