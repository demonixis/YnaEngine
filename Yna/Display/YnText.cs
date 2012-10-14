using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display
{
	/// <summary>
	/// Simple text component
	/// </summary>
    public class YnText : YnObject
    {
    	/// <summary>
    	/// The SpriteFont which will be used to draw the text
    	/// </summary>
    	protected SpriteFont Font { get; set; }
    	
    	/// <summary>
    	/// The text to display
    	/// </summary>
    	public string Text { get; set; }
    	
    	/// <summary>
    	/// The text effects
    	/// </summary>
    	protected SpriteEffects TextEffects { get; set;}

        /// <summary>
        /// The text width according to it's font
        /// </summary>
        public new int Width
        {
            get { return (int) Font.MeasureString(Text).X; }
            protected set { base.Width = value; }
        }

        /// <summary>
        /// The text height according to it's font
        /// </summary>
        public new int Height
        {
            get { return (int)Font.MeasureString(Text).Y; }
            protected set { base.Height = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position">The text position on screen</param>
        /// <param name="text">The text to display</param>
        private YnText(Vector2 position, string text)
            : base()
        {
            Text = text;
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
            Font = font;
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
        
        #region Deprecated constructors
        // TODO: Deprecated
        public YnText(SpriteFont font, Vector2 position, string text) 
            : this(position, text)
        {
            Font = font;
            _textureLoaded = true;
        }

        // TODO: Deprected
        public YnText(string fontName, Vector2 position, string text)
            : this(position, text)
        {
            _textureName = fontName;
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
                    (float) (x / 2 - Width / 2),
                    (float) (y / 2 - Height / 2)
                );
            }
        }

        public override void Initialize()
        {
            LoadContent();
            Vector2 size = Font.MeasureString(Text);
            Rectangle = new Rectangle(X, Y, (int)size.X, (int)size.Y);
        }

        public override void LoadContent()
        {
            if (!_textureLoaded && _textureName != String.Empty)
            {
                Font = YnG.Content.Load<SpriteFont>(_textureName);
                Vector2 size = Font.MeasureString(Text);
                Width = (int) size.X;
                Height = (int) size.Y;
                _textureLoaded = true;
            }
        }

        public override void UnloadContent() { }

        public override void Update(GameTime gameTime) 
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                spriteBatch.DrawString(Font, Text, _position, Color, _rotation, _origin, _scale, TextEffects, _layerDepth);
            }
        }
    }
}
