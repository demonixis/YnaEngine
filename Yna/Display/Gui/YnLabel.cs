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
        /// Default constructor
        /// </summary>
        public YnLabel() : base() {
            Text = "";
            UseCustomColor = false;
        }

        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            Color color = (UseCustomColor) ? TextColor : skin.DefaultTextColor;

            spriteBatch.DrawString(skin.Font, Text, Position, color);
        }
    }
}
