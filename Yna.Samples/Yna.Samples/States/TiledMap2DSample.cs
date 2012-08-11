using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Display.TiledMap.Basic;
using Yna.State;

namespace Yna.Sample.States
{
    /// <summary>
    /// Example of simple tiled map user and rendering
    /// </summary>
    public class TiledMap2DSample : YnState
    {
        /// <summary>
        /// This is the TiledMap component
        /// </summary>
        private TiledMap2D _map;

        /// <summary>
        /// This Vector represents the point of view on the map.
        /// </summary>
        private Vector2 _camera;

        /// <summary>
        /// This Rectangle represents the area where the map will be rendered.
        /// </summary>
        private Rectangle _viewport;

        /// <summary>
        /// This dummy texture is used to draw the zone where the map is rendered
        /// </summary>
        private Texture2D _dummyTexture;

        public TiledMap2DSample()
        {
            // A TiledMap contains layers which contains the tiles definitions.
            // Each layer is rendered on top of the previous one. There is no
            // layer limit, but use them wisely : using more layers causes more 
            // rendering and can result in lower performance with huge maps.

            // For prototyping, you can use the array declaration of a layer.
            // It's quite easy to understand and quick.
            // So first, you have to create the tile data. Each tile is be defined
            // by it's texture ID. The Texture ID is obtained by cutting the map
            // tileset from top left to bottom right. The first tile cutted has
            // the ID 0, the next on the right has the ID 1 and so on.

            // Here is a sample of a 30x20 tiled map data
            int[,] data = {
				{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,0,1,1,1,1,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,3,4,4,4,4,5,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,6,7,11,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,3,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,6,7,11,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,3,4,4,4,4,0,1,1,1,2,4,4,4,4,4,4,4,4,4,4,4,4},
				{1,1,1,1,1,1,1,1,9,4,4,4,4,3,0,1,2,5,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,3,3,4,5,5,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,3,6,7,8,5,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,6,7,7,7,8,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4}
			};

            // Here is the layer we're gonna put in the map. Note that if you
            // use multiple layers, they MUST have the same size. If you don't
            // want to use all tiles in a layer, fill it with transparent texture
            // ID.
            Layer2D layer = new Layer2D(data);

            // Here we go, hello TiledMap!
            // - First there is the tileset texture containing all tiles
            // - Then the array of layers (for this example there's just one layer)
            // - And to finish, the tile size. Note that in this example, tiles are
            // squares but you can use rectangle tiles too. Just use another constructor
            // to define tile width and tile height.
            _map = new TiledMap2D("Tilesets/tileset", new Layer2D[] { layer }, 20);

            // The map can be moved with a simple Vector2. The camera position
            // is screen relative. The top left tile will be drawn at this position
            _camera = new Vector2(64, 64);

            // This part is optionnal. You can define a drawing zone for the map :
            // a viewport. When doing this, the map will be rendered only in this zone. 
            // This Rectangle's position is screen relative.
            _viewport = new Rectangle(128, 128, 256, 256);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            // By calling LoadContent on the map, the tileset texture will be loaded
            // and tile zones will be calculated according to the tile size.
            _map.LoadContent();

            // For this example we need an empty texture for drawing the rectangle
            // around the map (ensuring that no pixel gets out of the zone! :-) )
            _dummyTexture = new Texture2D(YnG.GraphicsDevice, 1, 1);
            _dummyTexture.SetData(new Color[] { Color.White });
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();

            // Here we simply draw the map with the camera position and the draw zone
            _map.Draw(spriteBatch, _camera, _viewport);

            // A rectangle is drawn to represent the map's drawing zone
            DrawRectangle(_viewport, Color.Red);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            // Simple handling of the camera position
            if (YnG.Keys.Left) _camera.X++;
            if (YnG.Keys.Right) _camera.X--;
            if (YnG.Keys.Up) _camera.Y++;
            if (YnG.Keys.Down) _camera.Y--;

            if (YnG.Keys.JustPressed(Keys.Escape))
            {
                YnG.SwitchState(new GameMenu());
            }
        }

        private void DrawRectangle(Rectangle rectangle, Color color)
        {
            // Draw each border as a texture
            spriteBatch.Draw(_dummyTexture, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, 1), color);
            spriteBatch.Draw(_dummyTexture, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, 1), color);
            spriteBatch.Draw(_dummyTexture, new Rectangle(rectangle.Left, rectangle.Top, 1, rectangle.Height), color);
            spriteBatch.Draw(_dummyTexture, new Rectangle(rectangle.Right, rectangle.Top, 1, rectangle.Height + 1), color);
        }
    }
}
