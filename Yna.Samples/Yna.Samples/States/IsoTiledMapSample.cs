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

            int[,] decoData = {
				{ 0,  0,  0,  0,  0},
				{-1, -1, -1, -1, -1},
				{-1, -1, -1, -1, -1},
				{-1, -1, -1, -1, -1},
				{ 0,  0,  0,  0,  0}
			};
            LayerIso decoLayer = new LayerIso(decoData);

            LayerIso[] layers = new LayerIso[] { groundLayer, decoLayer };

            _map = new TiledMapIso("Tilesets//iso_ground_tileset", "Tilesets//iso_deco_tileset", layers, 96, 96, 192);

            _camera = new Vector2(YnG.Width / 2 - 96 / 2, 128);
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

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.SwitchState(new GameMenu());
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Here we simply draw the map with the camera position and the draw zone
            spriteBatch.Begin();
            _map.Draw(spriteBatch, _camera, _viewport);
            spriteBatch.End();
            // A rectangle is drawn to represent the map's drawing zone
            //DrawRectangle(_viewport, Color.Red);
        }
    }
}
