// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Material;
using Yna.Engine.Graphics3D.Terrain.Geometry;

namespace Yna.Engine.Graphics3D.Terrain
{
    /// <summary>
    /// An heightmap terrain.
    /// </summary>
    public class Heightmap : BaseTerrain
    {
        /// <summary>
        /// Create an heightmap terrain.
        /// </summary>
        /// <param name="heigtmapName">Texture name to use for height generation.</param>
        /// <param name="textureName">Texture name to use for terrain.</param>
        public Heightmap(string heigtmapName, string textureName)
            : this(heigtmapName, textureName, new Vector3(1.0f))
        {

        }

        /// <summary>
        /// Create an heightmap terrain.
        /// </summary>
        /// <param name="heigtmapName">Texture name to use for height generation.</param>
        /// <param name="textureName">Texture name to use for terrain.</param>
        /// <param name="size">Size between each vertex.</param>
        public Heightmap(string heightmapName, string textureName, Vector3 size)
        {
            _geometry = new HeightmapGeometry(heightmapName, size);
            _material = new BasicMaterial(textureName);
        }

        /// <summary>
        /// Create an heightmap terrain.
        /// </summary>
        /// <param name="heightmapTexture">Texture to use for height generation.</param>
        /// <param name="textureName">Texture name to use for terrain.</param>
        public Heightmap(Texture2D heightmapTexture, string textureName, Vector3 size)
        {
            _geometry = new HeightmapGeometry(heightmapTexture, size);
            _material = new BasicMaterial(textureName);
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

            float sizedPosX = (positionX / geometry.SegmentSizes.X) / _scale.X;
            float sizedPosY = (positionY / geometry.SegmentSizes.Y) / _scale.Y;
            float sizedPosZ = (positionZ / geometry.SegmentSizes.Z) / _scale.Z;

            int x = (int)((positionX / geometry.SegmentSizes.X) / _scale.X);
            int z = (int)((positionZ / geometry.SegmentSizes.Z) / _scale.Z);

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

            return (terrainHeigth * geometry.SegmentSizes.Y * _scale.Y);
        }
    }
}
