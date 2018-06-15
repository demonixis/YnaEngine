// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics3D.Geometries
{
    public abstract class TerrainGeometry : Geometry
    {
        protected float _width;
        protected float _height;
        protected float _depth;

        public int Width
        {
            get => (int)_width;
            protected set => _width = value;
        }

        public int Height
        {
            get => (int)_height;
            protected set => _height = value;
        }

        public int Depth
        {
            get => (int)_depth;
            protected set => _depth = value;
        }

        /// <summary>
        /// Create indices with vertex array
        /// </summary>
        protected override void CreateIndices()
        {
            _indices = new short[((int)Width - 1) * ((int)Depth - 1) * 6];

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
            _vertices[x + z * (int)Width].Position.Y += deltaY;

            // TODO : compute vertex normal only for this vertex
            ComputeNormals(ref _vertices);
        }

        /// <summary>
        /// Draw Terrain
        /// </summary>
        /// <param name="device"></param>
        public override void Draw(GraphicsDevice device, Materials.Material material)
        {
            device.SetVertexBuffer(_vertexBuffer);
            device.Indices = _indexBuffer;

            foreach (EffectPass pass in material.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length, _indices, 0, _indices.Length / 3);
            }

            device.SetVertexBuffer(null);
            device.Indices = null;
        }
    }
}
