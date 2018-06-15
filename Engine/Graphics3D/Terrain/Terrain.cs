// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Geometries;
using Yna.Engine.Graphics3D.Materials;

namespace Yna.Engine.Graphics3D.Terrains
{
    /// <summary>
    /// Abstract class that represent a basic Terrain
    /// </summary>
    public class Terrain : YnMeshGeometry
    {
        /// <summary>
        /// Create a flat terrain
        /// </summary>
        /// <param name="textureName"></param>
        /// <param name="width"></param>
        /// <param name="depth"></param>
        /// <param name="segmentX"></param>
        /// <param name="segmentZ"></param>
        public Terrain(string textureName, int width = 100, int depth = 100, int segmentX = 1, int segmentZ = 1)
        {
            _width = width;
            _depth = depth;
            _geometry = new SimpleTerrainGeometry(width, 0, depth, new Vector3(segmentX, 0, segmentZ));
            _material = new BasicMaterial(textureName);
        }

        /// <summary>
        /// Create a terrain with a heightmap.
        /// </summary>
        /// <param name="heigtmapName">Texture name to use for height generation.</param>
        /// <param name="textureName">Texture name to use for terrain.</param>
        public Terrain(string heigtmapName, string textureName)
            : this(heigtmapName, textureName, new Vector3(1.0f))
        {
        }

        /// <summary>
        /// Create a terrain with a heightmap.
        /// </summary>
        /// <param name="heigtmapName">Texture name to use for height generation.</param>
        /// <param name="textureName">Texture name to use for terrain.</param>
        /// <param name="size">Size between each vertex.</param>
        public Terrain(string heightmapName, string textureName, Vector3 size)
        {
            _geometry = new HeightmapGeometry(heightmapName, size);
            _material = new BasicMaterial(textureName);
        }

        /// <summary>
        /// Create a terrain with a heightmap.
        /// </summary>
        /// <param name="heightmapTexture">Texture to use for height generation.</param>
        /// <param name="textureName">Texture name to use for terrain.</param>
        public Terrain(Texture2D heightmapTexture, string textureName, Vector3 size)
        {
            _geometry = new HeightmapGeometry(heightmapTexture, size);
            _material = new BasicMaterial(textureName);
        }

        public override void LoadContent()
        {
            _geometry.Generate();
            _material.LoadContent();
            _initialized = true;

            UpdateBoundingVolumes();
        }

        public override void UpdateBoundingVolumes()
        {
            // Reset bounding box to min/max values
            _boundingBox.Min = new Vector3(float.MaxValue);
            _boundingBox.Max = new Vector3(float.MinValue);

            for (int i = 0; i < _geometry.Vertices.Length; i++)
            {
                _boundingBox.Min.X = Math.Min(_boundingBox.Min.X, _geometry.Vertices[i].Position.X);
                _boundingBox.Min.Y = Math.Min(_boundingBox.Min.Y, _geometry.Vertices[i].Position.Y);
                _boundingBox.Min.Z = Math.Min(_boundingBox.Min.Z, _geometry.Vertices[i].Position.Z);
                _boundingBox.Max.X = Math.Max(_boundingBox.Max.X, _geometry.Vertices[i].Position.X);
                _boundingBox.Max.Y = Math.Max(_boundingBox.Max.Y, _geometry.Vertices[i].Position.Y);
                _boundingBox.Max.Z = Math.Max(_boundingBox.Max.Z, _geometry.Vertices[i].Position.Z);
            }

            // Apply scale on the object
            _boundingBox.Min *= _scale;
            _boundingBox.Max *= _scale;

            // Update size of the object
            _width = _boundingBox.Max.X - _boundingBox.Min.X;
            _height = _boundingBox.Max.Y - _boundingBox.Min.Y;
            _depth = _boundingBox.Max.Z - _boundingBox.Min.Z;

            // Update bouding sphere
            _boundingSphere.Center.X = X + (_width / 2);
            _boundingSphere.Center.Y = Y + (_height / 2);
            _boundingSphere.Center.Z = Z + (_depth / 2);
            _boundingSphere.Radius = Math.Max(Math.Max(_width, _height), _depth) / 2;
        }

        /// <summary>
        /// Get the terrain height at the specified position
        /// </summary>
        /// <param name="positionX">X value</param>
        /// <param name="positionY">Y value</param>
        /// <param name="positionZ">Z value</param>
        /// <returns></returns>
        public virtual float GetTerrainHeight(float positionX, float positionY, float positionZ)
        {
            var geometry = _geometry as HeightmapGeometry;

            float terrainHeigth = 0.0f;

            float sizedPosX = (positionX / geometry.Size.X) / _scale.X;
            float sizedPosY = (positionY / geometry.Size.Y) / _scale.Y;
            float sizedPosZ = (positionZ / geometry.Size.Z) / _scale.Z;

            int x = (int)((positionX / geometry.Size.X) / _scale.X);
            int z = (int)((positionZ / geometry.Size.Z) / _scale.Z);

            if (x < 0 || x >= geometry.HeightmapData.GetLength(0) - 1 || z < 0 || z >= geometry.HeightmapData.GetLength(1) - 1)
                terrainHeigth = positionY;
            else
            {
                float triangleY0 = geometry.HeightmapData[x, z];
                float triangleY1 = geometry.HeightmapData[x + 1, z];
                float triangleY2 = geometry.HeightmapData[x, z + 1];
                float triangleY3 = geometry.HeightmapData[x + 1, z + 1];

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

            return (terrainHeigth * geometry.Size.Y * _scale.Y);
        }
    }
}
