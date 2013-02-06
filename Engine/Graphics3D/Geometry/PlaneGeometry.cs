﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics3D.Geometry
{
    public class PlaneGeometry : BaseGeometry<VertexPositionNormalTexture>
    {
        public PlaneGeometry(string textureName, Vector3 sizes, Vector3 position)
        {
            _segmentSizes = sizes;
            _position = position;
            _textureName = textureName;
            _width = sizes.X;
            _height = sizes.Y;
            _depth = sizes.Z;
        }

        protected override void CreateVertices()
        {
            Vector3[] vertexPositions = new Vector3[4]
            {
                new Vector3(-1.0f, 1.0f, -1.0f),
                new Vector3(-1.0f, 1.0f, 1.0f),
                new Vector3(1.0f, 1.0f, -1.0f),
                new Vector3(1.0f, 1.0f, 1.0f)
            };

            Vector2[] textureCoordinates = new Vector2[4]
            {
                new Vector2(0.0f, 1.0f),
                new Vector2(0.0f, 0.0f),
                new Vector2(1.0f, 1.0f),
                new Vector2(1.0f, 0.0f)
            };

            _vertices = new VertexPositionNormalTexture[4];

            for (int i = 0; i < 4; i++)
            {
                _vertices[i].Position = vertexPositions[i] * _segmentSizes;
                _vertices[i].TextureCoordinate = textureCoordinates[i] * _textureRepeat;
                _vertices[i].Normal = Vector3.Up;
            }
        }

        protected override void CreateIndices()
        {
            _indices = new short[] 
            {
                0, 1, 2, 1, 3, 2
            };
        }
        
        /// <summary>
        /// Draw the plane shape
        /// </summary>
        /// <param name="device"></param>
        public override void Draw(GraphicsDevice device)
        {
            PreDraw();

            foreach (EffectPass pass in _material.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length, _indices, 0, _indices.Length / 3);
            }
        }
    }
}