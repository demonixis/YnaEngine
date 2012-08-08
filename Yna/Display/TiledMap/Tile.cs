using System;

namespace Yna.Display.TiledMap
{
    /// <summary>
    /// A Tile is a section of a TiledMap.
    /// </summary>
    public class Tile
    {
        #region private declarations
        private int _x;
        private int _y;
        private int _textureID;
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
        public Tile(int x, int y, int textureID)
        {
            _x = x;
            _y = y;
            _textureID = textureID;
        }
    }
}
