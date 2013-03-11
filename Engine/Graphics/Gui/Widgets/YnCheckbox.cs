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
    /// Basic checkbox widget
    /// </summary>
    public class YnCheckbox : YnButton
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
            _hasBackground = true;
            _checked = false;
            Width = 20;
            Height = 20;
        }

        protected override void DoCustomUpdate(GameTime gameTime)
        {
            if (_clicked)
            {
                _checked = !_checked;
            }
        }

        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
        	
            if (_checked)
            {
                // Draw a square with padding (Width /5)
                int padding = Width / 5;
                Texture2D fg = GetSkin().BackgroundDefault;
                Rectangle source = new Rectangle(0, 0, fg.Width, fg.Height);
                Rectangle dest = new Rectangle((int)ScreenPosition.X + padding, (int)ScreenPosition.Y + padding, Width - padding * 2, Height - padding * 2);

                spriteBatch.Draw(fg, dest, source, Color.White);
            }

        }

        protected override void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            // Always draw the clicked background
            Texture2D background = GetSkin().BackgroundClicked;

            Rectangle source = new Rectangle(0, 0, background.Width, background.Height);
            Rectangle dest = new Rectangle((int)ScreenPosition.X, (int)ScreenPosition.Y, Width, Height);

            spriteBatch.Draw(background, dest, source, Color.White);
            
        }
    }
}
