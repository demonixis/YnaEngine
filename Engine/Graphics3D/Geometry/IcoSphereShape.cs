// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Engine.Graphics3D.Geometry
{
    internal class TriangleData
    {
        public int A;
        public int B;
        public int C;
        public Vector2 UVA;
        public Vector2 UVB;
        public Vector2 UVC;

        public TriangleData(int a, int b, int c, Vector2 uva, Vector2 uvb, Vector2 uvc)
        {
            A = a;
            B = b;
            C = c;
            UVA = uva;
            UVB = uvb;
            UVC = uvc;
        }
    }

    public class IcoSphereGeometry : BaseGeometry<VertexPositionNormalTexture>
    {
        private float _radius;
        private int _lod;
        private List<Vector3> _vertexPositions;
        private List<TriangleData> _triangles;
        private Dictionary<Int64, int> _cache;

        public IcoSphereGeometry(float radius, int lod, bool invertFaces)
            : base()
        {
            _radius = radius;
            _lod = lod;
            _invertFaces = invertFaces;
            _vertexPositions = new List<Vector3>();
            _triangles = new List<TriangleData>();
        }

        protected override void CreateVertices()
        {
            CreateVertexData();
            ApplyVertexData();
            for (int i = 0; i < _vertices.Length; i++)
                _vertices[i].TextureCoordinate *= _textureRepeat;
        }

        protected override void CreateIndices()
		{
			// Indices are calculated in ApplyVertexData
		}

        /// <summary>
        /// Compute the vertices created in CreateVertexData into usable VertexPositionNormalTexture
        /// </summary>
        private void ApplyVertexData()
        {
            _vertices = new VertexPositionNormalTexture[_triangles.Count * 3];
            _indices = new short[_triangles.Count * 3];

            Vector2 textureCoordinate = Vector2.One;
            Vector3 vertexNormal;
            int idx = 0;
            foreach (TriangleData triangle in _triangles)
            {
                Vector3 p1 = _vertexPositions[triangle.A];
                Vector3 p2 = _vertexPositions[triangle.B];
                Vector3 p3 = _vertexPositions[triangle.C];

                //vertexNormal = ComputeNormal(p1, p2, p3);
                vertexNormal = new Vector3(0, -1, 0);

                // Create the "real" vertices
                _vertices[idx] = new VertexPositionNormalTexture(p1, vertexNormal, triangle.UVA);
                _indices[idx++] = (short)triangle.A;

                _vertices[idx] = new VertexPositionNormalTexture(p2, vertexNormal, triangle.UVB);
                _indices[idx++] = (short)triangle.B;

                _vertices[idx] = new VertexPositionNormalTexture(p3, vertexNormal, triangle.UVC);
                _indices[idx++] = (short)triangle.C;

            }
        }

        /// <summary>
        /// Compute 3D position into 2D UV position
        /// </summary>
        /// <param name="point">The 3D sphere point</param>
        /// <returns>The 2D UV coordinate (x = U; y = V)</returns>
        private Vector2 TextureCoordinate(Vector3 point)
        {
            float u = 0f;
            float v = 0f;

            if (point.Z == 0)
            {
                u = (float)(point.X * Math.PI / 2);
            }
            else
            {
                u = (float)Math.Atan(point.X / point.Z);

                if (point.Z < 0)
                {
                    u += (float)Math.PI;
                }

            }
            if (u < 0)
            {
                u += (float)(2 * Math.PI);
            }

            u = (float)(u / 2 * Math.PI);
            v = -point.Y + 1 / 2;

            // TEST
            //u = (float)(0.5f - Math.Atan2(point.X, point.Z) * (1/Math.PI) * 2);
            //v = (float)(0.5f - Math.Asin(point.Y) * (1/Math.PI));
            return new Vector2(u, v) * _textureRepeat;
        }

        /// <summary>
        /// Create vertices and triangles vertices indice list
        /// </summary>
        private void CreateVertexData()
        {
            float t = (float)((1.0 + Math.Sqrt(5.0)) / 2.0);

            // Create the 12 vertices of the base isoahedron
            // See : http://blog.andreaskahler.com/2009/06/creating-icosphere-mesh-in-code.html
            AddVertex(-1, t, 0); // 0
            AddVertex(1, t, 0); // 1
            AddVertex(-1, -t, 0); // 2
            AddVertex(1, -t, 0); // 3

            AddVertex(0, -1, t); // 4
            AddVertex(0, 1, t); // 5
            AddVertex(0, -1, -t); // 6
            AddVertex(0, 1, -t); // 7

            AddVertex(t, 0, -1); // 8
            AddVertex(t, 0, 1); // 9
            AddVertex(-t, 0, -1); // 10
            AddVertex(-t, 0, 1); // 11

            // Create the base triangles of the isoahedron
            // WARNING : 
            // 1 - Use _material.Texture instead of Texture
            // 2 - An object can don't have a texture so you MUST use a default value
            int w = 1;
            int h = 1;

            float triSizeW = w / 5.5f;
            float triSizeH = h / 3f;
            float halfTriSize = triSizeW / 2;

            // 5 faces around point 0
            AddTriangle(0, 5, 11, new Vector2(halfTriSize / w, 0), new Vector2(triSizeW / w, triSizeH / h), new Vector2(0, triSizeH / h));
            AddTriangle(0, 1, 5, new Vector2((halfTriSize + triSizeW) / w, 0), new Vector2((triSizeW * 2) / w, triSizeH / h), new Vector2(triSizeW / w, triSizeH / h));
            AddTriangle(0, 7, 1, new Vector2((halfTriSize + triSizeW*2) / w, 0), new Vector2((triSizeW * 3) / w, triSizeH / h), new Vector2(triSizeW*2 / w, triSizeH / h));
            AddTriangle(0, 10, 7, new Vector2((halfTriSize + triSizeW * 3) / w, 0), new Vector2((triSizeW * 4) / w, triSizeH / h), new Vector2(triSizeW * 3 / w, triSizeH / h));
            AddTriangle(0, 11, 10, new Vector2((halfTriSize + triSizeW * 4) / w, 0), new Vector2((triSizeW * 5) / w, triSizeH / h), new Vector2(triSizeW * 4 / w, triSizeH / h));

            // 5 adjacent faces
            AddTriangle(1, 9, 5, new Vector2(triSizeW * 2 / w, triSizeH / h), new Vector2((halfTriSize + triSizeW) / w, triSizeH * 2 / h), new Vector2(triSizeW / w, triSizeH / h));
            AddTriangle(5, 4, 11, new Vector2(triSizeW / w, triSizeH / h), new Vector2(halfTriSize / w, triSizeH * 2 / h), new Vector2(0, triSizeH / h));
            AddTriangle(11, 2, 10, new Vector2(triSizeW * 5 / w, triSizeH / h), new Vector2((halfTriSize + triSizeW * 4) / w, triSizeH * 2 / h), new Vector2(triSizeW * 4 / w, triSizeH / h));
            AddTriangle(10, 6, 7, new Vector2(triSizeW * 4 / w, triSizeH / h), new Vector2((halfTriSize + triSizeW * 3) / w, triSizeH * 2 / h), new Vector2(triSizeW * 3 / w, triSizeH / h));
            AddTriangle(7, 8, 1, new Vector2(triSizeW * 3 / w, triSizeH / h), new Vector2((halfTriSize + triSizeW * 2) / w, triSizeH * 2 / h), new Vector2(triSizeW * 2 / w, triSizeH / h));

            // 5 faces around point 3

            AddTriangle(3, 4, 9, new Vector2(triSizeW / w, 1), new Vector2(halfTriSize / w, triSizeH * 2 / h), new Vector2((halfTriSize + triSizeW) / w, triSizeH * 2 / h));
            AddTriangle(3, 2, 4, new Vector2(triSizeW * 5 / w, 1), new Vector2((halfTriSize + triSizeW * 4) / w, triSizeH * 2 / h), new Vector2((halfTriSize + triSizeW * 5) / w, triSizeH * 2 / h));
            AddTriangle(3, 6, 2, new Vector2(triSizeW * 4 / w, 1), new Vector2((halfTriSize + triSizeW * 3) / w, triSizeH * 2 / h), new Vector2((halfTriSize + triSizeW * 4) / w, triSizeH * 2 / h));
            AddTriangle(3, 8, 6, new Vector2(triSizeW * 3 / w, 1), new Vector2((halfTriSize + triSizeW*2) / w, triSizeH * 2 / h), new Vector2((halfTriSize + triSizeW * 3) / w, triSizeH * 2 / h));
            AddTriangle(3, 9, 8, new Vector2(triSizeW * 2 / w, 1), new Vector2((halfTriSize + triSizeW) / w, triSizeH * 2 / h), new Vector2((halfTriSize + triSizeW * 2) / w, triSizeH * 2 / h));

            // 5 adjacent faces
            AddTriangle(4, 5, 9, new Vector2(halfTriSize / w, triSizeH * 2 / h), new Vector2(triSizeW / w, triSizeH / h), new Vector2((halfTriSize + triSizeW)  / w, triSizeH * 2 / h));
            AddTriangle(2, 11, 4, new Vector2((halfTriSize + triSizeW * 4) / w, triSizeH * 2 / h), new Vector2((triSizeW * 5) / w, triSizeH / h), new Vector2((halfTriSize + triSizeW * 5) / w, triSizeH * 2 / h));
            AddTriangle(6, 10, 2, new Vector2((halfTriSize + triSizeW * 3) / w, triSizeH * 2 / h), new Vector2(triSizeW * 4 / w, triSizeH / h), new Vector2((halfTriSize + triSizeW * 4) / w, triSizeH * 2 / h));
            AddTriangle(8, 7, 6, new Vector2((halfTriSize + triSizeW * 2) / w, triSizeH * 2 / h), new Vector2(triSizeW * 3 / w, triSizeH / h), new Vector2((halfTriSize + triSizeW * 3) / w, triSizeH * 2 / h));
            AddTriangle(9, 1, 8, new Vector2((halfTriSize + triSizeW) / w, triSizeH * 2 / h), new Vector2((triSizeW * 2) / w, triSizeH / h), new Vector2((halfTriSize + triSizeW*2) / w, triSizeH * 2 / h));

            // Refine the triangles to reach the wanted level of detail
            _cache = new Dictionary<Int64, int>();
            for (int level = 0; level < _lod; level++)
            {
                List<TriangleData> newTriangles = new List<TriangleData>();
                //Vector2 uva, uvb, uvc;
                foreach (TriangleData triangle in _triangles)
                {
                    // Replace the current triangle by 4 triangles
                    int a = CreateMiddlePoint(triangle.A, triangle.B);
                    int b = CreateMiddlePoint(triangle.B, triangle.C);
                    int c = CreateMiddlePoint(triangle.C, triangle.A);

                    // TODO Handle UV Mapping curing vertex refining
                    Vector2 uva = GetMiddle(triangle.UVA, triangle.UVB);
                    Vector2 uvb = GetMiddle(triangle.UVB, triangle.UVC);
                    Vector2 uvc = GetMiddle(triangle.UVC, triangle.UVA);


                    newTriangles.Add(new TriangleData(triangle.A, a, c, triangle.UVA, uva, uvc));
                    newTriangles.Add(new TriangleData(triangle.B, b, a, triangle.UVB, uvb, uva));
                    newTriangles.Add(new TriangleData(triangle.C, c, b, triangle.UVC, uvc, uvb));
                    newTriangles.Add(new TriangleData(a, b, c, uva, uvb, uvc));
                }

                // Update the base triangle list
                _triangles = newTriangles;
            }
        }

        private int AddVertex(float x, float y, float z)
        {
            float length = (float)Math.Sqrt(x * x + y * y + z * z);
            _vertexPositions.Add(new Vector3(x / length, y / length, z / length));
            return _vertexPositions.Count - 1;
        }

        private void AddTriangle(int a, int b, int c)
        {
            AddTriangle(a, b, c, Vector2.Zero, Vector2.Zero, Vector2.Zero);
        }

        private void AddTriangle(int a, int b, int c, Vector2 uva, Vector2 uvb, Vector2 uvc)
        {
            _triangles.Add(new TriangleData(a, b, c, uva, uvb, uvc));
        }

        private int CreateMiddlePoint(int idx1, int idx2)
        {
            // Build the cache key
            bool firstIsSmaller = idx1 < idx2;
            Int64 smallerIndex = firstIsSmaller ? idx1 : idx2;
            Int64 greaterIndex = firstIsSmaller ? idx2 : idx1;
            Int64 key = (smallerIndex << 32) + greaterIndex;

            int index;
            if (_cache.ContainsKey(key))
            {
                // The middle point was already computed, get it from the cache
                index = _cache[key];
            }
            else
            {
                // Retrieve already stored vertices positions
                Vector3 p1 = _vertexPositions[idx1];
                Vector3 p2 = _vertexPositions[idx2];

                // Create the middle point
                Vector3 middle = new Vector3(
                    (p1.X + p2.X) / 2,
                    (p1.Y + p2.Y) / 2,
                    (p1.Z + p2.Z) / 2
                );

                // Add the middle point to the vertex position list
                index = AddVertex(middle.X, middle.Y, middle.Z);

                // Add this point to the cache
                _cache.Add(key, index);
            }

            return index;
        }

        private Vector2 GetMiddle(Vector2 a, Vector2 b)
        {
            return (a + b) / 2;
        }

        public override void Draw(GraphicsDevice device, BaseMaterial material)
        {
            DrawPrimitives(device, material);
        }

        private Vector3 ComputeNormal(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3 u = p2 - p1;
            Vector3 v = p3 - p1;

            return u * v;
        }
    }
}
