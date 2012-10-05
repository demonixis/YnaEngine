using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display3D.Camera;

namespace Yna.Display3D.Terrain
{
    public class Heightmap : Terrain
    {
        private string _heightmapAssetName;
        private Texture2D _heightmapTexture;
        private float[,] _heightData;

        public Heightmap(BaseCamera camera, string heightmapAsset)
            : base(camera)
        {
            _heightmapAssetName = heightmapAsset;
        }

        public Heightmap(string heightmapAsset)
            : base()
        {
            _heightmapAssetName = heightmapAsset;
        }

        public void LoadContent()
        {
            _heightmapTexture = YnG.Content.Load<Texture2D>(_heightmapAssetName);
            _basicEffect = new BasicEffect(YnG.GraphicsDevice);

            LoadHeightDatas();
            CreateVertices();
            CreateIndex();
        }

        private void LoadHeightDatas()
        {
            Width = _heightmapTexture.Width;
            Height = _heightmapTexture.Height;

            Color[] colors = new Color[Width * Height];
            _heightmapTexture.GetData(colors);

            _heightData = new float[Width, Height];

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    _heightData[x, y] = colors[x + y * Width].B / 10.0f; // Max height 25.5f
        }

        protected override void CreateVertices()
        {
            vertices = new VertexPositionColor[Width * Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    vertices[x + y * Width].Position = new Vector3(x, _heightData[x, y], y);

                    Color color;
                    int value = (int)_heightData[x, y];

                    if (value < 5)
                        color = Color.Blue;
                    else if (value < 15)
                        color = new Color(38, 127, 0);
                    else
                        color = Color.LightGray;

                    vertices[x + y * Width].Color = color;

                }
            }
        }

        protected override void CreateIndex()
        {
            _indexes = new short[(Width - 1) * (Height - 1) * 6];
            int counter = 0;

            for (int x = 0; x < Width - 1; x++)
            {
                for (int y = 0; y < Height - 1; y++)
                {
                    short lowerLeft = (short)(x + y * Width);
                    short lowerRight = (short)((x + 1) + y * Width);
                    short topLeft = (short)(x + (y + 1) * Width);
                    short topRight = (short)((x + 1) + (y + 1) * Width);

                    _indexes[counter++] = topLeft;
                    _indexes[counter++] = lowerLeft;
                    _indexes[counter++] = lowerRight;
                    _indexes[counter++] = topLeft;
                    _indexes[counter++] = lowerRight;
                    _indexes[counter++] = topRight;
                }
            }
        }
    }
}
