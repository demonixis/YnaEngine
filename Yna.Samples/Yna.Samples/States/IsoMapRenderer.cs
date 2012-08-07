using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Display.TiledMap;
using Yna.Input;
using Yna.Sample.States;
using Yna.State;

namespace Yna.Samples.Windows.States
{
	/// <summary>
	/// Description of IsoMapRenderer.
	/// </summary>
	public class IsoMapRenderer : YnState
	{
		private Texture2D _tileset;
		private TiledMap _map;
		private Vector2 _camera;
		private Rectangle _viewer;
		private Texture2D _dummyTexture;
		
		public IsoMapRenderer()
		{
			_tileset = YnG.Content.Load<Texture2D>("Spritesheets//tileset");
			
			int[,] data = {
				{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,0,1,2,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,3,4,5,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,6,7,8,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,0,1,2,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,3,4,5,4,4,4,4,4,4,4,4,4,4},
				{4,4,4,4,4,4,4,6,7,8,4,4,4,4,4,4,4,4,4,4},
			};
			Layer layer1 = new Layer(data);
			
			int[,] data2 = {
				{14,13,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14},
				{14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14},
				{14,14,14,14,14,14,14,14,13,14,14,14,14,14,14,14,14,14,14,14},
				{14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14},
				{14,14,14,14,13,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14},
				{14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14},
				{14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14},
				{14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14},
				{14,13,14,14,14,14,13,14,14,14,14,14,14,14,14,14,14,14,14,14},
				{14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14}
			};
			Layer layer2 = new Layer(data2);
			layer2.Alpha = 100f;
			
			Layer[] layers = new Layer[2];
			layers[0] = layer1;
			layers[1] = layer2;
			
			_map = new TiledMap("Spritesheets//tileset", layers, 20);
			
			_camera = new Vector2(64, 64);
			_viewer = new Rectangle(128, 128, 256, 256);
		}
		
		public override void LoadContent()
		{
			base.LoadContent();
			_map.LoadContent();
			
			_dummyTexture = new Texture2D(YnG.GraphicsDevice, 1, 1);
			_dummyTexture.SetData(new Color[]{Color.White});
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			
			spriteBatch.Begin();
			DrawRectangle(_viewer, Color.Red);
			spriteBatch.End();
			_map.Draw(spriteBatch, _camera, _viewer);
		}
		
		public override void Update(GameTime gameTime)
		{
			if(YnG.Mouse.Clicked(MouseButton.Left))
			{
			   	// Zoom in
			   //	ScreenRotatio
			}
			else if(YnG.Mouse.Clicked(MouseButton.Right))
			{
		   		// Zoom out
			}
			
			if(YnG.Keys.Left) _camera.X++;
			if(YnG.Keys.Right) _camera.X--;
			if(YnG.Keys.Up) _camera.Y++;
			if(YnG.Keys.Down) _camera.Y--;
			
			
		}
		
		private void DrawRectangle(Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(_dummyTexture, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, 1), color);
            spriteBatch.Draw(_dummyTexture, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, 1), color);
            spriteBatch.Draw(_dummyTexture, new Rectangle(rectangle.Left, rectangle.Top, 1, rectangle.Height), color);
            spriteBatch.Draw(_dummyTexture, new Rectangle(rectangle.Right, rectangle.Top, 1, rectangle.Height + 1), color);
        }

	}
}
