// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics.TileMap.Basic
{
	/// <summary>
	/// A 2 dimension layer
	/// </summary>
	public class Layer2D : BaseLayer
	{
		/// <summary>
		/// Constructor with layer size. Tiles are not initialized.
        /// </summary>
        /// <param name="tilesetName">The layer's tileset name</param>
		/// <param name="width">The layer width</param>
		/// <param name="height">The layer height</param>
		public Layer2D(string tilesetName, int width, int height)
		{
            _tilesetName = tilesetName;
			_tiles = new Tile2D[width,height];
			_layerWidth = width;
			_layerHeight = height;
		}
		
		/// <summary>
		/// Create the layer from a raw data array containing tile IDs
        /// </summary>
        /// <param name="tilesetName">The layer's tileset name</param>
		/// <param name="data">the raw data</param>
        public Layer2D(string tilesetName, int[,] data)
            : this(tilesetName, data.GetUpperBound(1), data.GetUpperBound(0))
		{
			for(int x = 0; x < _layerWidth; x++)
			{	
				for(int y = 0; y < _layerHeight; y++)
				{
					this[x, y] = new Tile2D(x, y, data[y, x]);
				}
			}
		}

        public override void CreateMapping()
        {
            int index;
            int tilesPerColumn;
            int tilesPerRow;

            tilesPerRow = _tileset.Width / _tileWidth;
            tilesPerColumn = _tileset.Height / _tileHeight;

            index = 0;
            _mapping = new Rectangle[tilesPerRow * tilesPerColumn];
            for (int y = 0; y < tilesPerColumn; y++)
            {
                for (int x = 0; x < tilesPerRow; x++)
                {
                    // Each tile texture is zoned and stored in mapping array
                    _mapping[index] = new Rectangle(_tileWidth * x, _tileHeight * y, _tileWidth, _tileHeight);
                    index++;
                }
            }
        }
	}
}
