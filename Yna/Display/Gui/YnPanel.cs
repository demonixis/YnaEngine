using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Yna.Display.Gui
{
    public class YnPanel : YnWidget
    {
        public YnPanel()
            : base()
        {
            WithBackground = true;
            WithBorder = true;
            Pack = true;
        }

        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            // Nothing here, just draw the children in super class
        }
    }
}
