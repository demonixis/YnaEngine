// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Engine.Graphics3D.Geometry
{
    /// <summary>
    /// Sphere geometry
    /// Inspired by Xnawiki : http://www.xnawiki.com/index.php/Shape_Generation#Sphere
    /// </summary>
    public class SphereGeometry : BaseGeometry<VertexPositionNormalTexture>
    {
        private float _radius;
        private int _tessellationLevel;

        public float Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        public int TessellationLevel
        {
            get { return _tessellationLevel; }
            set { _tessellationLevel = value; }
        }

        #region Constructors

        public SphereGeometry(float radius)
            : this(radius, false, 10)
        {

        }

        public SphereGeometry(float radius, bool invertFaces, int tessellationLevel)
            : this(radius, invertFaces, tessellationLevel, new Vector3(1.0f))
        {

        }

        public SphereGeometry(float radius, bool invertFaces, int tessellationLevel, Vector3 sizes)
            : this (radius, invertFaces, tessellationLevel, sizes, new Vector3(0.0f))
        {

        }

        public SphereGeometry(float radius, bool invertFaces, int tessellationLevel, Vector3 sizes, Vector3 origin)
            : base()
        {
            _segmentSizes = sizes;
            _radius = radius;
            _invertFaces = invertFaces;
            _tessellationLevel = tessellationLevel;
        }

        #endregion

        protected override void CreateVertices()
        {
            if (_radius < 0)
                _radius *= -1;

            _tessellationLevel = Math.Max(4, _tessellationLevel);
            _tessellationLevel += (_tessellationLevel % 2);

            int vertexCount = 0;
            int indexCount = 0;

            _vertices = new VertexPositionNormalTexture[((_tessellationLevel / 2) * (_tessellationLevel - 1)) + 1];
            _indices = new short[(((_tessellationLevel / 2) - 2) * (_tessellationLevel + 1) * 6) + (6 * (_tessellationLevel + 1))];

            for (int j = 0; j <= _tessellationLevel / 2; j++)
            {
                float theta = j * MathHelper.TwoPi / _tessellationLevel - MathHelper.PiOver2;

                for (int i = 0; i <= _tessellationLevel; i++)
                {
                    float phi = i * MathHelper.TwoPi / _tessellationLevel;

                    _vertices[vertexCount++] = new VertexPositionNormalTexture()
                    {
                        Position = new Vector3()
                        {
                            X = (_radius * (float)(Math.Cos(theta) * Math.Cos(phi)) + _origin.X) * _segmentSizes.X,
                            Y = (_radius * (float)(Math.Sin(theta)) + _origin.Y) * _segmentSizes.Y,
                            Z = (_radius * (float)(Math.Cos(theta) * Math.Sin(phi)) + _origin.Z) * _segmentSizes.Z
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
                            short i0 = 0;
                            short i1 = (short)((i % _tessellationLevel) + 1);
                            short i2 = (short)i;

                            _indices[indexCount++] = i0;
                            _indices[indexCount++] = _invertFaces ? i2 : i1;
                            _indices[indexCount++] = _invertFaces ? i1 : i2;
                        }
                    }
                    else if (j < _tessellationLevel / 2 - 1)
                    {
                        // middle area
                        short i0 = (short)(vertexCount - 1);
                        short i1 = (short)vertexCount;
                        short i2 = (short)(vertexCount + _tessellationLevel);
                        short i3 = (short)(vertexCount + _tessellationLevel + 1);

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
                            short i0 = (short)(vertexCount - 1);
                            short i1 = (short)((vertexCount - 1) - ((i % _tessellationLevel) + 1));
                            short i2 = (short)((vertexCount - 1) - i);

                            _indices[indexCount++] = i0;
                            _indices[indexCount++] = _invertFaces ? i2 : i1;
                            _indices[indexCount++] = _invertFaces ? i1 : i2;
                        }
                    }
                }
            }
        }

        protected override void CreateIndices()
        {

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
            DrawUserIndexedPrimitives(device, material);
        }
    }
}
