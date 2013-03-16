using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Material;
using Yna.Engine.Graphics3D.Lighting;

namespace Yna.Engine.Graphics3D.Geometry
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
    public abstract class BaseGeometry<T> where T : struct, IVertexType
    {
        #region Protected declarations
    
        // Geometry
        protected T[] _vertices;
        protected short[] _indices;
        protected VertexBuffer _vertexBuffer;
        protected IndexBuffer _indexBuffer;

        // Texture
        protected Vector2 _textureRepeat;

        // Segments size
        protected Vector3 _segmentSizes;
        protected Vector3 _origin;
        protected Vector3 _position;
        protected Vector3 _dimension;
        protected bool _constructed;

        #endregion

        #region Globals properties

        /// <summary>
        /// Gets the vertex array used by this object
        /// </summary>
        public T[] Vertices
        {
            get { return _vertices; }
            protected set { _vertices = value; }
        }

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
        /// Gets or sets the repeat value for the texture used by the object
        /// </summary>
        public Vector2 TextureRepeat
        {
            get { return _textureRepeat; }
            set { _textureRepeat = value; }
        }

        /// <summary>
        /// Gets or sets the segments size. It represent the space between two vertex.
        /// </summary>
        public Vector3 SegmentSizes
        {
            get { return _segmentSizes; }
            set { _segmentSizes = value; }
        }

        public Vector3 Dimension
        {
            get { return _dimension; }
            set { _dimension = value; }
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

        public int Width
        {
            get { return (int)_dimension.X; }
            protected set { _dimension.X = value; }
        }

        public int Height
        {
            get { return (int)_dimension.Y; }
            protected set { _dimension.Y = value; }
        }

        public int Depth
        {
            get { return (int)_dimension.Z; }
            protected set { _dimension.Z = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a basic shape
        /// </summary>
        public BaseGeometry()
        {
            _textureRepeat = Vector2.One;
            _position = Vector3.Zero;
            _origin = Vector3.Zero;
            _segmentSizes = Vector3.One;
            _dimension = Vector3.Zero;
            _constructed = false;
        }

        /// <summary>
        /// Create a new shape with a texture, a size and a position
        /// </summary>
        /// <param name="textureName">The texture name</param>
        /// <param name="sizes">Desired size</param>
        /// <param name="position">Position</param>
        public BaseGeometry(Vector3 sizes)
            : this()
        {
            _segmentSizes = sizes;
        }

        #endregion

        /// <summary>
        /// Generate the shape, update the shader and update bounding volumes
        /// </summary>
        public virtual void GenerateGeometry()
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
            _vertexBuffer = new VertexBuffer(YnG.GraphicsDevice, typeof(T), _vertices.Length, BufferUsage.WriteOnly);
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
            for (int i = 0; i < _vertices.Length; i++)
                vertices[i].Normal = Vector3.Zero;

            for (int i = 0; i < _indices.Length / 3; i++)
            {
                int index1 = _indices[i * 3];
                int index2 = _indices[i * 3 + 1];
                int index3 = _indices[i * 3 + 2];

                // Select the face
                Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
                Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal += normal;
                vertices[index2].Normal += normal;
                vertices[index3].Normal += normal;
            }

            for (int i = 0; i < _vertices.Length; i++)
                vertices[i].Normal.Normalize();
        }

        /// <summary>
        /// Draw the shape
        /// </summary>
        /// <param name="device">Graphics device</param>
        public virtual void Draw(GraphicsDevice device, BaseMaterial material)
        {
            device.SetVertexBuffer(_vertexBuffer);
            device.Indices = _indexBuffer;

            foreach (EffectPass pass in material.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, _vertices.Length / 3);
            }

            device.SetVertexBuffer(null);
            device.Indices = null;
        }
    }
}
