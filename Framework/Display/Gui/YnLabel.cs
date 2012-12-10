using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display.Gui
{
    /// <summary>
    /// Simple text label widget
    /// </summary>
    public class YnLabel : YnWidget
    {
        #region Attributes

        protected string _text;
        protected Color _textColor;
        protected bool _useCustomColor;
        protected SpriteFont _customFont;

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
            set
            {
                _textColor = value;
                _useCustomColor = true;
            }
        }

        /// <summary>
        /// Set to true to use a custom color
        /// </summary>
        public bool UseCustomColor
        {
            get { return _useCustomColor; }
            set { _useCustomColor = value; }
        }

        /// <summary>
        /// A custom font may be used to display text
        /// 
        /// </summary>
        /// 
        public SpriteFont CustomFont
        {
            get { return _customFont; }
            set { _customFont = value; }
        }

        #endregion

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
        }

        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color color = (_useCustomColor) ? _textColor : _skin.DefaultTextColor;
            SpriteFont font = (_customFont == null) ? _skin.Font : _customFont;

            spriteBatch.DrawString(font, _text, AbsolutePosition, color);
        }

        public override void Layout()
        {
            base.Layout();

            SpriteFont font = (_customFont == null) ? _skin.Font : _customFont;

            Vector2 size = font.MeasureString(Text);
            _bounds.Width = (int) size.X;
            _bounds.Height = (int) size.Y;
        }
    }
}
