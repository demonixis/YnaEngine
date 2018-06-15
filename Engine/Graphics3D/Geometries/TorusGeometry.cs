// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics3D.Geometries
{
    /// <summary>
    /// Torus geometry
    /// Inspired by Xnawiki : http://www.xnawiki.com/index.php/Shape_Generation#Torus
    /// </summary>
    public sealed class TorusGeometry : Geometry
    {
        private float _radiusExterior;
        private float _raduisInterior;
        private int _nbSlices;
        private int _nbSegments;

        #region Constructors

        public TorusGeometry(float radiusExterior, float radiusInterior)
            : this(radiusExterior, radiusInterior, new Vector3(1.0f))
        {
        }

        public TorusGeometry(float radiusExterior, float radiusInterior, Vector3 sizes)
            : this(radiusExterior, radiusInterior, false, 10, 10, sizes)
        {
        }

        public TorusGeometry(float radiusExterior, float radiusInterior, bool invertFaces, int nbSlices, int nbSegments, Vector3 sizes)
            : this(radiusExterior, radiusInterior, invertFaces, nbSlices, nbSegments, sizes, new Vector3(0.0f))
        {
        }

        public TorusGeometry(float radiusExterior, float radiusInterior, bool invertFaces, int nbSlices, int nbSegments, Vector3 sizes, Vector3 origin)
            : base()
        {
            _size = sizes;
            _origin = origin;
            _radiusExterior = radiusExterior;
            _raduisInterior = radiusInterior;
            _invertFaces = invertFaces;
            _nbSlices = nbSlices;
            _nbSegments = nbSegments;
        }

        #endregion

        protected override void CreateVertices()
        {
            _nbSegments = Math.Max(3, _nbSegments);
            _nbSlices = Math.Max(3, _nbSlices);

            var invSegments = 1f / _nbSegments;
            var invSlices = 1f / _nbSlices;
            var radSegment = MathHelper.TwoPi * invSegments;
            var radSlice = MathHelper.TwoPi * invSlices;
            var lines = false;
            var indexCount = 0;

            _vertices = new VertexPositionNormalTexture[(_nbSegments + 1) * (_nbSlices + 1)];
            _indices = new short[_nbSegments * _nbSlices * (lines ? 8 : 6)];

            for (var j = 0; j <= _nbSegments; j++)
            {
                var theta = j * radSegment - MathHelper.PiOver2;
                var cosTheta = (float)Math.Cos(theta);
                var sinTheta = (float)Math.Sin(theta);

                for (var i = 0; i <= _nbSlices; i++)
                {
                    var phi = i * radSlice;
                    var cosPhi = (float)Math.Cos(phi);
                    var sinPhi = (float)Math.Sin(phi);

                    var position = new Vector3()
                    {
                        X = cosTheta * (_radiusExterior + _raduisInterior * cosPhi),
                        Y = _raduisInterior * sinPhi,
                        Z = sinTheta * (_radiusExterior + _raduisInterior * cosPhi)
                    };

                    var center = new Vector3()
                    {
                        X = _radiusExterior * cosTheta,
                        Y = 0,
                        Z = _radiusExterior * sinTheta
                    };

                    _vertices[(j * (_nbSlices + 1)) + i] = new VertexPositionNormalTexture()
                    {
                        Position = position,
                        Normal = Vector3.Normalize(position - center),
                        TextureCoordinate = new Vector2(j * invSegments, i * invSegments) * _UVOffset
                    };

                    // 0---2
                    // | \ |
                    // 1---3
                    if (j < _nbSegments && i < _nbSlices)
                    {
                        var i0 = (short)((j * (_nbSlices + 1)) + i);
                        var i1 = (short)((j * (_nbSlices + 1)) + i + 1);
                        var i2 = (short)(((j + 1) * (_nbSlices + 1)) + i);
                        var i3 = (short)(((j + 1) * (_nbSlices + 1)) + i + 1);

                        _indices[indexCount++] = i0;
                        _indices[indexCount++] = _invertFaces ? i1 : i3;
                        _indices[indexCount++] = _invertFaces ? i3 : i1;

                        _indices[indexCount++] = i0;
                        _indices[indexCount++] = _invertFaces ? i3 : i2;
                        _indices[indexCount++] = _invertFaces ? i2 : i3;
                    }
                }
            }
        }

        protected override void CreateIndices() { }

        public override void Draw(GraphicsDevice device, Materials.Material material)
        {
            DrawUserIndexedPrimitives(device, material);
        }
    }
}
