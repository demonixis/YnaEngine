// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Materials;

namespace Yna.Engine.Graphics3D.Geometries
{
    /// <summary>
    /// Plane geometry
    /// </summary>
    public sealed class PlaneGeometry : Geometry
    {
        /// <summary>
        /// Create a plane geometry.
        /// </summary>
        /// <param name="size">Size of geometry.</param>
        /// <param name="invertFaces">Sets to true for invert faces.</param>
        public PlaneGeometry(Vector3 size, bool invertFaces = false)
            : base()
        {
            _size = size;
            _invertFaces = invertFaces;
        }

        protected override void CreateVertices()
        {
            var vertexPositions = new Vector3[4]
            {
                new Vector3(-1.0f, 0.0f, -1.0f),
                new Vector3(-1.0f, 0.0f, 1.0f),
                new Vector3(1.0f, 0.0f, -1.0f),
                new Vector3(1.0f, 0.0f, 1.0f)
            };

            var textureCoordinates = new Vector2[4]
            {
                new Vector2(0.0f, 1.0f),
                new Vector2(0.0f, 0.0f),
                new Vector2(1.0f, 1.0f),
                new Vector2(1.0f, 0.0f)
            };

            _vertices = new VertexPositionNormalTexture[4];

            for (var i = 0; i < 4; i++)
            {
                _vertices[i].Position = (vertexPositions[i] + _origin) * _size;
                _vertices[i].TextureCoordinate = textureCoordinates[i] * _UVOffset;
                _vertices[i].Normal = Vector3.Up;
            }
        }

        protected override void CreateIndices()
        {
            _indices = new short[6];
            _indices[0] = 0;
            _indices[1] = (short)(_invertFaces ? 1 : 2);
            _indices[2] = (short)(_invertFaces ? 2 : 1);
            _indices[3] = 1;
            _indices[4] = (short)(_invertFaces ? 3 : 2);
            _indices[5] = (short)(_invertFaces ? 2 : 3);
        }

        protected override void CreateBuffers()
        {
            ComputeNormals(ref _vertices);
            base.CreateBuffers();
        }
        /// <summary>
        /// Draw the plane shape
        /// </summary>
        /// <param name="device"></param>
        public override void Draw(GraphicsDevice device, Material material)
        {
            DrawUserIndexedPrimitives(device, material);
        }
    }
}
