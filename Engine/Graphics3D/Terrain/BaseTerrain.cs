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
    internal class BaseTerrainGeometry : BaseGeometry<VertexPositionNormalTexture>
    {
        /// <summary>
        /// Create indices with vertex array
        /// </summary>
        protected override void CreateIndices()
        {
            _indices = new short[(Width - 1) * (Depth - 1) * 6];

            int counter = 0;

            for (int x = 0; x < Width - 1; x++)
            {
                for (int y = 0; y < Depth - 1; y++)
                {
                    short lowerLeft = (short)(x + y * Width);
                    short lowerRight = (short)((x + 1) + y * Width);
                    short topLeft = (short)(x + (y + 1) * Width);
                    short topRight = (short)((x + 1) + (y + 1) * Width);

                    _indices[counter++] = topLeft;
                    _indices[counter++] = lowerLeft;
                    _indices[counter++] = lowerRight;
                    _indices[counter++] = topLeft;
                    _indices[counter++] = lowerRight;
                    _indices[counter++] = topRight;
                }
            }
        }

        public virtual void MoveVertex(int x, int z, float deltaY)
        {
            _vertices[x + z * Width].Position.Y += deltaY;

            // TODO : compute vertex normal only for this vertex
            ComputeNormals(ref _vertices);
        }

        #region Bounding volumes

        

        #endregion

        /// <summary>
        /// Draw Terrain
        /// </summary>
        /// <param name="device"></param>
        public override void Draw(GraphicsDevice device, BaseMaterial material)
        {
            foreach (EffectPass pass in material.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length, _indices, 0, _indices.Length / 3);
            }
        }
    }

    /// <summary>
    /// Abstract class that represent a basic Terrain
    /// </summary>
    public abstract class BaseTerrain : YnEntity3D
    {
        private YnGeometryMesh<VertexPositionNormalTexture> _mesh;
        private BaseTerrainGeometry _geometry;

        /// <summary>
        /// Basic initialization for an abstract terrain
        /// </summary>
        public BaseTerrain()
            : base()
        {
            _geometry = new BaseTerrainGeometry();
            _mesh = new YnGeometryMesh<VertexPositionNormalTexture>(_geometry);
        }

        public override void Draw(GraphicsDevice device)
        {
            _mesh.Draw(device);
        }
    }
}
