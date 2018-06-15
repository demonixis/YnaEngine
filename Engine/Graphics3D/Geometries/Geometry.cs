// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Materials;
using Yna.Engine.Graphics3D.Lighting;

namespace Yna.Engine.Graphics3D.Geometries
{
    /// <summary>
    /// Basic class for create an object geometry
    /// How does it work ?
    /// 1 - Make your constructor
    /// 2 - override CreateVertices()
    /// 3 - override CreateIndices()
    /// 4 - (optional) override GenerateShape for change the order of generation
    /// 5 - (optional but recommanded) override UpdateBoundingVolumes
    /// 6 - (optional) if you don't want to use vertex/indexBuffer override Draw method
    /// 7 - (optional) override UpdateShader() if you want to use a custom Effect
    ///     and call in first PreDraw() method. Do your stuff after that
    /// </summary>
    /// <typeparam name="T">Type of IVertexType</typeparam>
    public abstract class Geometry
    {
        #region Protected declarations

        // Geometry
        protected VertexPositionNormalTexture[] _vertices;
        protected short[] _indices;
        protected VertexBuffer _vertexBuffer;
        protected IndexBuffer _indexBuffer;
        protected Vector2 _UVOffset;
        protected Vector3 _size;
        protected Vector3 _origin;
        protected Vector3 _position;
        protected bool _constructed;
        protected bool _invertFaces;
        protected bool _doubleSided;
        protected bool _wireframe;
        protected RasterizerState _newRasterizerState;
        protected RasterizerState _oldRasterizerState;

        #endregion

        #region Globals properties

        /// <summary>
        /// Gets the vertex array used by this object
        /// </summary>
        public VertexPositionNormalTexture[] Vertices => _vertices;

        public short[] Indices => _indices;

        /// <summary>
        /// Gets the state of the geometry.
        /// true if constructed then false
        /// </summary>
        public bool Constructed
        {
            get { return _constructed; }
            protected set { _constructed = value; }
        }

        /// <summary>
        /// Enable of disable face inversion. Note that you can't invert faces when
        /// the geometry is constructed.
        /// </summary>
        public bool InvertFaces
        {
            get { return _invertFaces; }
            set { _invertFaces = value; }
        }

        /// <summary>
        /// Gets or sets the repeat value for the texture used by the object
        /// </summary>
        public Vector2 TextureRepeat
        {
            get { return _UVOffset; }
            set { _UVOffset = value; }
        }

        /// <summary>
        /// Gets or sets the segments size. It represent the space between two vertex.
        /// </summary>
        public Vector3 Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public Vector3 Position
        {
            get { return _position; }
            protected set { _position = value; }
        }

        public Vector3 Origin
        {
            get { return _origin; }
            protected set { _origin = value; }
        }

        public VertexBuffer VertexBuffer => _vertexBuffer;
        public IndexBuffer IndexBuffer => _indexBuffer;

        /// <summary>
        /// Enable or disable the rendering on all faces, event hidden faces.
        /// </summary>
        public bool DoubleSided
        {
            get { return _doubleSided; }
            set
            {
                _doubleSided = value;
                if (_doubleSided)
                    _newRasterizerState.CullMode = CullMode.None;
                else
                    _newRasterizerState.CullMode = CullMode.CullClockwiseFace;
            }
        }

        /// <summary>
        /// Enable or disable the wireframe mode for this entity.
        /// </summary>
        public bool WireFrame
        {
            get { return _wireframe; }
            set
            {
                _wireframe = value;
                if (_wireframe)
                    _newRasterizerState.FillMode = FillMode.WireFrame;
                else
                    _newRasterizerState.FillMode = FillMode.Solid;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a basic shape
        /// </summary>
        public Geometry()
        {
            _UVOffset = Vector2.One;
            _position = Vector3.Zero;
            _origin = Vector3.Zero;
            _size = Vector3.One;
            _constructed = false;
            _invertFaces = false;
            _doubleSided = false;
            _newRasterizerState = new RasterizerState();
            _oldRasterizerState = null;
        }

        /// <summary>
        /// Create a new shape with a texture, a size and a position
        /// </summary>
        /// <param name="textureName">The texture name</param>
        /// <param name="sizes">Desired size</param>
        /// <param name="position">Position</param>
        public Geometry(Vector3 sizes)
            : this()
        {
            _size = sizes;
        }

        #endregion

        /// <summary>
        /// Generate the shape, update the shader and update bounding volumes
        /// </summary>
        public virtual void Generate()
        {
            if (_constructed)
            {
                _vertexBuffer.Dispose();
                _indexBuffer.Dispose();
            }

            CreateVertices();
            CreateIndices();
            CreateBuffers();
            _constructed = true;
        }

        /// <summary>
        /// Create vertex array
        /// </summary>
        protected abstract void CreateVertices();

        /// <summary>
        /// Create index array
        /// </summary>
        protected abstract void CreateIndices();

        /// <summary>
        /// Create vertex buffer and index buffer
        /// </summary>
        protected virtual void CreateBuffers()
        {
            _vertexBuffer = new VertexBuffer(YnG.GraphicsDevice, typeof(VertexPositionNormalTexture), _vertices.Length, BufferUsage.WriteOnly);
            _vertexBuffer.SetData(_vertices);

            _indexBuffer = new IndexBuffer(YnG.GraphicsDevice, IndexElementSize.SixteenBits, _indices.Length, BufferUsage.WriteOnly);
            _indexBuffer.SetData(_indices);
        }

        /// <summary>
        /// Calculate normals for an object who use an array of VertexPositionNormalTexture
        /// </summary>
        /// <param name="vertices">A reference of an array of VertexPositionNormalTexture</param>
        public virtual void ComputeNormals(ref VertexPositionNormalTexture[] vertices)
        {
            for (var i = 0; i < _vertices.Length; i++)
                vertices[i].Normal = Vector3.Zero;

            for (var i = 0; i < _indices.Length / 3; i++)
            {
                var index1 = _indices[i * 3];
                var index2 = _indices[i * 3 + 1];
                var index3 = _indices[i * 3 + 2];

                // Select the face
                var side1 = vertices[index1].Position - vertices[index3].Position;
                var side2 = vertices[index1].Position - vertices[index2].Position;
                var normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal += normal;
                vertices[index2].Normal += normal;
                vertices[index3].Normal += normal;
            }

            for (var i = 0; i < _vertices.Length; i++)
                vertices[i].Normal.Normalize();
        }

        protected virtual void PreDraw(GraphicsDevice device)
        {
            if (_doubleSided || _wireframe)
            {
                _oldRasterizerState = device.RasterizerState;
                device.RasterizerState = _newRasterizerState;
            }
        }

        protected virtual void PostDraw(GraphicsDevice device)
        {
            if (_doubleSided || _wireframe)
                device.RasterizerState = _oldRasterizerState;
        }

        /// <summary>
        /// Draw the shape
        /// </summary>
        /// <param name="device">Graphics device</param>
        public virtual void Draw(GraphicsDevice device, Material material)
        {
            DrawPrimitives(device, material);
        }

        protected virtual void DrawPrimitives(GraphicsDevice device, Material material)
        {
            PreDraw(device);

            device.SetVertexBuffer(_vertexBuffer);
            device.Indices = _indexBuffer;

            foreach (var pass in material.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, _vertices.Length / 3);
            }

            device.SetVertexBuffer(null);
            device.Indices = null;

            PostDraw(device);
        }

        protected virtual void DrawUserIndexedPrimitives(GraphicsDevice device, Material material)
        {
            PreDraw(device);

            foreach (var pass in material.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length, _indices, 0, _indices.Length / 3);
            }

            PostDraw(device);
        }
    }
}
