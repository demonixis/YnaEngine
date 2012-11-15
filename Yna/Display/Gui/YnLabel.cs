using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display.Gui
{
    public class YnLabel : YnWidget
    {
        /// <summary>
        /// The displayed text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The text color. If left empty, the default text color of the skin will be used
        /// </summary>
        public Color TextColor { get; set; }

        /// <summary>
        /// Set to true to use a custom color
        /// </summary>
        public bool UseCustomColor { get; set; }

        /// <summary>
        /// A custom font may be used to display text
        /// </summary>
        public SpriteFont CustomFont
        {
            get { return _customFont; }
            set { _customFont = value; }
        }
        protected SpriteFont _customFont;

        /// <summary>
        /// Default constructor
        /// </summary>
        public YnLabel() : base() {
            Text = "";
            UseCustomColor = false;
        }

        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            Color color = (UseCustomColor) ? TextColor : skin.DefaultTextColor;
            SpriteFont font = (_customFont == null) ? skin.Font : _customFont;

            spriteBatch.DrawString(font, Text, AbsolutePosition, color);
        }

        public override void Layout()
        {
            base.Layout();

            SpriteFont font = (_customFont == null) ? Skin.Font : _customFont;

            Vector2 size = font.MeasureString(Text);
            bounds.Width = (int) size.X;
            bounds.Height = (int) size.Y;
        }
    }
}
