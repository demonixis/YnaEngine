using System;

namespace Yna.Display.TiledMap
{
    /// <summary>
    /// This class represents a tile on a tiled map
    /// </summary>
    public abstract class AbstractTile
    {
        protected int _x;
        protected int _y;
        protected int _textureID;

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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">X position in the map</param>
        /// <param name="y">Y position in the map</param>
        /// <param name="textureID">Texture ID fore rendering</param>
        public AbstractTile(int x, int y, int textureID)
            : base()
        {
            _x = x;
            _y = y;
            _textureID = textureID;
        }
    }
}
