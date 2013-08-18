// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Engine.Graphics3D.Terrain.Geometry
{
    public abstract class BaseTerrainGeometry : BaseGeometry<VertexPositionNormalTexture>
    {
        protected float _width;
        protected float _height;
        protected float _depth;

        public int Width
        {
            get { return (int)_width; }
            protected set { _width = value; }
        }

        public int Height
        {
            get { return (int)_height; }
            protected set { _height = value; }
        }

        public int Depth
        {
            get { return (int)_depth; }
            protected set { _depth = value; }
        }

        public BaseTerrainGeometry()
            : this(0, 0, 0)
        {

        }

        public BaseTerrainGeometry(float width, float height, float depth)
            : this(width, height, depth, new Vector3(0.0f))
        {

        }

        public BaseTerrainGeometry(float width, float height, float depth, Vector3 segmentSize)
            : base(segmentSize)
        {
            _width = width;
            _height = height;
            _depth = depth;
        }

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

        /// <summary>
        /// Draw Terrain
        /// </summary>
        /// <param name="device"></param>
        public override void Draw(GraphicsDevice device, BaseMaterial material)
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
