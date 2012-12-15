using System;

namespace Yna.Framework.Display.TileMap.Isometric
{
	/// <summary>
	/// Tilemap layer for isometric maps
	/// </summary>
	public class LayerIso : AbstractLayer<TileIso>
	{
		/// <summary>
		/// Constructor with size. Tiles are not initialized
		/// </summary>
		/// <param name="width">The layer width</param>
		/// <param name="height">The layer height</param>
		public LayerIso(int width, int height)
		{
			_tiles = new TileIso[width,height];
			_layerWidth = width;
			_layerHeight = height;
		}
		
		/// <summary>
		/// Create the layer from a raw data array containing only tile IDs.
		/// Heights are set to 0.
		/// </summary>
		/// <param name="data">the raw data</param>
		public LayerIso(int[,] data)
		{
			_layerHeight = data.GetUpperBound(0) +1;
			_layerWidth = data.GetUpperBound(1) +1;
			_tiles = new TileIso[_layerWidth,_layerHeight];
			
			for(int x = 0; x < _layerWidth; x++)
			{	
				for(int y = 0; y < _layerHeight; y++)
				{
					_tiles[x, y] = new TileIso(x, y, data[y, x]);
				}
			}
		}
		
	}
}
