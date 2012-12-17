using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D.Camera;

namespace Yna.Framework.Display3D.Terrain
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

        public Heightmap(string heightmapName, string textureName, Vector3 segmentSizes)
            : this (heightmapName, textureName)
        {
            _segmentSizes = segmentSizes;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _heightmapTexture = YnG.Content.Load<Texture2D>(_heightmapAssetName);

            LoadHeightDatas();
            CreateVertices();
            CreateIndices();
            ComputeNormals();

            SetupShader();

            UpdateBoundingVolumes();
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

        #region Terrain construction

        protected override void CreateVertices()
        {
            _vertices = new VertexPositionNormalTexture[Width * Depth];

            Color color = Color.White;

            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    _vertices[x + z * Width].Position = new Vector3(
                        (_position.X + x) * _segmentSizes.X,
                        (_position.Y +  _heightData[x, z]) * _segmentSizes.Y,
                        (_position.Z + z) * _segmentSizes.Z);

                    _vertices[x + z * Width].TextureCoordinate = new Vector2(
                        (float)x / (float)Width, 
                        (float)z / (float)Depth);

                    _vertices[x + z * Width].Normal = Vector3.Zero;
                }
            }
        }

        #endregion

        public float GetTerrainHeight(float positionX, float positionY, float positionZ)
        {
            float terrainHeigth = 0.0f;

            float sizedPosX = (positionX / _segmentSizes.X) / _scale.X;
            float sizedPosY = (positionY / _segmentSizes.Y) / _scale.Y;
            float sizedPosZ = (positionZ / _segmentSizes.Z) / _scale.Z;

            int x = (int)((positionX / _segmentSizes.X) / _scale.X);
            int z = (int)((positionZ / _segmentSizes.Z) / _scale.Z);

            if (x < 0 || x >= _width - 1 || z < 0 || z >= _depth - 1)
                terrainHeigth = positionY;
            else
            {
                float triangleY0 = _heightData[x, z];
                float triangleY1 = _heightData[x + 1, z];
                float triangleY2 = _heightData[x, z + 1];
                float triangleY3 = _heightData[x + 1, z + 1];

                // Determine where are the point
                float segX = sizedPosX - x;
                float segZ = sizedPosZ - z;

                // We are on the first triangle
                if ((segX + segZ) < 1)
                {
                    terrainHeigth = triangleY0;
                    terrainHeigth += (triangleY1 - triangleY0) * segX;
                    terrainHeigth += (triangleY2 - triangleY0) * segZ;
                }
                else // Second triangle
                {
                    terrainHeigth = triangleY3;
                    terrainHeigth += (triangleY1 - triangleY3) * segX;
                    terrainHeigth += (triangleY2 - triangleY3) * segZ;
                }
            }

            return (terrainHeigth * _segmentSizes.Y * _scale.Y);
        }
    }
}
