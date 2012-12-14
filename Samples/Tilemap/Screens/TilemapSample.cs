using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna;
using Yna.Helpers;
using Yna.State;
using Yna.Display;
using Yna.Display.TileMap.Basic;
using Yna.Samples.Data;

namespace Yna.Samples.Screens
{
    public class TilemapSample : YnState
    {
        private TileMap2D map;
        private Vector2 camera;
        private Rectangle viewport;
        private Texture2D texture;

        public TilemapSample(string name)
            : base(name, 0, 0)
        {
            // A TiledMap contains layers which contains the tiles definitions.
            // Each layer is rendered on top of the previous one. There is no
            // layer limit, but use them wisely : using more layers causes more 
            // rendering and can result in lower performance with huge maps.

            // Here is the layer we're gonna put in the map. Note that if you
            // use multiple layers, they MUST have the same size. If you don't
            // want to use all tiles in a layer, fill it with transparent texture
            // ID.
            Layer2D layer = new Layer2D(MapBasic.data);
            layer.TilesetName = "tileset01";

            // Here we go, hello TiledMap!
            // - First there is the tileset texture containing all tiles
            // - Then the array of layers (for this example there's just one layer)
            // - And to finish, the tile size. Note that in this example, tiles are
            // squares but you can use rectangle tiles too. Just use another constructor
            // to define tile width and tile height.
            map = new TileMap2D("tileset01", layer, 32);

            // The map can be moved with a simple Vector2. The camera position
            // is screen relative. The top left tile will be drawn at this position
            camera = new Vector2(64, 64);

            // This part is optionnal. You can define a drawing zone for the map :
            // a viewport. When doing this, the map will be rendered only in this zone. 
            // This Rectangle's position is screen relative.
            viewport = new Rectangle(YnG.Width / 2 - 256, YnG.Height / 2 - 256, 512, 512);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            // By calling LoadContent on the map, the tileset texture will be loaded
            // and tile zones will be calculated according to the tile size.
            map.LoadContent();

            // For this example we need an empty texture for drawing the rectangle
            // around the map (ensuring that no pixel gets out of the zone! :-) )
            texture = GraphicsHelper.CreateTexture(Color.White, 1, 1);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();

            // Here we simply draw the map with the camera position and the draw zone
            map.Draw(spriteBatch, camera, viewport);

            // A rectangle is drawn to represent the map's drawing zone
            DrawRectangle(viewport, Color.Red);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            // Simple handling of the camera position
            if (YnG.Keys.Left)
                camera.X++;
            else if (YnG.Keys.Right)
                camera.X--;

            if (YnG.Keys.Up)
                camera.Y++;
            else if (YnG.Keys.Down)
                camera.Y--;

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.ScreenManager.SetScreenActive("menu", true);
            
        }

        /// <summary>
        /// Draw a rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="color"></param>
        private void DrawRectangle(Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, 1), color);
            spriteBatch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, 1), color);
            spriteBatch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Top, 1, rectangle.Height), color);
            spriteBatch.Draw(texture, new Rectangle(rectangle.Right, rectangle.Top, 1, rectangle.Height + 1), color);
        }
    }
}