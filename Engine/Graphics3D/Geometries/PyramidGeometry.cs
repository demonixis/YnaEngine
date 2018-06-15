// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Materials;

namespace Yna.Engine.Graphics3D.Geometries
{
    public sealed class PyramidGeometry : Geometry
    {
        public PyramidGeometry(Vector3 sizes)
            : base()
        {
            _size = sizes;
        }

        protected override void CreateVertices()
        {
            var topLeft = new Vector2(0.0f, 0.0f);
            var topRight = new Vector2(1.0f, 0.0f);
            var bottomLeft = new Vector2(0.0f, 1.0f);
            var bottomRight = new Vector2(1.0f, 1.0f);

            var normal = Vector3.Up;

            _vertices = new VertexPositionNormalTexture[12];
            _vertices[0] = new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, 1.0f), normal, bottomRight);
            _vertices[1] = new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), normal, bottomLeft);
            _vertices[2] = new VertexPositionNormalTexture(new Vector3(0.0f, 1.0f, 0.0f), normal, topRight);

            _vertices[3] = new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, -1.0f), normal, bottomRight);
            _vertices[4] = new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, 1.0f), normal, bottomLeft);
            _vertices[5] = new VertexPositionNormalTexture(new Vector3(0.0f, 1.0f, 0.0f), normal, topRight);

            _vertices[6] = new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), normal, bottomRight);
            _vertices[7] = new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, -1.0f), normal, bottomLeft);
            _vertices[8] = new VertexPositionNormalTexture(new Vector3(0.0f, 1.0f, 0.0f), normal, topRight);

            _vertices[9] = new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), normal, bottomRight);
            _vertices[10] = new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), normal, bottomLeft);
            _vertices[11] = new VertexPositionNormalTexture(new Vector3(0.0f, 1.0f, 0.0f), normal, topRight);

            for (var i = 0; i < 12; i++)
            {
                _vertices[i].Position = (_vertices[i].Position + _origin) * _size;
                _vertices[i].TextureCoordinate *= TextureRepeat;
            }
        }

        protected override void CreateIndices()
        {
            _indices = new short[]
            {
                0, 1, 2,
                3, 4, 5,
                6, 7, 8,
                9, 10, 11
            };
        }

        public override void Generate()
        {
            base.Generate();
            ComputeNormals(ref _vertices);
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
