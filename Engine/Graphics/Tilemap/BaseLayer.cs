// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics.TileMap
{
    /// <summary>
    /// This class describes an abstract layer which takes place
    /// in a tiled map
    /// </summary>
    public abstract class BaseLayer
    {
        #region Attributes

        protected int _layerWidth;
        protected int _layerHeight;
        protected BaseTile[,] _tiles;
        protected Rectangle[] _mapping;
        protected Texture2D _tileset;
        protected String _tilesetName;
        protected int _tileWidth;
        protected int _tileHeight;

        #endregion

        #region Properties

        /// <summary>
        /// The layer's width
        /// </summary>
        public int LayerWidth { get { return _layerWidth; } }

        /// <summary>
        /// The layer's height
        /// </summary>
        public int LayerHeight { get { return _layerHeight; } }

        /// <summary>
        /// The layer's tiles
        /// </summary>
        public BaseTile[,] Tiles { get { return _tiles; } }

        /// <summary>
        /// The layer's tileset file name
        /// </summary>
        public string TilesetName
        {
            get { return _tilesetName; }
            set { _tilesetName = value; }
        }

        /// <summary>
        /// All map tiles are stored in one texture, one after the other.
        /// From top left to bottom right, each tile has an id. The first
        /// texture (top left) has the id 0. The cutting is defined by the
        /// tiles size. The tileset must fist a multiple of the tile size!
        /// </summary>
        public Texture2D Tileset
        {
            get { return _tileset; }
            set { _tileset = value; }
        }

        /// <summary>
        /// The tileset mapping
        /// </summary>
        public Rectangle[] Mapping
        {
            get { return _mapping; }
            set { _mapping = value; }
        }

        /// <summary>
        /// The tile width
        /// </summary>
        public int TileWidth
        {
            get { return _layerWidth; }
            set { _tileWidth = value; }
        }

        /// <summary>
        /// The tile height
        /// </summary>
        public int TileHeight
        {
            get { return _tileHeight; }
            set { _tileHeight = value; }
        }

        #endregion

        /// <summary>
        /// Access to a single tile by using this array
        /// </summary>
        public BaseTile this[int x, int y]
        {
            get
            {
                if (x >= 0 && x < _layerWidth && y >= 0 && y < _layerHeight)
                {
                    return _tiles[x, y];
                }
                return default(BaseTile);
            }
            set
            {
                if (x >= 0 && x < _layerWidth && y >= 0 && y < _layerHeight)
                {
                    _tiles[x, y] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// This method creates tiles mapping
        /// </summary>
        public abstract void CreateMapping();

        /// <summary>
        /// Load the tileset texture and create tiles mapping
        /// </summary>
        public void LoadContent()
        {
            // Load the layer's tileset
            _tileset = YnG.Content.Load<Texture2D>(_tilesetName);

            // Create custom mapping according to the layer type
            CreateMapping();
        }
    }
}
