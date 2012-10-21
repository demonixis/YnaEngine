﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display3D.Camera;

namespace Yna.Display3D.Terrain
{
    public class SimpleTerrain : BaseTerrain
    {
        public SimpleTerrain(string textureName, int width, int depth)
        {
            _width = width;
            _height = 0;
            _depth = depth;
            _textureName = textureName;
            _textureEnabled = true;
            _colorEnabled = false;
            _segmentSizes = new Vector3(5.0f, 0.0f, 5.0f);
        }

        public SimpleTerrain(string textureName, int width, int depth, int segmentX, int segmentZ)
            : this (textureName, width, depth)
        {
            _segmentSizes = new Vector3(segmentX, 0, segmentZ);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            CreateVertices();
            CreateIndices();
            SetupShader();

            UpdateBoundingVolumes();
        }

        protected override void CreateVertices()
        {
            _vertices = new VertexPositionColorTexture[Width * Depth];

            Random random = new Random(DateTime.Now.Millisecond);

            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    _vertices[x + z * Width].Position = new Vector3(
                        (_position.X + x) * _segmentSizes.X, 
                        (_position.Y + 0) * _segmentSizes.Y, 
                        (_position.Z + z) * _segmentSizes.Z);

                    _vertices[x + z * Width].TextureCoordinate = new Vector2(
                        ((float)x / (float)Width) * _textureRepeat.X, 
                        ((float)z / (float)Depth) * _textureRepeat.Y);

                    _vertices[x + z * Width].Color = new Color(0, 147, 14);
                }
            }
        }
    }
}
