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
	public abstract class AbstractTiledMap
	{
		/// <summary>
		/// Tile width
		/// </summary>
		protected int _tileWidth;
		
		/// <summary>
		/// Tile height
		/// </summary>
		protected int _tileHeight;
		
		/// <summary>
		/// The map width (in tiles)
		/// </summary>
		protected int _mapWidth;
		
		/// <summary>
		/// The map height (in tiles)
		/// </summary>
		protected int _mapHeight;
		
		/// <summary>
		/// Layers definition.
		/// </summary>
		protected AbstractLayer[] _layers;
		
		/// <summary>
		/// All map tiles are stored in one texture, one after the other.
		/// From top left to bottom right, each tile has an id. The first
		/// texture (top left) has the id 0. The cutting is defined by the
		/// tiles size. The tileset must fist a multiple of the tile size!
		/// </summary>
		protected Texture2D _tileset;
		
		/// <summary>
		/// The tileset file name
		/// </summary>
		protected String _tilesetName;
		
	}
}
