using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Engine.Graphics3D.Terrain
{
    public class SimpleTerrainGeometry : BaseTerrainGeometry
    {
        public SimpleTerrainGeometry()
        {
            _segmentSizes = new Vector3(5.0f, 0.0f, 5.0f);
        }

        public SimpleTerrainGeometry(Vector3 size)
        {
            _segmentSizes = size;
        }

        protected override void CreateVertices()
        {
            _vertices = new VertexPositionNormalTexture[Width * Depth];

            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    _vertices[x + z * Width].Position = new Vector3(
                        x * _segmentSizes.X,
                        0 * _segmentSizes.Y,
                        z * _segmentSizes.Z);

                    _vertices[x + z * Width].TextureCoordinate = new Vector2(
                        ((float)x / (float)Width) * _textureRepeat.X,
                        ((float)z / (float)Depth) * _textureRepeat.Y);

                    _vertices[x + z * Width].Normal = Vector3.Zero;
                }
            }
        }
    }
}
