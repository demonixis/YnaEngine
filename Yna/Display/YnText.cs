using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display
{
    public class YnText : YnObject
    {
        protected SpriteFont _font;
        protected string _text;
        protected SpriteEffects _effect;
		
		public SpriteFont Font
		{
			get { return _font; }
			set { _font = value; }
		}

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public float LayerDepth
        {
            get { return _layerDepth; }
            set { _layerDepth = value; }
        }

        public new int Width
        {
            get { return (int)_font.MeasureString(_text).X; }
            protected set { base.Width = value; }
        }

        public new int Height
        {
            get { return (int)_font.MeasureString(_text).Y; }
            protected set { base.Height = value; }
        }

        private YnText(Vector2 position, string text)
            : base()
        {
            _text = text;
            _position = position;
            _textureLoaded = false;
            _color = Color.Black;
        }

        public YnText(string fontName, string text)
            : this(Vector2.Zero, text)
        {
            _textureName = fontName;
            _textureLoaded = false;
        }

        public YnText(SpriteFont font, int x, int y, string text)
            : this(new Vector2(x, y), text)
        {
            _font = font;
            _textureLoaded = true;
        }

        public YnText(string fontName, int x, int y, string text)
            : this(new Vector2(x, y), text)
        {
            _textureName = fontName;
            _textureLoaded = false;
        }
        
        // TODO: Deprecated
        public YnText(SpriteFont font, Vector2 position, string text) 
            : this(position, text)
        {
            _font = font;
            _textureLoaded = true;
        }

        // TODO: Deprected
        public YnText(string fontName, Vector2 position, string text)
            : this(position, text)
        {
            _textureName = fontName;
        }

        public void CenterRelativeTo(int width, int height)
        {
            if (_textureLoaded)
            {
                Position = new Vector2(
                    (float)(width / 2 - Width / 2),
                    (float)(height / 2 - Height / 2)
                    );
            }
        }

        public override void Initialize()
        {
            LoadContent();
            Vector2 size = _font.MeasureString(_text);
            Rectangle = new Rectangle(X, Y, (int)size.X, (int)size.Y);
        }

        public override void LoadContent()
        {
            if (!_textureLoaded && _textureName != String.Empty)
            {
                _font = YnG.Content.Load<SpriteFont>(_textureName);
                Vector2 size = _font.MeasureString(_text);
                Width = (int)size.X;
                Height = (int)size.Y;
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
                spriteBatch.DrawString(_font, _text, _position, Color, _rotation, _origin, _scale, _effect, _layerDepth);
        }
    }
}
