using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display
{
    /// <summary>
    /// Represent a basic image
    /// </summary>
    public class YnImage : YnObject
    {
        public YnImage(string imageAsset)
        {
            _textureName = imageAsset;
            _textureLoaded = false;
        }

        public YnImage(string imageAsset, int x, int y)
            : this (imageAsset)
        {
            _position = new Vector2(x, y);
        }

        public override void Initialize()
        {
            Rectangle = new Rectangle(X, Y, _texture.Width, _texture.Height);
        }

        public override void UnloadContent()
        {

        }

        public override void LoadContent()
        {
            if (!_textureLoaded)
            {
                if (_textureName != String.Empty)
                    _texture = YnG.Content.Load<Texture2D>(_textureName);
                else
                    throw new Exception("[YnImage] The texture name is empty");
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, _color * _alpha);
        }
    }
}
