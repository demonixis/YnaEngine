using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display.TiledMap.Isometric
{
	/// <summary>
	/// Description of IsometricTiledMap.
	/// </summary>
	public class TiledMapIso : AbstractTiledMap
	{
		/// <summary>
		/// This array contains zoning for each tile. It is initialized when
		/// the tileset texture is loaded
		/// </summary>
		private Rectangle[,] _tilesMapping;
		
		/// <summary>
		/// Basically, the TiledMap is initialized with default values
		/// </summary>
		/// <param name="tilesetName">The Tileset file name</param>
		/// <param name="layers">The map layers</param>
		/// <param name="tileWidth">The tile width</param>
		/// <param name="tileHeight">The tile height</param>
		public TiledMapIso(String tilesetName, LayerIso[] layers, int tileWidth, int tileHeight)
		{
			_tilesetName = tilesetName;
			_layers = layers;
			_tileWidth = tileWidth;
			_tileHeight = _tileHeight;
		}
		
				/// <summary>
		/// Load the tileset texture and initialize the tiles mapping
		/// </summary>
		public void LoadContent()
		{
			_tileset = YnG.Content.Load<Texture2D>(_tilesetName);
			
			// Tiles MUST be ordered in certain way. A row contains all possible representations of a tile
			// in an isometric map. There is 8 textures for representing the tile in all
			// directions and height. Only 5 are needed, the 3 others can be calculated by mirroring the
			// others.
			
			// TODO handle the total definition of the tiles (no use of mirrors)
			
			// Initialize the tile zoning
			int tilesPerRow = _tileset.Width / _tileWidth;
			int tilesNumber = _tileset.Height / _tileHeight;
			
			_tilesMapping = new Rectangle[tilesNumber, 8];
			int index = 0;
			for(int y = 0; y < tilesNumber;y++)
			{
				for(int x = 0; x < tilesPerRow;x++)
				{
					//_tilesMapping[index] = new Rectangle(_tileWidth*x, _tileHeight*y, _tileWidth, _tileHeight);
					//index++;
				}
			}
		}
	}
}
