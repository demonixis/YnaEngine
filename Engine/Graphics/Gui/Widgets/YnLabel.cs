using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics.Gui.Widgets
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
            X = 0;
            Y = 0;
        }

        /// <summary>
        /// Create a label with the given text
        /// </summary>
        /// <param name="text">The label text</param>
        public YnLabel(string text)
            : this()
        {
            _text = text;
        }

        /// <summary>
        /// See documentation in YnWidget
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="skin"></param>
        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            // The label does not react to user input so we simply get the default text color
            Color color = (_useCustomColor) ? _textColor : skin.TextColorDefault;
            SpriteFont font = (_customFont == null) ? skin.FontDefault : _customFont;

            spriteBatch.DrawString(font, _text, ScreenPosition, color, _rotation, Vector2.Zero, _scale, SpriteEffects.None, 1.0f);
            
        }

        /// <summary>
        /// See documentation in YnWidget
        /// </summary>
        /// <param name="skin"></param>
        protected override void ApplySkin(YnSkin skin)
        {
            // If a custom font is currently in use, take it to measure the text
            SpriteFont font = (_useCustomColor && _customFont != null) ? _customFont : skin.FontDefault;

            // Measure the text to define widget's width & height
            Vector2 size = font.MeasureString(Text);
            Width = (int)( size.X * _scale.X);
            Height = (int)( size.Y * _scale.Y);
        }
        
    }
}
