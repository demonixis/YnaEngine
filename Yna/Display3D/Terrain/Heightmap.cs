using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display3D.Camera;

namespace Yna.Display3D.Terrain
{
    public class Heightmap : BaseTerrain
    {
        private string _heightmapAssetName;
        private Texture2D _heightmapTexture;
        private float[,] _heightData;

        public Heightmap(string heightmapAsset)
            : base()
        {
            _heightmapAssetName = heightmapAsset;
            _textureEnabled = false;
            _colorEnabled = true;
        }

        public Heightmap(string heightmapName, string textureName)
            : this(heightmapName)
        {
            _textureName = textureName;
            _textureEnabled = true;
            _colorEnabled = false;
        }

        public void LoadContent()
        {
            base.LoadContent();
            _heightmapTexture = YnG.Content.Load<Texture2D>(_heightmapAssetName);
            _basicEffect = new BasicEffect(YnG.GraphicsDevice);

            LoadHeightDatas();
            CreateVertices();
            CreateIndices();
            SetupShader();
        }

        private void LoadHeightDatas()
        {
            Width = _heightmapTexture.Width;
            Depth = _heightmapTexture.Height;

            Color[] colors = new Color[Width * Depth];
            _heightmapTexture.GetData(colors);

            _heightData = new float[Width, Depth];

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Depth; y++)
                    _heightData[x, y] = colors[x + y * Width].B / 10.0f; // Max height 25.5f
        }

        protected override void CreateVertices()
        {
            _vertices = new VertexPositionColorTexture[Width * Depth];

            Color color = Color.White;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Depth; y++)
                {
                    _vertices[x + y * Width].Position = new Vector3(x, _heightData[x, y], y);
                    _vertices[x + y * Width].TextureCoordinate = new Vector2((float)x / (float)Width, (float)y / (float)Depth);

                    if ((int)_heightData[x, y] < 3)
                        color = new Color(30, 65, 255);
                    else if ((int)_heightData[x, y] < 6)
                        color = new Color(30, 157, 255);
                    else if ((int)_heightData[x, y] < 8)
                        color = new Color(0, 147, 14);
                    else if ((int)_heightData[x, y] < 15)
                        color = new Color(38, 127, 0);
                    else
                        color = Color.LightGray;

                    _vertices[x + y * Width].Color = color;
                }
            }
        }
    }
}
