using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display.Mapping
{
    public class Map : YnObject
    {
        private int _tileWidth;
        private int _tileHeight;
        private Tile[,] _tiles;
        private uint[,] _table;
        private Dictionary<uint, Texture2D> _texturesDictionnary;

        #region Propriétés
        public Tile[,] Tiles
        {
            get { return _tiles; }
            set { _tiles = value; }
        }
		
		public bool Initialized { get; set; }
        #endregion

        public Map() : base()
        {
            _tiles = new Tile[10, 10];
            _tileWidth = 32;
            _tileHeight = 32;
            InitializeTiles();
        }

        public Map(uint[,] table, Dictionary<uint, Texture2D> textures) : this ()
        {
            _tiles = new Tile[table.GetLength(0), table.GetLength(1)];
            _table = table;
            _tileWidth = textures[0].Width;
            _tileHeight = textures[0].Height;
            _texturesDictionnary = textures;
            InitializeWithTextures();
        }

        private void InitializeWithTextures()
        {
            for (int y = 0; y < _tiles.GetLength(0); y++)
            {
                for (int x = 0; x < _tiles.GetLength(1); x++)
                {
                    uint key = _table[y, x];
                    _tiles[y, x] = new Tile(_texturesDictionnary[key], x * _tileWidth, y * _tileHeight);
                    _tiles[y, x].Scale = Scale;
                }
            }
            
            _textureLoaded = true;
            Initialized = true;
            Width = _tiles.GetLength(1);
            Height = _tiles.GetLength(0);
        }

        private void InitializeTiles()
        {
            for (int y = 0; y < _tiles.GetLength(0); y++)
            {
                for (int x = 0; x < _tiles.GetLength(1); x++)
                {
                    _tiles[y, x] = new Tile(x * _tileWidth, y * _tileHeight);
                }
            }

            _textureLoaded = true;
            Initialized = true;
            Width = _tiles.GetLength(1);
            Height = _tiles.GetLength(0);
        }

        public override void Initialize()
        {
            
        }

        public override void LoadContent()
        {
            if (!Initialized)
                InitializeTiles();

			if (!_textureLoaded)
			{
	            for (int y = 0; y < _tiles.GetLength(0); y++)
	            {
	                for (int x = 0; x < _tiles.GetLength(1); x++)
	                {
	                    _tiles[y, x].LoadContent();
	                }
	            }
				_textureLoaded = true;
			}
        }
		
		public override void UnloadContent()
		{
			
		}
		
		public override void Update(GameTime gameTime)
		{
			
		}
		
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    _tiles[y, x].Draw(gameTime, spriteBatch);
                }
            }
		}
		
        public void Draw(SpriteBatch spriteBatch, Rectangle camera, GameTime gt)
        {
			int limitX = camera.X + camera.Width;
			int limitY = camera.Y + camera.Height;
			
			if (limitX >= _tiles.GetLength(1))
				limitX = _tiles.GetLength(1) - 1;
			
			if (limitY >= _tiles.GetLength(0))
				limitY = _tiles.GetLength(0) - 1;
			
            for (int y = camera.Y; y < limitY; y++)
            {
                for (int x = camera.X; x < limitX; x++)
                {
                    //_tiles[y, x].Position = new Vector2((x - camera.X) * 32 * _scale.X, (y - camera.Y) * 32 * _scale.Y);
                    _tiles[y, x].Draw(gt, spriteBatch);
                }
            }
        }

        public void SetPosition(Vector2 position)
        {
            for (int y = 0; y < _tiles.GetLength(0); y++)
            {
                for (int x = 0; x < _tiles.GetLength(1); x++)
                {
                    _tiles[y, x].Position += position;
                }
            }
        }
    }
}
