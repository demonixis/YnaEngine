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
        #region Protected declarations

        protected string _text;
        protected Color _textColor;
        protected bool _useCustomColor;

        #endregion

        #region Properties

        /// <summary>
        /// The displayed text
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// The text color. If left empty, the default text color of the skin will be used
        /// </summary>
        public Color TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        /// <summary>
        /// Set to true to use a custom color
        /// </summary>
        public bool UseCustomColor 
        {
            get { return _useCustomColor; }
            set { _useCustomColor = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public YnLabel()
            : base()
        {
            _text = "";
            _useCustomColor = false;
        }

        public YnLabel(string text)
        {
            _text = text;
            _useCustomColor = false;
        }

        public YnLabel(string text, Color textColor)
        {
            _text = text;
            _textColor = textColor;
            _useCustomColor = true;
        }

        #endregion

        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            Color color = (_useCustomColor) ? _textColor : skin.DefaultTextColor;

            spriteBatch.DrawString(skin.Font, _text, AbsolutePosition, color);
        }

        public override void Layout()
        {
            base.Layout();

            Vector2 size = Skin.Font.MeasureString(Text);
            bounds.Width = (int)size.X;
            bounds.Height = (int)size.Y;
        }
    }
}
