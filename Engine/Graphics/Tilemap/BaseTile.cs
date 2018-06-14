// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;

namespace Yna.Engine.Graphics.TileMap
{
    /// <summary>
    /// This class represents a base tile on a tiled map
    /// </summary>
    public abstract class BaseTile
    {
        #region Attributes

        protected int _x;
        protected int _y;
        protected int _textureID;

        #endregion

        #region Properties

        /// <summary>
        /// X tile position in the map
        /// </summary>
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Y tile position in the map
        /// </summary>
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Texture ID (according to the map tileset)
        /// </summary>
        public int TextureID
        {
            get { return _textureID; }
            set { _textureID = value; }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">X position in the map</param>
        /// <param name="y">Y position in the map</param>
        /// <param name="textureID">Texture ID fore rendering</param>
        public BaseTile(int x, int y, int textureID)
            : base()
        {
            _x = x;
            _y = y;
            _textureID = textureID;
        }
    }
}
