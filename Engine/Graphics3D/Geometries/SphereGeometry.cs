// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Materials;

namespace Yna.Engine.Graphics3D.Geometries
{
    /// <summary>
    /// Sphere geometry
    /// Inspired by Xnawiki : http://www.xnawiki.com/index.php/Shape_Generation#Sphere
    /// </summary>
    public sealed class SphereGeometry : Geometry
    {
        private float _radius;
        private int _tessellationLevel;

        #region Constructors

        public SphereGeometry(float radius = 1, int tessellationLevel = 8, bool invertFaces = false)
            : base()
        {
              _invertFaces = invertFaces;
            _radius = radius;
            _tessellationLevel = tessellationLevel;
        }

        #endregion

        protected override void CreateVertices()
        {
            if (_radius < 0)
                _radius *= -1;

            _tessellationLevel = Math.Max(4, _tessellationLevel);
            _tessellationLevel += (_tessellationLevel % 2);

            var vertexCount = 0;
            var indexCount = 0;

            _vertices = new VertexPositionNormalTexture[((_tessellationLevel / 2) * (_tessellationLevel - 1)) + 1];
            _indices = new short[(((_tessellationLevel / 2) - 2) * (_tessellationLevel + 1) * 6) + (6 * (_tessellationLevel + 1))];

            for (var j = 0; j <= _tessellationLevel / 2; j++)
            {
                var theta = j * MathHelper.TwoPi / _tessellationLevel - MathHelper.PiOver2;

                for (var i = 0; i <= _tessellationLevel; i++)
                {
                    var phi = i * MathHelper.TwoPi / _tessellationLevel;

                    _vertices[vertexCount++] = new VertexPositionNormalTexture()
                    {
                        Position = new Vector3()
                        {
                            X = (_radius * (float)(Math.Cos(theta) * Math.Cos(phi)) + _origin.X) * _size.X,
                            Y = (_radius * (float)(Math.Sin(theta)) + _origin.Y) * _size.Y,
                            Z = (_radius * (float)(Math.Cos(theta) * Math.Sin(phi)) + _origin.Z) * _size.Z
                        },
                        TextureCoordinate = new Vector2()
                        {
                            X = (i / _tessellationLevel) * TextureRepeat.X,
                            Y = (2 * j / _tessellationLevel) * TextureRepeat.Y
                        }
                    };

                    if (j == 0)
                    {
                        // bottom cap
                        for (i = 0; i <= _tessellationLevel; i++)
                        {
                            var i0 = (short)0;
                            var i1 = (short)((i % _tessellationLevel) + 1);
                            var i2 = (short)i;

                            _indices[indexCount++] = i0;
                            _indices[indexCount++] = _invertFaces ? i2 : i1;
                            _indices[indexCount++] = _invertFaces ? i1 : i2;
                        }
                    }
                    else if (j < _tessellationLevel / 2 - 1)
                    {
                        // middle area
                        var i0 = (short)(vertexCount - 1);
                        var i1 = (short)vertexCount;
                        var i2 = (short)(vertexCount + _tessellationLevel);
                        var i3 = (short)(vertexCount + _tessellationLevel + 1);

                        _indices[indexCount++] = i0;
                        _indices[indexCount++] = _invertFaces ? i2 : i1;
                        _indices[indexCount++] = _invertFaces ? i1 : i2;

                        _indices[indexCount++] = i1;
                        _indices[indexCount++] = _invertFaces ? i2 : i3;
                        _indices[indexCount++] = _invertFaces ? i3 : i2;
                    }
                    else if (j == _tessellationLevel / 2)
                    {
                        // top cap
                        for (i = 0; i <= _tessellationLevel; i++)
                        {
                            var i0 = (short)(vertexCount - 1);
                            var i1 = (short)((vertexCount - 1) - ((i % _tessellationLevel) + 1));
                            var i2 = (short)((vertexCount - 1) - i);

                            _indices[indexCount++] = i0;
                            _indices[indexCount++] = _invertFaces ? i2 : i1;
                            _indices[indexCount++] = _invertFaces ? i1 : i2;
                        }
                    }
                }
            }
        }

        protected override void CreateIndices() { }

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
