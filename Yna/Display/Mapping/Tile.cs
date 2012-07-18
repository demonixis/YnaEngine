using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Helpers;

namespace Yna.Display.Mapping
{
    public class Tile : YnObject
    {        
		public SpriteEffects Effects { get; set; }
		public float LayerDepth { get; set; }

        public Tile(int x, int y) 
            : this(GraphicsHelper.CreateTexture(Color.Snow, 32, 32), x, y) 
        {
            _texture = GraphicsHelper.CreateTexture(Color.Snow, 32, 32);
        }

        public Tile(Texture2D texture, int x, int y, int width = 32, int height = 32) : base ()
        {
            _position = new Vector2(x, y);
            _rectangle = new Rectangle(x, y, width, height);
            _texture = texture;
			_textureName = texture.Name;
			_textureLoaded = true;
            
			Effects = SpriteEffects.None;
			LayerDepth = 1.0f;
        }

        public Tile(Texture2D texture, Rectangle rectangle)
            : this(texture, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height) 
        {
            
        }
		
		public Tile (string textureName, int x, int y, int width = 32, int height = 32) : base ()
		{
			_position = new Vector2(x, y);
			_textureName = textureName;
			LoadContent();
			
			Effects = SpriteEffects.None;
			LayerDepth = 1.0f;
		}
		
		public Tile (string textureName, Rectangle rectangle)
			: this (textureName, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height) { }
		
		public override void Initialize()
		{
			
		}
		
        public override void LoadContent()
        {
            if (!_textureLoaded)
            {
                YnG.Content.Load<Texture2D>(_textureName);
                _rectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
				_textureLoaded = true;
            }
        }
		
		public override void Update(GameTime gameTime)
		{
			
		}
		
		public override void UnloadContent()
		{
			
		}
		
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_texture, _position, null, _color, _rotation, _origin, _scale, Effects, LayerDepth);
		}
    }
}
