﻿using System;

namespace Yna.Display.TiledMap.Basic
{
	/// <summary>
	/// A 2 dimension layer for AbstractTiledMaps
	/// </summary>
	public class Layer2D : AbstractLayer
	{
		/// <summary>
		/// Constructor with size
		/// </summary>
		/// <param name="width">The layer width</param>
		/// <param name="height">The layer height</param>
		public Layer2D(int width, int height)
		{
			_tiles = new Tile2D[width,height];
			_layerWidth = width;
			_layerHeight = height;
			_alpha = 1f;
		}
		
		/// <summary>
		/// Create the layer from a raw data array containing only tile IDs
		/// </summary>
		/// <param name="data">the raw data</param>
		public Layer2D(int[,] data)
		{
			_layerHeight = data.GetUpperBound(0) +1;
			_layerWidth = data.GetUpperBound(1) +1;
			_tiles = new Tile2D[_layerWidth,_layerHeight];
			_alpha = 1f;
			
			for(int x = 0; x < _layerWidth; x++)
			{	
				for(int y = 0; y < _layerHeight; y++)
				{
					_tiles[x, y] = new Tile2D(x, y, data[y, x]);
				}
			}
		}
	}
}
