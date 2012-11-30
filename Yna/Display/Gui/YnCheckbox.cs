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
    /// Basic checkbox widget
    /// </summary>
    public class YnCheckbox : YnWidget
    {
        /// <summary>
        /// The current check state
        /// </summary>
        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; }
        }
        private bool _checked;

        public YnCheckbox()
            : base()
        {
            // TODO Handle size according to default text size
            Width = 20;
            Height = 20;
            WithBackground = true;
            _checked = false;
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
                Texture2D fg = Skin.ButtonBackground;
                Rectangle source = new Rectangle(0, 0, fg.Width, fg.Height);
                Rectangle dest = new Rectangle((int)AbsolutePosition.X + padding, (int)AbsolutePosition.Y + padding, Bounds.Width - padding * 2, Bounds.Height - padding * 2);

                spriteBatch.Draw(fg, dest, source, Color.White);
            }

        }

        protected override void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch, Texture2D background)
        {
            Texture2D bg = GraphicsHelper.CreateTexture(Skin.DefaultTextColor, Width, Height);
            base.DrawBackground(gameTime, spriteBatch, bg);
        }
    }
}
