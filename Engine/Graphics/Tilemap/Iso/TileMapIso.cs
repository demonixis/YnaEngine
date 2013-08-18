// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Helpers;

namespace Yna.Engine.Graphics.TileMap.Isometric
{
    /// <summary>
    /// Description of IsometricTiledMap.
    /// </summary>
    public class TileMapIso : BaseTileMap
    {
        #region Attributes
        /// <summary>
        /// This array contains zoning for each tile. It is initialized when
        /// the tileset texture is loaded
        /// </summary>+
        private Rectangle[,] _tilesMapping;

        /// <summary>
        /// The decoration tile height
        /// </summary>
        private int _decoTileHeight;

        /// <summary>
        /// The decoration tileset name
        /// </summary>
        private string _decoTilesetName;

        /// <summary>
        /// The tileset containing all decoration textures
        /// </summary>
        private Texture2D _decoTileset;

        /// <summary>
        /// The decoration tileset mapping
        /// </summary>
        private Rectangle[,] _decoMapping;

        /// <summary>
        /// In TiledMapIso, the ground layer is defined here
        /// </summary>
        private LayerIso _groundLayer;

        #endregion

        #region Constructors
        /// <summary>
        /// Basically, the TiledMap is initialized with default values
        /// </summary>
        /// <param name="groundLayer">The ground layers</param>
        /// <param name="layers">The deco layers</param>
        /// <param name="groundTileWidth">The ground tile width</param>
        /// <param name="groundTileHeight">The ground tile height</param>
        /// <param name="decoTileHeight">The decoration tile height</param>
        public TileMapIso(LayerIso groundLayer, LayerIso[] layers, int groundTileWidth, int groundTileHeight, int decoTileHeight)
        {
            //_tilesetName = groundTilesetName;
            _groundLayer = groundLayer;
            _layers = layers;
            _tileWidth = groundTileWidth;
            _tileHeight = groundTileHeight;
            _mapWidth = groundLayer.LayerWidth;
            _mapHeight = groundLayer.LayerHeight;
            _decoTileHeight = decoTileHeight;
        }

        /// <summary>
        /// Basically, the TiledMap is initialized with default values
        /// </summary>
        /// <param name="groundTilesetName">The Tileset file name</param>
        /// <param name="groundLayer">The ground layers</param>
        /// <param name="tileWidth">The tile width</param>
        /// <param name="tileHeight">The tile height</param>
        public TileMapIso(LayerIso groundLayer, int tileWidth, int tileHeight)
            : this(groundLayer, new LayerIso[] { }, tileWidth, tileHeight, 0)
        {
        }

        #endregion

        /// <summary>
        /// Load the tileset texture and initialize the tiles mapping
        /// </summary>
        public void LoadContent()
        {
            // Load content for the ground layer
            // Initialize tile size
            _groundLayer.TileWidth = _tileWidth;
            _groundLayer.TileHeight = _tileHeight;
            _groundLayer.LoadContent();

            // Load content for each decoration layer
            // TODO
            foreach (BaseLayer layer in _layers)
            {
                layer.TileWidth = _tileWidth;
                layer.TileHeight = _decoTileHeight;
                layer.LoadContent();
            }

            // Tiles MUST be ordered in certain way. A row contains all possible representations of a tile
            // in an isometric map. There is 9 textures for representing the tile in all
            // directions and height. Only 6 are needed, the 3 others can be calculated by mirroring the
            // others.

            // TODO Handle unomplete tiles definitions
            /*
            // Initialize the tile zoning
            int tilesPerRow = _tileset.Width / _tileWidth;
            int tilesNumber = _tileset.Height / _tileHeight;

            _tilesMapping = new Rectangle[tilesNumber, 23];
            int index = 0;
            int type = 0;
            for (int y = 0; y < tilesNumber; y++)
            {
                for (int x = 0; x < tilesPerRow; x++)
                {
                    _tilesMapping[index, type] = new Rectangle(_tileWidth * x, _tileHeight * y, _tileWidth, _tileHeight);
                    type++;
                }
                type = 0;
                index++;
            }

            // Now load the decoration tileset if present
            // TODO Handle unomplete tiles definitions
            if (_decoTilesetName != null)
            {
                _decoTileset = YnG.Content.Load<Texture2D>(_decoTilesetName);

                // Initialize the tile zoning
                tilesPerRow = _decoTileset.Width / _tileWidth;
                tilesNumber = _decoTileset.Height / _decoTileHeight;

                _decoMapping = new Rectangle[tilesNumber, 23];
                index = 0;
                type = 0;
                for (int y = 0; y < tilesNumber; y++)
                {
                    for (int x = 0; x < tilesPerRow; x++)
                    {
                        _decoMapping[index, type] = new Rectangle(_tileWidth * x, _decoTileHeight * y, _tileWidth, _decoTileHeight);
                        type++;
                    }
                    type = 0;
                    index++;
                }
            }
             */
        }

        /// <summary>
        /// Draw the tiled map on the entire screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="camera">The current position of the camera on the map</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 camera)
        {
            // TODO use static rectangle instead of this
            Rectangle rec = new Rectangle(0, 0, YnG.Width, YnG.Height);
            Draw(spriteBatch, camera, rec);
        }

        /// <summary>
        /// Draw the tiled map on a specific zone
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="camera">The current position of the camera on the map</param>
        /// <param name="drawZone">The limited zone where the map will be rendered</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 camera, Rectangle drawZone)
        {
            // Draw the base layer (ground)
            DrawLayer(spriteBatch, camera, drawZone, _groundLayer, _tileWidth, _tileHeight, false);

            // Draw the decoration layers
            DrawDeco(spriteBatch, camera, drawZone);
        }

        /// <summary>
        /// Draw the decoration layers
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="camera">The current position of the camera on the map</param>
        /// <param name="drawZone">The limited zone where the map will be rendered</param>
        public void DrawDeco(SpriteBatch spriteBatch, Vector2 camera, Rectangle drawZone)
        {
            LayerIso layer;
            int layerCount = _layers.GetUpperBound(0) + 1;
            for (int layerLevel = 0; layerLevel < layerCount; layerLevel++)
            {
                // Each layer is drawn
                layer = (LayerIso)_layers[layerLevel];
                DrawLayer(spriteBatch, camera, drawZone, layer, _tileWidth, _decoTileHeight, true);
            }
        }

        public void DrawLayer(SpriteBatch spriteBatch, Vector2 camera, Rectangle drawZone, LayerIso layer, int tileWidth, int tileHeight, bool isDeco)
        {
            TileIso tile;
            Rectangle texRect;
            Vector2 position;
            int delta;
            int decoDelta = 0;
            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    tile = (TileIso)layer[x, y];
                    decoDelta = 0;

                    // Negative ID represents invisible tiles and do not need to be rendered
                    if (tile.TextureID >= 0)
                    {
                        // Here we get the translated coordinates of the current tile
                        // Note that this position is the simple translation of the tile
                        // position on the map.
                        position = IsometricHelper.ToOrtho(x, y);

                        // To get correct orthogonals coordinates, they're respectively 
                        // multiplied by half width and half height
                        position.X *= _tileWidth / 2;
                        position.Y *= _tileHeight / 2;

                        // We're almost done : add the camera position
                        position.X += camera.X;
                        position.Y += camera.Y;

                        // Special translation for decoration tiles. If the tile hieght is
                        // different from the ground tile size, an adjustment is needed
                        /*
						if(tileset != _tileset)
						{
							// Here we have a decoration layer
							position.Y -= _decoTileHeight/2;
						}
						*/
                        // Textures may not be rendered entirely if the drawZone parameter
                        // is a smaller rectangle than the game window.
                        texRect = layer.Mapping[tile.TileType() + tile.TextureID * 23];
                        if (drawZone.Width != YnG.Width && drawZone.Height != YnG.Height)
                        {
                            // The tile must be cropped.
                            // If the tile is on the edge of the draw zone, only the portion
                            // of the tile in the zone is rendered

                            // For decoration layers, tile height must be handled too
                            if (isDeco)
                            {
                                position.Y -= _decoTileHeight / 2;
                            }

                            if (position.X < drawZone.X)
                            {
                                // The tile is out on the left side
                                delta = drawZone.X - (int)position.X;
                                texRect.X += delta;
                                texRect.Width -= delta;
                                position.X = drawZone.X;
                            }
                            else if (position.X > drawZone.X + drawZone.Width - tileWidth)
                            {
                                // The tile is out on the right side
                                delta = drawZone.X + drawZone.Width - (int)position.X;
                                texRect.Width -= tileWidth - delta;
                            }

                            if (position.Y < drawZone.Y)
                            {
                                // The tile is out on the top side
                                delta = drawZone.Y - (int)position.Y;
                                texRect.Y += delta;
                                texRect.Height -= delta;
                                position.Y = drawZone.Y;
                            }
                            else if (position.Y > drawZone.Y + drawZone.Height - tileHeight)
                            {
                                // The tile is out on the bottom side
                                delta = drawZone.Y + drawZone.Height - (int)position.Y;
                                texRect.Height -= tileHeight - delta;
                            }
                        }

                        

                        spriteBatch.Draw(layer.Tileset, position, texRect, Color.White);
                    }
                }
            }
        }
    }
}
