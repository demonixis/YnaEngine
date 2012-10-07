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
        public new Vector2 Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;

                if (_textureLoaded)
                    Rectangle = new Rectangle(X, Y, (int)(_texture.Width * _scale.X), (int)(_texture.Height * _scale.Y));
            }
        }

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
            
        }

        public override void UnloadContent()
        {

        }

        public override void LoadContent()
        {
            if (!_textureLoaded)
            {
                if (_textureName != String.Empty)
                {
                    _texture = YnG.Content.Load<Texture2D>(_textureName);
                    Rectangle = new Rectangle(X, Y, (int)(_texture.Width * _scale.X), (int)(_texture.Height * _scale.Y));
                }
                else
                    throw new Exception("[YnImage] The texture name is empty");
            }
        }

        public void SetFullScreen()
        {
            Rectangle = new Rectangle(0, 0, YnG.Width, YnG.Height);
        }
    }
}
