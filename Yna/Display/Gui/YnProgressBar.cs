using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Yna.Helpers;

namespace Yna.Display.Gui
{
    /// <summary>
    /// Progress bar widget
    /// </summary>
    public class YnProgressBar : YnWidget
    {
        /// <summary>
        /// The minimum possible value
        /// </summary>
        public int MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }
        protected int _minValue;

        /// <summary>
        /// The maximum possible value
        /// </summary>
        public int MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }
        protected int _maxValue;

        /// <summary>
        /// The current progress value
        /// </summary>
        public int Value
        {
            get { return _currentValue; }
            set { _currentValue = (int) MathHelper.Clamp(value, _minValue, _maxValue); }
        }
        protected int _currentValue;

        public YnProgressBar()
            : base()
        {
            // Default use as percentages
            _minValue = 0;
            _maxValue = 100;
            _currentValue = 0;
            _withBackground = true;
            _orientation = YnOrientation.Horizontal;    
        }

        protected override void DoCustomUpdate(GameTime gameTime)
        {

        }

        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            Texture2D fg = Skin.ButtonBackground;
            Rectangle source;
            Rectangle dest;
            if(_orientation == YnOrientation.Vertical)
            {
                int height = Height * _currentValue  / _maxValue;

                source = new Rectangle(0, 0, fg.Width, fg.Height);
                dest = new Rectangle((int)AbsolutePosition.X, (int)AbsolutePosition.Y + Height - height, Width, height);
            }
            else
            {
                int width = Width * _currentValue  / _maxValue;

                source = new Rectangle(0, 0, fg.Width, fg.Height);
                dest = new Rectangle((int)AbsolutePosition.X, (int)AbsolutePosition.Y, width, Bounds.Height);
            }

            spriteBatch.Draw(fg, dest, source, Color.White);
        }

        protected override void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch, Texture2D background)
        {
            // TODO : store this texture instead of generating it each time
            Texture2D bg = GraphicsHelper.CreateTexture(Skin.DefaultTextColor, 1, 1);
            Rectangle source = new Rectangle(0, 0, bg.Width, bg.Height);
            Rectangle dest = new Rectangle((int)AbsolutePosition.X, (int)AbsolutePosition.Y, Bounds.Width, Bounds.Height);

            spriteBatch.Draw(bg, dest, source, Color.White);
        }
    }
}
