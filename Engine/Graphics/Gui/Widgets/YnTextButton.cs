using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics.Gui.Widgets
{
    /// <summary>
    /// All possible text alignments for a YnTextButon
    /// </summary>
    public enum YnTextAlign { TopLeft, Top, TopRight, Left, Center, Right,BottomLeft, Bottom, BottomRight}

    /// <summary>
    /// Text button widget
    /// </summary>
    public class YnTextButton : YnButton
    {
        #region Attributes
        
        private YnLabel _label;
        private YnTextAlign _align;

        #endregion

        #region Properties

        /// <summary>
        /// The text alignment inside the button
        /// </summary>
        public YnTextAlign TextAlign
        {
            get { return _align; }
            set {
                _align = value;
                AlignText();
            }
        }

        /// <summary>
        /// The text displayed in the button
        /// </summary>
        public string Text
        {
            get { return _label.Text; }
            set { _label.Text = value;}
        }

        /// <summary>
        /// Overrides the skin font and use this one instead
        /// </summary>
        public SpriteFont CustomFont
        {
            set { _label.CustomFont = value; }
        }

        /// <summary>
        /// Overrides the default text color defined in the skin and use this one instead
        /// </summary>
        public Color TextColor
        {
            get { return _label.TextColor; }
            set { _label.TextColor = value; }
        }

        #endregion

        #region Constructors

        public YnTextButton()
            : base()
        {
            _label = Add(new YnLabel());
            _align = YnTextAlign.Center;
        }

        public YnTextButton(int width, int height, string text)
             : this()
        {
            Width = width;
            Height = height;

            _label.Text = text;
            Vector2 textSize = YnGui.GetSkin(_skinName).FontDefault.MeasureString(text);
            _label.Width = (int)textSize.X;
            _label.Height = (int)textSize.Y;
            AlignText();
        }
        #endregion

        
        public void AlignText()
        {
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
    }
}
