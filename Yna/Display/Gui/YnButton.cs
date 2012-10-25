using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display.Gui
{
    public abstract class YnButton : YnPanel
    {

        public YnButton()
            : base()
        {
            Padding = 3;
            
        }

        public override void Layout()
        {
            base.Layout();

            bounds.Width = GetMaxChildWidth();
            bounds.Height = GetMaxChildHeight();
        }

        protected override void DoMouseOver()
        {
            base.DoMouseOver();
        }

        protected override void DrawBorders(GameTime gameTime, SpriteBatch spriteBatch, YnBorder border)
        {
            if (IsHovered)
                base.DrawBorders(gameTime, spriteBatch, Skin.HoveredBoxBorder);
            else
                base.DrawBorders(gameTime, spriteBatch, Skin.BoxBorder);
        }

        protected override void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch, Texture2D background)
        {
             if (IsHovered)
                base.DrawBackground(gameTime, spriteBatch, Skin.HoveredBackground);
            else
                base.DrawBackground(gameTime, spriteBatch, Skin.BoxBackground);
        }
    }
}
