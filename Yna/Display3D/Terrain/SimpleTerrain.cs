using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display3D.Camera;

namespace Yna.Display3D.Terrain
{
    public class SimpleTerrain : BaseTerrain
    {
        public SimpleTerrain(int width, int depth, string textureName)
        {
            _width = width;
            _height = 0;
            _depth = depth;
            _textureName = textureName;
            _textureEnabled = true;
            _colorEnabled = false;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            CreateVertices();
            CreateIndices();
            SetupShader();
        }

        protected override void CreateVertices()
        {
            _vertices = new VertexPositionColorTexture[Width * Depth];

            Random random = new Random(DateTime.Now.Millisecond);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Depth; y++)
                {
                    _vertices[x + y * Width].Position = new Vector3(x, 0, y);
                    _vertices[x + y * Width].TextureCoordinate = new Vector2(
                        ((float)x / (float)Width) * _textureRepeat.X, 
                        ((float)y / (float)Depth) * _textureRepeat.Y);
                    _vertices[x + y * Width].Color = new Color(0, 147, 14);
                }
            }
        }
    }
}
