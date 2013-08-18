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
    /// Torus geometry
    /// Inspired by Xnawiki : http://www.xnawiki.com/index.php/Shape_Generation#Torus
    /// </summary>
    public class TorusGeometry : BaseGeometry<VertexPositionNormalTexture>
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
            _segmentSizes = sizes;
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

            float invSegments = 1f / _nbSegments, invSlices = 1f / _nbSlices;
            float radSegment = MathHelper.TwoPi * invSegments;
            float radSlice = MathHelper.TwoPi * invSlices;
            bool lines = false;

            int indexCount = 0;
            _vertices = new VertexPositionNormalTexture[(_nbSegments + 1) * (_nbSlices + 1)];
            _indices = new short[_nbSegments * _nbSlices * (lines ? 8 : 6)];

            for (int j = 0; j <= _nbSegments; j++)
            {
                float theta = j * radSegment - MathHelper.PiOver2;
                float cosTheta = (float)Math.Cos(theta), sinTheta = (float)Math.Sin(theta);

                for (int i = 0; i <= _nbSlices; i++)
                {
                    float phi = i * radSlice;
                    float cosPhi = (float)Math.Cos(phi);
                    float sinPhi = (float)Math.Sin(phi);

                    Vector3 position = new Vector3()
                    {
                        X = cosTheta * (_radiusExterior + _raduisInterior * cosPhi),
                        Y = _raduisInterior * sinPhi,
                        Z = sinTheta * (_radiusExterior + _raduisInterior * cosPhi)
                    };
                    Vector3 center = new Vector3()
                    {
                        X = _radiusExterior * cosTheta,
                        Y = 0,
                        Z = _radiusExterior * sinTheta
                    };

                    _vertices[(j * (_nbSlices + 1)) + i] = new VertexPositionNormalTexture()
                    {
                        Position = position,
                        Normal = Vector3.Normalize(position - center),
                        TextureCoordinate = new Vector2(j * invSegments, i * invSegments) * _textureRepeat
                    };

                    // 0---2
                    // | \ |
                    // 1---3
                    if (j < _nbSegments && i < _nbSlices)
                    {
                        short i0 = (short)((j * (_nbSlices + 1)) + i);
                        short i1 = (short)((j * (_nbSlices + 1)) + i + 1);
                        short i2 = (short)(((j + 1) * (_nbSlices + 1)) + i);
                        short i3 = (short)(((j + 1) * (_nbSlices + 1)) + i + 1);

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

        protected override void CreateIndices()
        {

        }

        public override void Draw(GraphicsDevice device, BaseMaterial material)
        {
            DrawUserIndexedPrimitives(device, material);
        }
    }
}
