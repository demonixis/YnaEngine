using System;
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
        private YnLabel _label;
        private YnTextAlign _align;

        public YnTextAlign TextAlign
        {
            get { return _align; }
            set { _align = value; }
        }

        public string Text
        {
            get { return _label.Text; }
            set { _label.Text = value;}
        }

        public SpriteFont CustomFont
        {
            set { _label.CustomFont = value; }
        }

        public Color TextColor
        {
            get { return _label.TextColor; }
            set { _label.TextColor = value; }
        }

        #region Constructors

        public YnTextButton()
            : base()
        {
            _label = Add(new YnLabel());
            _align = YnTextAlign.Center;
            Pack = true;
        }

        public YnTextButton(string text)
            : this()
        {
            _label.Text = text;
        }

        public YnTextButton(string text, int width, int height)
            : this(text)
        {
            Width = width;
            Height = height;
        }

        public YnTextButton(string text, int width, int height, bool pack)
            : this(text, width, height)
        {
            Pack = pack;
        }

        #endregion

        public override void Layout()
        {
            base.Layout();
            _label.TextColor = Skin.ClickedButtonTextColor;

            // Align the text in the widget
            int width = _label.Width;
            int height = _label.Height;

            Vector2 pos = Vector2.Zero;
            switch (_align)
            {
                case YnTextAlign.TopLeft:
                    pos = Vector2.Zero;
                    break;
                case YnTextAlign.Top:
                    pos = new Vector2(Width / 2 - width / 2, 0);
                    break;
                case YnTextAlign.TopRight:
                    pos = new Vector2(Width - width, 0);
                    break;
                case YnTextAlign.Left:
                    pos = new Vector2(0, Height / 2 - height / 2);
                    break;
                case YnTextAlign.Center:
                    pos = new Vector2(Width/2 - width/2, Height/2 - height/2);
                    break;
                case YnTextAlign.Right:
                    pos = new Vector2(Width - width, Height / 2 - height / 2);
                    break;
                case YnTextAlign.BottomLeft:
                    pos = new Vector2(0, Height - height);
                    break;
                case YnTextAlign.Bottom:
                    pos = new Vector2(Width / 2 - width / 2, Height - height);
                    break;
                case YnTextAlign.BottomRight:
                    pos = new Vector2(Width - width, Height - height);
                    break;
            }
            _label.Position = pos;
        }

        protected override void DoCustomUpdate(GameTime gameTime)
        {
            if(_buttonDown) _label.UseCustomColor = true;
            else _label.UseCustomColor = false;

            base.DoCustomUpdate(gameTime);
        }

        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            base.DrawWidget(gameTime, spriteBatch, skin);
        }
    }
}
