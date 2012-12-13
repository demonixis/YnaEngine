using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display.TileMap
{
	/// <summary>
	/// This class describes an abstract layer which takes place
	/// in a tiled map
	/// </summary>
	public abstract class AbstractLayer<TileType>
	{
		#region Attributes
		protected int _layerWidth;
		protected int _layerHeight;
		protected TileType[,] _tiles;
		protected string _tilesetName;
		#endregion
		
		#region Properties
		/// <summary>
		/// The layer's width
		/// </summary>
		public int LayerWidth
		{
			get{return _layerWidth;}
		}
		
		/// <summary>
		/// The layer's height
		/// </summary>
		public int LayerHeight
		{
			get{return _layerHeight;}
		}
		
		/// <summary>
		/// The layer's tiles
		/// </summary>
		public TileType[,] Tiles
		{
			get{return _tiles;}
		}
		
		/// <summary>
		/// The layer's tileset name
		/// </summary>
		public string TilesetName{get; set;}
		
		/// <summary>
		/// The tileset mapping
		/// </summary>
		public Rectangle[] Mapping{get; set;}
		
		#endregion
		
		/// <summary>
		/// Access to a single tile by using this array
		/// </summary>
		public TileType this[int x, int y]
		{
			get
			{
				if(x >= 0 && x < _layerWidth  && y >= 0 && y < _layerHeight)
				{
					return _tiles[x, y];
				}
				return default(TileType);
			}
			set
			{
				if(x >= 0 && x < _layerWidth  && y >= 0 && y < _layerHeight)
				{
					_tiles[x, y] = value;
				}
				else
				{
					throw new IndexOutOfRangeException();
				}
			}
		}
	}
}
