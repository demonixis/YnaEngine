// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics.TileMap.Isometric
{
	/// <summary>
	/// Tilemap layer for isometric maps
	/// </summary>
	public class LayerIso : BaseLayer
	{
		/// <summary>
		/// Constructor with size. Tiles are not initialized
		/// </summary>
		/// <param name="width">The layer width</param>
		/// <param name="height">The layer height</param>
		public LayerIso(string tilesetName, int width, int height)
		{
            _tilesetName = tilesetName;
			_tiles = new TileIso[width,height];
			_layerWidth = width;
			_layerHeight = height;
		}
		
		/// <summary>
		/// Create the layer from a raw data array containing only tile IDs.
		/// Heights are set to 0.
		/// </summary>
		/// <param name="data">the raw data</param>
        public LayerIso(string tilesetName, int[,] data)
        {
            _tilesetName = tilesetName;
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

        public override void CreateMapping()
        {
            int tilesPerRow = _tileset.Width / _tileWidth;
            int tilesNumber = _tileset.Height / _tileHeight;

            _mapping = new Rectangle[tilesNumber * 23];
            int index = 0;
            int type = 0;
            for (int y = 0; y < tilesNumber; y++)
            {
                for (int x = 0; x < tilesPerRow; x++)
                {
                    _mapping[x + index * 23] = new Rectangle(_tileWidth * x, _tileHeight * y, _tileWidth, _tileHeight);
                    type++;
                }
                type = 0;
                index++;
            }
        }
	}
}
