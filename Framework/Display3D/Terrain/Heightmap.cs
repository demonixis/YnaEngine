﻿using System;
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

        private Heightmap(string heightmapAsset)
            : base()
        {
            _heightmapAssetName = heightmapAsset;
        }

        public Heightmap(string heightmapName, string textureName)
            : this(heightmapName)
        {
            _textureName = textureName;
        }

        public Heightmap(string heightmapName, string textureName, Vector3 segmentSizes)
            : this (heightmapName, textureName)
        {
            _segmentSizes = segmentSizes;
        }

        public Heightmap(Texture2D heightmapTexture, string textureName)
        {
            _heightmapAssetName = heightmapTexture.Name;
            _heightmapTexture = heightmapTexture;
            _textureName = textureName;
        }

        public Heightmap(Texture2D heightmapTexture, string textureName, Vector3 segmentSizes)
            : this(heightmapTexture, textureName)
        {
            _segmentSizes = segmentSizes;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (_heightmapTexture == null)
                _heightmapTexture = YnG.Content.Load<Texture2D>(_heightmapAssetName);

            LoadHeightDatas();

            GenerateShape();

            ComputeNormals(ref _vertices);

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
                    _heightData[x, y] = colors[x + y * Width].R / 10.0f; // Max height 25.5f
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
                        x * _segmentSizes.X,
                        _heightData[x, z] * _segmentSizes.Y,
                        _position.Z + z * _segmentSizes.Z);

                    _vertices[x + z * Width].TextureCoordinate = new Vector2(
                        (float)x / (float)Width * _textureRepeat.X,
                        (float)z / (float)Depth * _textureRepeat.Y);

                    _vertices[x + z * Width].Normal = Vector3.Zero;
                }
            }
        }

        #endregion

        /// <summary>
        /// Get the terrain height at the specified position
        /// </summary>
        /// <param name="positionX">X value</param>
        /// <param name="positionY">Y value</param>
        /// <param name="positionZ">Z value</param>
        /// <returns></returns>
        public virtual float GetTerrainHeight(float positionX, float positionY, float positionZ)
        {
            float terrainHeigth = 0.0f;

            float sizedPosX = (positionX / _segmentSizes.X) / _scale.X;
            float sizedPosY = (positionY / _segmentSizes.Y) / _scale.Y;
            float sizedPosZ = (positionZ / _segmentSizes.Z) / _scale.Z;

            int x = (int)((positionX / _segmentSizes.X) / _scale.X);
            int z = (int)((positionZ / _segmentSizes.Z) / _scale.Z);

            if (x < 0 || x >= _heightData.GetLength(0) - 1 || z < 0 || z >= _heightData.GetLength(1) - 1)
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
