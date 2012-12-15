using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position">The text position on screen</param>
        /// <param name="text">The text to display</param>
        private YnText(Vector2 position, string text)
            : base()
        {
            _text = text;
            _position = position;
            _textureLoaded = false;
            _color = Color.Black;
        }

        /// <summary>
        /// Constructor. Text position at [0,0] on screen (top left)
        /// </summary>
        /// <param name="fontName">The font name</param>
        /// <param name="text">The text</param>
        public YnText(string fontName, string text)
            : this(Vector2.Zero, text)
        {
            _textureName = fontName;
            _textureLoaded = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="font">The font</param>
        /// <param name="x">X coordinate on screen</param>
        /// <param name="y">Y coordinate on screen</param>
        /// <param name="text">The text</param>
        public YnText(SpriteFont font, int x, int y, string text)
            : this(new Vector2(x, y), text)
        {
            _font = font;
            _textureLoaded = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fontName">The font name</param>
        /// <param name="x">X coordinate on screen</param>
        /// <param name="y">Y coordinate on screen</param>
        /// <param name="text">The text</param>
        public YnText(string fontName, int x, int y, string text)
            : this(new Vector2(x, y), text)
        {
            _textureName = fontName;
            _textureLoaded = false;
        }

        #endregion

        /// <summary>
        /// Center the text on the given position according to the text's width/height
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public void CenterRelativeTo(int x, int y)
        {
            if (_textureLoaded)
            {
                Position = new Vector2(
                    (float)(x / 2 - Width / 2),
                    (float)(y / 2 - Height / 2)
                );
            }
        }

        #region GameState pattern

        public override void Initialize()
        {
            LoadContent();
            Vector2 size = _font.MeasureString(Text);
            Rectangle = new Rectangle(X, Y, (int)size.X, (int)size.Y);
        }

        public override void LoadContent()
        {
            if (!_textureLoaded && _textureName != String.Empty)
            {
                _font = YnG.Content.Load<SpriteFont>(_textureName);
                _textureLoaded = true;

                Vector2 size = _font.MeasureString(Text);
                Width = (int)size.X;
                Height = (int)size.Y;
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
