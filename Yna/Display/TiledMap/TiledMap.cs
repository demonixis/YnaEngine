using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display.TiledMap
{
	/// <summary>
	/// Tile based map renderer
	/// </summary>
	public class TiledMap
	{
		/// <summary>
		/// Tile width
		/// </summary>
		private int _tileWidth;
		
		/// <summary>
		/// Tile height
		/// </summary>
		private int _tileHeight;
		
		/// <summary>
		/// The map width (in tiles)
		/// </summary>
		private int _mapWidth;
		
		/// <summary>
		/// The map height (in tiles)
		/// </summary>
		private int _mapHeight;
		
		/// <summary>
		/// Layers definition.
		/// </summary>
		private Layer[] _layers;
		
		/// <summary>
		/// All map tiles are stored in one texture, one after the other.
		/// From top left to bottom right, each tile has an id. The first
		/// texture (top left) has the id 0. The cutting is defined by the
		/// tiles size. The tileset must fist a multiple of the tile size!
		/// </summary>
		private Texture2D _tileset;
		
		/// <summary>
		/// The tileset file name
		/// </summary>
		private String _tilesetName;
		
		/// <summary>
		/// This array contains zoning for each tile. It is initialized when
		/// the tileset texture is loaded
		/// </summary>
		private Rectangle[] _tilesMapping;
		
		/// <summary>
		/// Constructor for tiled maps with square tiles (same width and height)
		/// </summary>
		/// <param name="tilesetName">The Tileset file name</param>
		/// <param name="layers">The map layers</param>
		/// <param name="tileSize">The tile width</param>
		public TiledMap(String tilesetName, Layer[] layers, int tileSize) : this(tilesetName, layers, tileSize, tileSize)
		{
		}
		
		/// <summary>
		/// Basically, the TiledMap is initialized with default values
		/// </summary>
		/// <param name="tilesetName">The Tileset file name</param>
		/// <param name="layers">The map layers</param>
		/// <param name="tileWidth">The tile width</param>
		/// <param name="tileHeight">The tile height</param>
		public TiledMap(String tilesetName, Layer[] layers, int tileWidth, int tileHeight) : base()
		{
			_tilesetName = tilesetName;
			
			_layers = layers;
			
			// Default map size : 10 tiles 
			_mapWidth = layers[0].LayerWidth;
			_mapHeight = layers[0].LayerHeight;
			
			// Default Tile width/height : 32px
			_tileWidth = tileWidth;
			_tileHeight = tileHeight;
		}
		
		/// <summary>
		/// Load the tileset texture and initialize the tiles mapping
		/// </summary>
		public void LoadContent()
		{
			_tileset = YnG.Content.Load<Texture2D>(_tilesetName);
			
			// Initialize the tile zoning
			int tilesPerRow = _tileset.Width / _tileWidth;
			int tilesPerColumn = _tileset.Height / _tileHeight;
			
			_tilesMapping = new Rectangle[tilesPerRow * tilesPerColumn];
			int index = 0;
			for(int y = 0; y < tilesPerColumn;y++)
			{
				for(int x = 0; x < tilesPerRow;x++)
				{
					_tilesMapping[index] = new Rectangle(_tileWidth*x, _tileHeight*y, _tileWidth, _tileHeight);
					index++;
				}
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
			Layer layer;
			Tile tile;
			int layerCount = _layers.Count();
			Rectangle rec;
			Vector2 position = Vector2.Zero;
			int delta;
			for(int layerLevel = 0; layerLevel < layerCount; layerLevel++)
			{
				// Each layer is drawn
				layer = _layers[layerLevel];
				for(int x = 0; x < _mapWidth; x++)
				{
					for(int y = 0; y < _mapHeight; y++)
					{
						tile = layer.GetTile(x, y);
						
						// Getting the texture position in the tileset
						rec = _tilesMapping[tile.TextureID];
						
						// Getting tile's real position on screen
						position.X = _tileWidth * tile.X + camera.X;
						position.Y = _tileHeight * tile.Y + camera.Y;
						
						// Draw tiles only if they're in the draw zone
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
							spriteBatch.Draw(_tileset, position, rec, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 1f);
						}
					}
				}
			}
		}
	}
}
