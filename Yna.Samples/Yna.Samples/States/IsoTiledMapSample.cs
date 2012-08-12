using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Display.TiledMap.Isometric;
using Yna;
using Yna.State;

namespace Yna.Sample.States
{
    /// <summary>
    /// Description of IsometricTiledMapSample.
    /// </summary>
    public class IsoTiledMapSample : YnState
    {
        /// <summary>
        /// This is the TiledMap component
        /// </summary>
        private TiledMapIso _map;

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

        public IsoTiledMapSample()
        {
            int[,] groundData = {
				{0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0},
				{0, 0, 0, 0, 0},
			};
            LayerIso groundLayer = new LayerIso(groundData);
            
            for(int x = 0; x < 5; x++)
            {
            	groundLayer.GetTile(x, 0).FlattenTo(1);
            	groundLayer.GetTile(x, 1).TopLeft = 1;
            	groundLayer.GetTile(x, 1).TopRight = 1;
            }

            int[,] decoData = {
				{ 0,  0,  0,  0,  0},
				{-1, -1, -1, -1, -1},
				{-1, -1, -1, -1, -1},
				{-1, -1, -1, -1, -1},
				{ 0,  0,  0,  0,  0}
			};
            LayerIso decoLayer = new LayerIso(decoData);

            LayerIso[] layers = new LayerIso[] { decoLayer };

            _map = new TiledMapIso("Tilesets/iso_ground_tileset", "Tilesets/iso_deco_tileset", groundLayer, layers, 96, 96, 192);

            _camera = new Vector2(YnG.Width / 2 - 96 / 2, 128);
            
            // Define the drawing zone
            _viewport = new Rectangle(YnG.Width/2 - 128, 64, 256, 256);
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Simple handling of the camera position
            if (YnG.Keys.Left)
            {
            	_camera.X+= 2;
            	_camera.Y++;
            }
            if (YnG.Keys.Right)
            {
            	_camera.X -= 2;
            	_camera.Y--;
            }
            if (YnG.Keys.Up)
            {
            	_camera.Y++;
            	_camera.X-=2;
            }
            if (YnG.Keys.Down)
            {
            	_camera.Y--;
            	_camera.X+=2;
            }

            if (YnG.Keys.JustPressed(Keys.Escape))
            {
                YnG.SwitchState(new GameMenu());
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Here we simply draw the map with the camera position and the draw zone
            spriteBatch.Begin();
            _map.Draw(spriteBatch, _camera, _viewport);
            spriteBatch.End();
            
            // A rectangle is drawn to represent the map's drawing zone
            DrawRectangle(_viewport, Color.Red);
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
