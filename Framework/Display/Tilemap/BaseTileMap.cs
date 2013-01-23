﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Framework.Display.TileMap
{
    /// <summary>
    /// Tile based map renderer
    /// </summary>
    public abstract class BaseTileMap
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
        protected BaseLayer[] _layers;
    }
}
