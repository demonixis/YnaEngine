// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics.TileMap.Basic
{
	/// <summary>
	/// Description of TiledMap2D.
	/// </summary>
	public class TileMap2D : BaseTileMap
	{
	
		#region Constructors

		/// <summary>
		/// Basically, the TiledMap is initialized with default values
		/// </summary>
		/// <param name="layers">The map layers</param>
		/// <param name="tileWidth">The tile width</param>
		/// <param name="tileHeight">The tile height</param>
		public TileMap2D(Layer2D[] layers, int tileWidth, int tileHeight) 
		{
			_layers = layers;
			
			_mapWidth = layers[0].LayerWidth;
			_mapHeight = layers[0].LayerHeight;
			
			_tileWidth = tileWidth;
			_tileHeight = tileHeight;
		}

        /// <summary>
        /// Constructor for tiled maps with square tiles (same width and height)
        /// </summary>
        /// <param name="layers">The map layers</param>
        /// <param name="tileSize">The tile width</param>
        public TileMap2D(Layer2D[] layers, int tileSize)
            : this(layers, tileSize, tileSize)
        {
        }

        public TileMap2D(Layer2D layer, int tileSize)
            : this(new Layer2D[] { layer }, tileSize)
        {
        }

		#endregion
		
		/// <summary>
		/// Load the tileset texture and initialize the tiles mapping
		/// </summary>
		public void LoadContent()
		{
			int layerCount = _layers.GetUpperBound(0) + 1;
			
			// Load each layer's tileset
            foreach (Layer2D layer in _layers)
            {
                // Initialize tile size
                layer.TileWidth = _tileWidth;
                layer.TileHeight = _tileHeight;

                layer.LoadContent();
            }
		}
		
		/// <summary>
		/// Draw the tiled map on the entire screen
		/// </summary>
		/// <param name="spriteBatch"></param>
		/// <param name="camera">The current position of the camera on the map</param>
		public void Draw(SpriteBatch spriteBatch, Vector2 camera)
		{
			Draw(spriteBatch, camera, new Rectangle(0, 0, YnG.Width, YnG.Height));
		}
			
		/// <summary>
		/// Draw the tiled map in a specific zone
		/// </summary>
		/// <param name="spriteBatch"></param>
		/// <param name="camera">The current position of the camera on the map</param>
		/// <param name="drawZone">The limited zone where the map will be rendered</param>
		public void Draw(SpriteBatch spriteBatch, Vector2 camera, Rectangle drawZone)
		{
			Layer2D layer;
			BaseTile tile;
			int layerCount = _layers.GetUpperBound(0) +1;
			Rectangle rec;
			Vector2 position = Vector2.Zero;
			int delta;
			for(int layerLevel = 0; layerLevel < layerCount; layerLevel++)
			{
				// Each layer is drawn
				layer = (Layer2D) _layers[layerLevel];
				for(int x = 0; x < _mapWidth; x++)
				{
					for(int y = 0; y < _mapHeight; y++)
					{
						tile = layer[x, y];
						
						// Getting the texture position in the tileset
						rec = layer.Mapping[tile.TextureID];
						
						// Getting tile's real position on screen
						position.X = _tileWidth * tile.X + camera.X;
						position.Y = _tileHeight * tile.Y + camera.Y;
						
						// Draw tiles only if they're in the draw zone
                        // TODO : Check this with rectangle ?
						if(position.X > drawZone.X - _tileWidth
						   && position.X < drawZone.X + drawZone.Width
						   && position.Y > drawZone.Y - _tileHeight
						   && position.Y < drawZone.Y + drawZone.Height)
						{
						
							// If the tile is on the edge of the draw zone, only the portion
							// of the tile in the zone is rendered
							if(position.X < drawZone.X)
							{
								// The tile is out on the left side
								delta = drawZone.X - (int) position.X;
								rec.X += delta;
								rec.Width -= delta;
								position.X = drawZone.X;
							}
							else if(position.X > drawZone.X + drawZone.Width - _tileWidth)
							{
								// The tile is out on the right side
								delta = drawZone.X + drawZone.Width - (int) position.X;
								rec.Width -= _tileWidth - delta;
							}
							
							if(position.Y < drawZone.Y)
							{
								// The tile is out on the top side
								delta = drawZone.Y - (int) position.Y;
								rec.Y += delta;
								rec.Height -= delta;
								position.Y = drawZone.Y;
							}
							else if(position.Y > drawZone.Y + drawZone.Height - _tileHeight)
							{
								// The tile is out on the bottom side
								delta = drawZone.Y + drawZone.Height - (int) position.Y;
								rec.Height -= _tileHeight - delta;
								
							}
							
							// The tile is totally on the screen
							spriteBatch.Draw(layer.Tileset, position, rec, Color.White);
						}
					}
				}
			}
		}
		
	}
}
