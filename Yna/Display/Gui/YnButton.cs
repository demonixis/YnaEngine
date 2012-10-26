using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display.Effect;

namespace Yna.Display.Gui
{
    public abstract class YnButton : YnPanel
    {
        protected bool _buttonDown;

        public YnButton()
            : base()
        {
            Padding = 10;
            WithBackground = true;
            Pack = false;
        }

        public override void Layout()
        {
            base.Layout();

            if (Pack)
            {
                bounds.Width = GetMaxChildWidth();
                bounds.Height = GetMaxChildHeight();
            }
        }

        protected override void DoCustomUpdate(GameTime gameTime)
        {
            _buttonDown = false;
        }

        protected override void DoMouseOver()
        {
            base.DoMouseOver();
        }

        protected override void DoMouseLeave()
        {
            base.DoMouseLeave();
        }

        protected override void DoMouseClick(bool leftClick, bool middleClick, bool rightClick, bool justClicked)
        {
            base.DoMouseClick(leftClick, middleClick, rightClick, justClicked);

            _buttonDown = leftClick && !justClicked;
        }

        protected override void DrawBorders(GameTime gameTime, SpriteBatch spriteBatch, YnBorder border)
        {
            if (IsHovered)
                base.DrawBorders(gameTime, spriteBatch, Skin.HoveredButtonBorder);
            else
                base.DrawBorders(gameTime, spriteBatch, Skin.PanelBorder);
        }

        protected override void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch, Texture2D background)
        {
            if (_buttonDown)
                base.DrawBackground(gameTime, spriteBatch, Skin.ClickedButtonBackground);
            else if (IsHovered)
                base.DrawBackground(gameTime, spriteBatch, Skin.HoveredButtonBackground);
            else
                base.DrawBackground(gameTime, spriteBatch, Skin.ButtonBackground);
        }
    }
}
