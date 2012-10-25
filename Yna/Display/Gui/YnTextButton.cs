using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display.Gui
{
    public class YnTextButton : YnButton
    {
        private YnLabel label;

        public string Text
        {
            get { return label.Text; }
            set { label.Text = value;}
        }

        public YnTextButton()
            : base()
        {
            label = Add(new YnLabel());
        }

        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            base.DrawWidget(gameTime, spriteBatch, skin);
        }
    }
}
