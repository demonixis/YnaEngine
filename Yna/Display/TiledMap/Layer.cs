using System;

namespace Yna.Display.TiledMap
{
    /// <summary>
    /// Description of Layer.
    /// </summary>
    public class Layer
    {
        /// <summary>
        /// The layer's width
        /// </summary>
        private int _layerWidth;
        /// <summary>
        /// The layer's width
        /// </summary>
        public int LayerWidth
        {
            get { return _layerWidth; }
        }

        /// <summary>
        /// The layer's height
        /// </summary>
        private int _layerHeight;
        /// <summary>
        /// The layer's height
        /// </summary>
        public int LayerHeight
        {
            get { return _layerHeight; }
        }

        /// <summary>
        /// The layer's tiles
        /// </summary>
        private Tile[,] _tiles;
        /// <summary>
        /// The layer's tiles
        /// </summary>
        public Tile[,] Tiles
        {
            get { return _tiles; }
        }

        /// <summary>
        /// The layer's alpha level
        /// </summary>
        private float _alpha;
        public float Alpha
        {
            get { return _alpha; }
            set { _alpha = value; }
        }

        public Tile GetTile(int x, int y) 
        { 
            return _tiles[x, y]; 
        }

        public void SetTile(int x, int y, Tile tile) 
        { 
            _tiles[x, y] = tile; 
        }

        // The TiledMap
        private TiledMap _parent;

        /// <summary>
        /// Constructor with size
        /// </summary>
        /// <param name="width">The layer width</param>
        /// <param name="height">The layer height</param>
        public Layer(int width, int height)
        {
            _tiles = new Tile[width, height];
            _layerWidth = width;
            _layerHeight = height;
            _alpha = 1f;
        }

        /// <summary>
        /// Create the layer from a raw data array containing only tile IDs
        /// </summary>
        /// <param name="data">the raw data</param>
        public Layer(int[,] data)
        {
            _layerHeight = data.GetUpperBound(0) + 1;
            _layerWidth = data.GetUpperBound(1) + 1;
            _tiles = new Tile[_layerWidth, _layerHeight];
            _alpha = 1f;

            for (int x = 0; x < _layerWidth; x++)
            {
                for (int y = 0; y < _layerHeight; y++)
                {
                    _tiles[x, y] = new Tile(x, y, data[y, x]);
                }
            }
        }
    }
}
