using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Yna.Framework.Helpers;

namespace Yna.Framework.Display.Gui
{
    /// <summary>
    /// Basic checkbox widget
    /// </summary>
    public class YnCheckbox : YnWidget
    {
        protected bool _checked;

        /// <summary>
        /// The current check state
        /// </summary>
        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; }
        }

        public YnCheckbox()
            : base()
        {
            _withBackground = true;
            _checked = false;
        }

        public override void Layout()
        {
            base.Layout();

            // The checkbox size is initialized with current font size
            int size = (int)_skin.Font.MeasureString("#").Y;
            Width = size;
            Height = size;
        }

        protected override void DoMouseClick(bool leftClick, bool middleClick, bool rightClick, bool justClicked)
        {
            if (justClicked && leftClick)
            {
                _checked = !_checked;
            }
        }

        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_checked)
            {
                // Draw a square with padding (Width /5)
                int padding = Width / 5;
                Texture2D fg = _skin.ButtonBackground;
                Rectangle source = new Rectangle(0, 0, fg.Width, fg.Height);
                Rectangle dest = new Rectangle((int)AbsolutePosition.X + padding, (int)AbsolutePosition.Y + padding, _bounds.Width - padding * 2, _bounds.Height - padding * 2);

                spriteBatch.Draw(fg, dest, source, Color.White);
            }

        }

        protected override void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch, Texture2D background)
        {
            Texture2D bg = YnGraphics.CreateTexture(_skin.DefaultTextColor, Width, Height);
            base.DrawBackground(gameTime, spriteBatch, bg);
        }
    }
}
