using System;

namespace Yna.Display.TiledMap
{
	/// <summary>
	/// This class describes an abstract layer which takes place
	/// in a tiled map
	/// </summary>
	public abstract class AbstractLayer
	{
		/// <summary>
		/// The layer's width
		/// </summary>
		protected int _layerWidth;
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
		protected int _layerHeight;
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
		protected AbstractTile[,] _tiles;
		/// <summary>
		/// The layer's tiles
		/// </summary>
		public AbstractTile[,] Tiles
		{
			get{return _tiles;}
		}
		
		/// <summary>
		/// The layer's alpha level
		/// </summary>
		protected float _alpha;
		public float Alpha
		{
			get{return _alpha;}
			set{_alpha = value;}
		}
		
		public AbstractTile GetTile(int x, int y){return _tiles[x, y];}
		public void SetTile(int x, int y, AbstractTile t){_tiles[x,y] = t;}
		
		// The TiledMap
		protected AbstractTiledMap _parent;
		
	}
}
