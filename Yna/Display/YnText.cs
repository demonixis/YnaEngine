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
        protected float _layerDepth;
		
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

        private YnText(Vector2 position, string text)
            : base()
        {
            _text = text;
            _position = position;
            _textureLoaded = false;
            _color = Color.Black;
        }

        public YnText(SpriteFont font, Vector2 position, string text) 
            : this(position, text)
        {
            _font = font;
            _textureLoaded = true;
        }

        public YnText(string fontName, Vector2 position, string text)
            : this(position, text)
        {
            _textureName = fontName;
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
                _textureLoaded = true;
            }
        }

        public override void UnloadContent() { }

        public override void Update(GameTime gameTime) { }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, _text, _position, Color, _rotation, _origin, _scale, _effect, _layerDepth);
        }
    }
}
