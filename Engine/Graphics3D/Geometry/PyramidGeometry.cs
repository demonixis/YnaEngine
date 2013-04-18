﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Engine.Graphics3D.Geometry
{
    public class PyramidGeometry : BaseGeometry<VertexPositionNormalTexture>
    {
        public PyramidGeometry(Vector3 sizes)
        {
            _segmentSizes = sizes;
        }

        protected override void CreateVertices()
        {
            Vector2 topLeft = new Vector2(0.0f, 0.0f);
            Vector2 topRight = new Vector2(1.0f, 0.0f);
            Vector2 bottomLeft = new Vector2(0.0f, 1.0f);
            Vector2 bottomRight = new Vector2(1.0f, 1.0f);

            Vector3 normal = Vector3.Up;

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

            for (int i = 0; i < 12; i++)
            {
                _vertices[i].Position = (_vertices[i].Position + _origin) * _segmentSizes;
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

        public override void GenerateGeometry()
        {
            base.GenerateGeometry();
            ComputeNormals(ref _vertices);
        }

        /// <summary>
        /// Draw the plane shape
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
}