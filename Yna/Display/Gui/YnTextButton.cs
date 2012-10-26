﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display.Gui
{
    public enum YnTextAlign { TopLeft, Top, TopRight, Left, Center, Right,BottomLeft, Bottom, BottomRight}

    public class YnTextButton : YnButton
    {
        private YnLabel label;
        private YnTextAlign _align;

        public string Text
        {
            get { return label.Text; }
            set { label.Text = value;}
        }

        public YnTextButton()
            : base()
        {
            label = Add(new YnLabel());
            _align = YnTextAlign.Center;
        }

        public override void Layout()
        {
            base.Layout();
            label.TextColor = Skin.ClickedButtonTextColor;

            // Align the text in the widget
            int width = label.Width;
            int height = label.Height;

            Vector2 pos = Vector2.Zero;
            switch (_align)
            {
                case YnTextAlign.Center:
                    pos = new Vector2(
                        Width/2 - width/2,
                        Height/2 - height/2
                    );
                    break;
                // TODO
            }
            label.Position = pos;
        }

        protected override void DoCustomUpdate(GameTime gameTime)
        {
            if(_buttonDown) label.UseCustomColor = true;
            else label.UseCustomColor = false;

            base.DoCustomUpdate(gameTime);
        }

        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            base.DrawWidget(gameTime, spriteBatch, skin);
        }
    }
}
