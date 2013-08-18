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
    /// Cylinder geometry
    /// Inspired by Xnawiki : http://www.xnawiki.com/index.php/Shape_Generation#Cylinder
    /// </summary>
    public class CylinderGeometry : BaseGeometry<VertexPositionNormalTexture>
    {
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private float _startRadius;
        private float _endRadius;
        private int _nbSegments;
        private int _nbSlices;
        private static Random random = new Random();

        public CylinderGeometry(Vector3 startPosition, Vector3 endPosition, float startRadius, float endRadius, bool invertFaces, int nbSegments, int nbSlices, Vector3 sizes)
            : base()
        {
            _segmentSizes = sizes;
            _startPosition = startPosition;
            _endPosition = endPosition;
            _startRadius = startRadius;
            _endRadius = endRadius;
            _invertFaces = invertFaces;
            _nbSegments = nbSegments;
            _nbSlices = nbSlices;
        }

        protected override void CreateVertices()
        {
            _nbSegments = Math.Max(1, _nbSegments);
            _nbSlices = Math.Max(3, _nbSlices);

            // this vector should not be between start and end
            Vector3 p = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());

            // derive two points on the plane formed by [end - start]
            Vector3 r = Vector3.Cross(p - _startPosition, _endPosition - _startPosition);
            Vector3 s = Vector3.Cross(r, _endPosition - _startPosition);
            r.Normalize();
            s.Normalize();

            int vertexCount = 0, indexCount = 0;
            float invSegments = 1f / _nbSegments;
            float invSlices = 1f / _nbSlices;

            _vertices = new VertexPositionNormalTexture[((_nbSegments + 1) * (_nbSlices + 1)) + 2];
            _indices = new short[(_nbSlices + (_nbSlices * _nbSegments)) * 6];

            for (int j = 0; j <= _nbSegments; j++)
            {
                Vector3 center = Vector3.Lerp(_startPosition, _endPosition, j * invSegments);
                float radius = MathHelper.Lerp(_startRadius, _endRadius, j * invSegments);

                if (j == 0)
                {
                    _vertices[vertexCount++] = new VertexPositionNormalTexture()
                    {
                        Position = (_origin + center) * _segmentSizes,
                        TextureCoordinate = new Vector2(0.5f, j * invSegments) * _textureRepeat
                    };
                }

                for (int i = 0; i <= _nbSlices; i++)
                {
                    float theta = i * MathHelper.TwoPi * invSlices;
                    float rCosTheta = radius * (float)Math.Cos(theta);
                    float rSinTheta = radius * (float)Math.Sin(theta);

                    _vertices[vertexCount++] = new VertexPositionNormalTexture()
                    {
                        Position = new Vector3()
                        {
                            X = (_origin.X + center.X + rCosTheta * r.X + rSinTheta * s.X) * _segmentSizes.X,
                            Y = (_origin.Y + center.Y + rCosTheta * r.Y + rSinTheta * s.Y) * _segmentSizes.Y,
                            Z = (_origin.Z + center.Z + rCosTheta * r.Z + rSinTheta * s.Z) * _segmentSizes.Z
                        },
                        TextureCoordinate = new Vector2()
                        {
                            X = i * invSlices * _textureRepeat.X,
                            Y = j * invSegments * _textureRepeat.Y
                        }
                    };

                    if (i < _nbSlices)
                    {
                        // just an alias to assist with think of each vertex that's
                        //  iterated in here as the bottom right corner of a triangle
                        int vRef = vertexCount - 1;

                        if (j == 0)
                        {   
                            // start cap - i0 is always center point on start cap
                            short i0 = 0;
                            short i1 = (short)(vRef + 1);
                            short i2 = (short)(vRef);

                            _indices[indexCount++] = i0;
                            _indices[indexCount++] = _invertFaces ? i2 : i1;
                            _indices[indexCount++] = _invertFaces ? i1 : i2;
                        }
                        if (j == _nbSegments)
                        {   
                            // end cap - i0 is always the center point on end cap
                            short i0 = (short)((vRef + _nbSlices + 2) - (vRef % (_nbSlices + 1)));
                            short i1 = (short)(vRef);
                            short i2 = (short)(vRef + 1);

                            _indices[indexCount++] = i0;
                            _indices[indexCount++] = _invertFaces ? i2 : i1;
                            _indices[indexCount++] = _invertFaces ? i1 : i2;
                        }

                        if (j < _nbSegments)
                        {   
                            // middle area
                            short i0 = (short)(vRef);
                            short i1 = (short)(vRef + 1);
                            short i2 = (short)(vRef + _nbSlices + 2);
                            short i3 = (short)(vRef + _nbSlices + 1);

                            _indices[indexCount++] = i0;
                            _indices[indexCount++] = _invertFaces ? i2 : i1;
                            _indices[indexCount++] = _invertFaces ? i1 : i2;

                            _indices[indexCount++] = i0;
                            _indices[indexCount++] = _invertFaces ? i3 : i2;
                            _indices[indexCount++] = _invertFaces ? i2 : i3;
                        }
                    }
                }

                if (j == _nbSegments)
                {
                    _vertices[vertexCount++] = new VertexPositionNormalTexture()
                    {
                        Position = (center + _origin) * _segmentSizes,
                        TextureCoordinate = new Vector2(0.5f, j * invSegments) * _textureRepeat
                    };
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
