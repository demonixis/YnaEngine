using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.TileMap.Isometric;
using Yna.Engine.Helpers;
using Yna.Samples.Data;

namespace Yna.Samples.Screens
{
    public class IsometricMapSample : YnState2D
    {
        private TileMapIso _map;
        private Vector2 camera;
        private Rectangle viewport;
        private Texture2D texture;

        public IsometricMapSample(string name)
            : base(name)
        {
            LayerIso groundLayer = new LayerIso("Sprites/iso_ground_tileset", IsometricMap.GroundData);

            LayerIso decoLayer = new LayerIso("Sprites/iso_deco_tileset", IsometricMap.DecorationData);

            LayerIso[] layers = new LayerIso[] { decoLayer };

            _map = new TileMapIso(groundLayer, layers, 96, 96, 192);

            camera = new Vector2(YnG.Width / 2 - 96 / 2, 128);

            // Define the drawing zone
            viewport = new Rectangle(YnG.Width / 2 - 256, YnG.Height / 2 - 256, 512, 512);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            // By calling LoadContent on the map, the tileset texture will be loaded
            // and tile zones will be calculated according to the tile size.
            _map.LoadContent();

            // For this example we need an empty texture for drawing the rectangle
            // around the map (ensuring that no pixel gets out of the zone! :-) )
            texture = YnGraphics.CreateTexture(Color.White, 1, 1);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Simple handling of the camera position
            if (YnG.Keys.Up)
            {
                camera.Y++;
                camera.X -= 2;
            }
            else if (YnG.Keys.Down)
            {
                camera.Y--;
                camera.X += 2;
            }

            if (YnG.Keys.Left)
            {
                camera.X += 2;
                camera.Y++;
            }
            else if (YnG.Keys.Right)
            {
                camera.X -= 2;
                camera.Y--;
            }


            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.StateManager.SetActive("menu", true);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Here we simply draw the map with the camera position and the draw zone
            spriteBatch.Begin();
            _map.Draw(spriteBatch, camera, viewport);

            // A rectangle is drawn to represent the map's drawing zone
            DrawRectangle(viewport, Color.Red);
            spriteBatch.End();
        }

        private void DrawRectangle(Rectangle rectangle, Color color)
        {
            // Draw each border as a texture
            spriteBatch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, 1), color);
            spriteBatch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, 1), color);
            spriteBatch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Top, 1, rectangle.Height), color);
            spriteBatch.Draw(texture, new Rectangle(rectangle.Right, rectangle.Top, 1, rectangle.Height + 1), color);
        }
    }
}