using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display.TiledMap.Isometric
{
	/// <summary>
	/// Description of IsometricTiledMap.
	/// </summary>
	public class TiledMapIso : AbstractTileMap<LayerIso>
	{
		#region Attributes
		/// <summary>
		/// This array contains zoning for each tile. It is initialized when
		/// the tileset texture is loaded
		/// </summary>
		private Rectangle[,] _tilesMapping;
		
		/// <summary>
		/// The decoration tile height
		/// </summary>
		private int _decoTileHeight;
		
		/// <summary>
		/// The decoration tileset name
		/// </summary>
		private string _decoTilesetName;
		
		/// <summary>
		/// The tileset containing all decoration textures
		/// </summary>
		private Texture2D _decoTileset;
		
		/// <summary>
		/// The decoration tileset mapping
		/// </summary>
		private Rectangle[,] _decoMapping;
		
		/// <summary>
		/// In TiledMapIso, the ground layer is defined here
		/// </summary>
		private LayerIso _groundLayer;
		#endregion
		
		#region Constructors
		/// <summary>
		/// Basically, the TiledMap is initialized with default values
		/// </summary>
		/// <param name="groundTilesetName">The ground tileset file name</param>
		/// <param name="decoTilesetName">The deco tileset file name</param>
		/// <param name="groundLayer">The ground layers</param>
		/// <param name="layers">The deco layers</param>
		/// <param name="groundTileWidth">The ground tile width</param>
		/// <param name="groundTileHeight">The ground tile height</param>
		/// <param name="decoTileHeight">The decoration tile height</param>
		public TiledMapIso(string groundTilesetName, string decoTilesetName, LayerIso groundLayer, LayerIso[] layers, int groundTileWidth, int groundTileHeight, int decoTileHeight)
		{
			_tilesetName = groundTilesetName;
			_decoTilesetName = decoTilesetName;
			_groundLayer = groundLayer;
			_layers = layers;
			_tileWidth = groundTileWidth;
			_tileHeight = groundTileHeight;
			_mapWidth = layers[0].LayerWidth;
			_mapHeight = layers[0].LayerHeight;
			_decoTileHeight = decoTileHeight;
		}
		
		/// <summary>
		/// Basically, the TiledMap is initialized with default values
		/// </summary>
		/// <param name="groundTilesetName">The Tileset file name</param>
		/// <param name="groundLayer">The ground layers</param>
		/// <param name="layers">The deco layers</param>
		/// <param name="tileWidth">The tile width</param>
		/// <param name="tileHeight">The tile height</param>
		public TiledMapIso(string groundTilesetName, LayerIso groundLayer, LayerIso[] layers, int tileWidth, int tileHeight)
			: this(groundTilesetName, null, groundLayer, layers, tileWidth, tileHeight, 0)
		{
		}
		#endregion
		
		/// <summary>
		/// Load the tileset texture and initialize the tiles mapping
		/// </summary>
		public void LoadContent()
		{
			_tileset = YnG.Content.Load<Texture2D>(_tilesetName);
			
			// Tiles MUST be ordered in certain way. A row contains all possible representations of a tile
			// in an isometric map. There is 9 textures for representing the tile in all
			// directions and height. Only 6 are needed, the 3 others can be calculated by mirroring the
			// others.
			
			// TODO Handle unomplete tiles definitions
			
			// Initialize the tile zoning
			int tilesPerRow = _tileset.Width / _tileWidth;
			int tilesNumber = _tileset.Height / _tileHeight;
			
			_tilesMapping = new Rectangle[tilesNumber, 23];
			int index = 0;
			int type = 0;
			for(int y = 0; y < tilesNumber;y++)
			{
				for(int x = 0; x < tilesPerRow;x++)
				{
					_tilesMapping[index, type] = new Rectangle(_tileWidth*x, _tileHeight*y, _tileWidth, _tileHeight);
					type++;
				}
				type = 0;
				index++;
			}
			
			// Now load the decoration tileset if present
			// TODO Handle unomplete tiles definitions
			if(_decoTilesetName != null)
			{
				_decoTileset = YnG.Content.Load<Texture2D>(_decoTilesetName);
				
				// Initialize the tile zoning
				tilesPerRow = _decoTileset.Width / _tileWidth;
				tilesNumber = _decoTileset.Height / _decoTileHeight;
				
				_decoMapping = new Rectangle[tilesNumber, 23];
				index = 0;
				type = 0;
				for(int y = 0; y < tilesNumber;y++)
				{
					for(int x = 0; x < tilesPerRow;x++)
					{
						_decoMapping[index, type] = new Rectangle(_tileWidth*x, _decoTileHeight*y, _tileWidth, _decoTileHeight);
						type++;
					}
					type = 0;
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
			// TODO use static rectangle instead of this
			Rectangle rec = new Rectangle(0, 0, YnG.Width, YnG.Height);
			DrawGround(spriteBatch, camera, rec);
			DrawDeco(spriteBatch, camera, rec);
		}
		
		/// <summary>
		/// Draw the tiled map on a specific zone
		/// </summary>
		/// <param name="spriteBatch"></param>
		/// <param name="camera">The current position of the camera on the map</param>
		/// <param name="drawZone">The limited zone where the map will be rendered</param>
		public void Draw(SpriteBatch spriteBatch, Vector2 camera, Rectangle drawZone)
		{
			DrawGround(spriteBatch, camera, drawZone);
			DrawDeco(spriteBatch, camera, drawZone);
		}
		
		/// <summary>
		/// Draw the decoration layers
		/// </summary>
		/// <param name="spriteBatch"></param>
		/// <param name="camera">The current position of the camera on the map</param>
		/// <param name="drawZone">The limited zone where the map will be rendered</param>
		public void DrawDeco(SpriteBatch spriteBatch, Vector2 camera, Rectangle drawZone)
		{
			// The camera position is the top vertex rendered
			LayerIso layer;
			TileIso tile;
			int layerCount = _layers.GetUpperBound(0) +1;
			Rectangle rec;
			Vector2 position = Vector2.Zero;
			int tileType;
			int tileHeight;
			Texture2D tileset;
			int delta;
			for(int layerLevel = 0; layerLevel < layerCount; layerLevel++)
			{
				// Each layer is drawn
				layer = (LayerIso) _layers[layerLevel];
				for(int x = 0; x < _mapWidth; x++)
				{
					for(int y = 0; y < _mapHeight; y++)
					{
						tile = layer[x, y];
						// A texture ID of -1 means that the tile is unused : it's not rendered
						if(tile.TextureID != -1)
						{
							// Getting the texture position in the tileset
							tileType = tile.TileType();
							rec = _decoMapping[tile.TextureID, tileType];
							tileHeight = _decoTileHeight;
							tileset = _decoTileset;
							
							// Getting tile's real position on screen
							position.X = camera.X  + (_tileWidth * x / 2) - (_tileHeight * y/2);
							position.Y = camera.Y + (_tileHeight * x / 4) + (_tileHeight * y / 4) - (_tileHeight/4 * tile.MinHeight());
							
							// All tile textures are aligned to the bottom in order to
							// handle differents textures height. The Y position calculated
							// above is the position of the top left edge on the map. This
							// value must be translated to align the texture to the bottom
							// of the tile
							position.Y -= tileHeight - _tileHeight/2 + (_groundLayer[x, y].MinHeight() * _tileHeight/4);
							
							// Handle tiles out of the screen
							if(position.X >= drawZone.X - _tileWidth
						   		|| position.X < drawZone.X + drawZone.Width
						   		||position.Y >= drawZone.Y - _tileHeight
						   		|| position.Y < drawZone.Y + drawZone.Height)
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
								else if(position.Y > drawZone.Y + drawZone.Height - _decoTileHeight)
								{
									// The tile is out on the bottom side
									delta = drawZone.Y + drawZone.Height - (int) position.Y;
									rec.Height -= _decoTileHeight - delta;
								}
								
								spriteBatch.Draw(tileset, position, rec, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 1f);
							}
							// TODO handle vertical tiles
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Draw the ground layer
		/// </summary>
		/// <param name="spriteBatch"></param>
		/// <param name="camera">The current position of the camera on the map</param>
		/// <param name="drawZone">The limited zone where the map will be rendered</param>
		private void DrawGround(SpriteBatch spriteBatch, Vector2 camera, Rectangle drawZone)
		{
			// The camera position is the top vertex rendered
			TileIso tile;
			int layerCount = _layers.GetUpperBound(0) +1;
			Rectangle rec;
			Vector2 position = Vector2.Zero;
			int tileType;
			int tileHeight;
			Texture2D tileset;
			int delta;
			// Each layer is drawn
			for(int x = 0; x < _mapWidth; x++)
			{
				for(int y = 0; y < _mapHeight; y++)
				{
					tile = _groundLayer[x, y];
					// A texture ID of -1 means that the tile is unused : it's not rendered
					if(tile.TextureID != -1)
					{
						// Getting the texture position in the tileset
						tileType = tile.TileType();
						rec = _tilesMapping[tile.TextureID, tileType];
						tileHeight = _tileHeight;
						tileset = _tileset;
						
						// Getting tile's real position on screen
						position.X = camera.X  + (_tileWidth * x / 2) - (_tileHeight * y/2);
						position.Y = camera.Y + (_tileHeight * x / 4) + (_tileHeight * y / 4) - (_tileHeight/4 * tile.MinHeight());
						
						// All tile textures are aligned to the bottom in order to
						// handle differents textures height. The Y position calculated
						// above is the position of the top left edge on the map. This
						// value must be translated to align the texture to the bottom
						// of the tile
						position.Y -= tileHeight /2;
						
						// Handle tiles out of the screen
						// TODO factorize this part
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
							
							spriteBatch.Draw(tileset, position, rec, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 1f);
						}
						// TODO handle vertical tiles
					}
				}
			}
		}
	}
}
