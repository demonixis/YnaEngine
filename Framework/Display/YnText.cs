﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace Yna.Framework.Display
{
    /// <summary>
    /// Simple text component
    /// </summary>
    public class YnText : YnObject
    {
        #region private declaration

        private SpriteFont _font;
        private string _text;

        #endregion

        #region Properties

        /// <summary>
        /// The Sprite_font which will be used to draw the text
        /// </summary>
        protected SpriteFont Font
        {
            get { return _font; }
            set { _font = value; }
        }

        /// <summary>
        /// The text to display
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }

        }

        /// <summary>
        /// The text width according to it's font
        /// </summary>
        public new int Width
        {
            get { return (int)_font.MeasureString(Text).X; }
            protected set { base.Width = value; }
        }

        /// <summary>
        /// The text height according to it's font
        /// </summary>
        public new int Height
        {
            get { return (int)_font.MeasureString(Text).Y; }
            protected set { base.Height = value; }
        }

        #endregion

        #region Constructors

        public YnText()
            : base()
        {
            _assetName = String.Empty;
            _text = String.Empty;
            _assetLoaded = false;
            _color = Color.Black;
            _position = Vector2.Zero;
        }

        /// <summary>
        /// Constructor. Text position at [0,0] on screen (top left)
        /// </summary>
        /// <param name="fontName">The font name</param>
        /// <param name="text">The text</param>
        public YnText(string fontName, string text)
            : this()
        {
            _assetName = fontName;
            _assetLoaded = false;
            _text = text;
        }

        public YnText(string fontName, string text, Vector2 position, Color color)
            : this(fontName, text)
        {
            _position = position;
            _color = color;
        }

        #endregion

        /// <summary>
        /// Center the text on the given position according to the text's width/height
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public void CenterRelativeTo(int x, int y)
        {
            if (_assetLoaded)
            {
                Position = new Vector2(
                    (float)(x / 2 - Width / 2),
                    (float)(y / 2 - Height / 2)
                );
            }
        }

        /// <summary>
        /// Get a Wrapped text with the specified line width
        /// </summary>
        /// <param name="text">The text</param>
        /// <param name="maxLineWidth">Max line width</param>
        /// <returns>Wrapped text</returns>
        public string GetWrappedText(string text, float maxLineWidth)
        {
            if (_font == null)
                return text;

            return WrapText(_font, text, maxLineWidth);
        }

        /// <summary>
        /// Get a wrapped text with the specified line width
        /// </summary>
        /// <param name="spriteFont">SpriteFont object</param>
        /// <param name="text">The text</param>
        /// <param name="maxLineWidth">Max line width</param>
        /// <returns>Wrapped text</returns>
        public static string WrapText(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');

            StringBuilder sb = new StringBuilder();

            float lineWidth = 0.0f;
            float spaceWidth = spriteFont.MeasureString(" ").X;
            Vector2 wordSize = Vector2.Zero;

            foreach (string word in words)
            {
                wordSize = spriteFont.MeasureString(word);

                if (lineWidth + wordSize.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += wordSize.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = wordSize.X + spaceWidth;
                }
            }

            return sb.ToString();
        }

        #region GameState pattern

        public override void Initialize() { }

        public override void LoadContent()
        {
            if (!_assetLoaded && _assetName != String.Empty)
            {
                _font = YnG.Content.Load<SpriteFont>(_assetName);
                _assetLoaded = true;

                Vector2 size = _font.MeasureString(Text);

                Width = (int)size.X;
                Height = (int)size.Y;

                Rectangle = new Rectangle(X, Y, (int)size.X, (int)size.Y);
            }
        }

        public override void UnloadContent() { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                spriteBatch.DrawString(_font, _text, _position, _color, _rotation, _origin, _scale, _effects, _layerDepth);
            }
        }

        #endregion
    }
}