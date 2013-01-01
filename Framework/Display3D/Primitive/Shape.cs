using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D.Material;

namespace Yna.Framework.Display3D.Primitive
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
    public abstract class Shape<T> : YnObject3D where T : struct, IVertexType
    {
        #region Protected declarations
    
        // Geometry
        protected T[] _vertices;
        protected short[] _indices;
        protected VertexBuffer _vertexBuffer;
        protected IndexBuffer _indexBuffer;

        // Texture
        protected string _textureName;
        protected Vector2 _textureRepeat;

        // Segments size
        protected Vector3 _segmentSizes;
        protected bool _constructed;

        // Update flags
        protected bool _needMatricesUpdate;

        #endregion

        #region Globals properties

        /// <summary>
        /// Gets the vertex array used by this object
        /// </summary>
        public T[] Vertices
        {
            get { return _vertices; }
        }

        /// <summary>
        /// True if the texture is loaded
        /// </summary>
        public bool Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
        }

        /// <summary>
        /// Gets the state of the geometry.
        /// true if constructed then false
        /// </summary>
        public bool Constructed
        {
            get { return _constructed; }
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

        /// <summary>
        /// Width of the shape
        /// </summary>
        public new int Width
        {
            get { return (int)_width; }
            protected set { _width = (float)value; }
        }

        /// <summary>
        /// Height of the shape
        /// </summary>
        public new int Height
        {
            get { return (int)_height; }
            protected set { _height = (float)value; }
        }

        /// <summary>
        /// Depth of the shape
        /// </summary>
        public new int Depth
        {
            get { return (int)_depth; }
            protected set { _depth = (float)value; }
        }

        #endregion

        #region Flags properties

        /// <summary>
        /// Gets or sets the value of the flags who's used to update matrices.
        /// If set to true, all matrices will be updated
        /// </summary>
        public bool NeedMatricesUpdate
        {
            get { return _needMatricesUpdate; }
            set { _needMatricesUpdate = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a basic shape
        /// </summary>
        public Shape()
            : base ()
        {
            _textureName = String.Empty;
            _textureRepeat = Vector2.One;

            _segmentSizes = Vector3.One;
            _constructed = false;

            _needMatricesUpdate = true;
        }

        /// <summary>
        /// Create a new shape with a texture
        /// </summary>
        /// <param name="textureName">The texture name</param>
        public Shape(string textureName)
            : this()
        {
            _textureName = textureName;
        }

        /// <summary>
        /// Create a new shape with a texture, a size and a position
        /// </summary>
        /// <param name="textureName">The texture name</param>
        /// <param name="sizes">Desired size</param>
        /// <param name="position">Position</param>
        public Shape(string textureName, Vector3 sizes, Vector3 position)
            : this(textureName)
        {
            _segmentSizes = sizes;
            _position = position;
        }

        #endregion

        /// <summary>
        /// Load the texture and launch the process of generation
        /// </summary>
        public override void LoadContent()
        {
            if (_material == null)
                _material = new BasicMaterial(_textureName);
                
            _material.LoadContent();

            GenerateShape();
        }

        /// <summary>
        /// Generate the shape, update the shader and update bounding volumes
        /// </summary>
        protected virtual void GenerateShape()
        {
            if (_constructed)
            {
                _vertexBuffer.Dispose();
                _indexBuffer.Dispose();
            }

            CreateVertices();
            CreateIndices();
            CreateBuffers();
            UpdateBoundingVolumes();
            _constructed = true;
            _initialized = true;
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
        /// Update matrices world and view. There are 3 updates
        /// 1 - Scale
        /// 2 - Rotation on Y axis (override if you wan't more)
        /// 3 - Translation
        /// </summary>
        public override void UpdateMatrices()
        {
            _world = Matrix.CreateScale(Scale) *
                Matrix.CreateFromYawPitchRoll(_rotation.Y, _rotation.X, _rotation.Z) *
                Matrix.CreateTranslation(Position);

            _view = _camera.View;
            _projection = _camera.Projection;
        }

        /// <summary>
        /// Update Bounding Box and Bounding Sphere
        /// </summary>
        public override void UpdateBoundingVolumes()
        {
            _boundingBox = new BoundingBox(
                new Vector3(X, Y, Z),
                new Vector3(X + Width, Y + Height, Z + Depth));

            _boundingSphere = new BoundingSphere(
                new Vector3(X + Width / 2, Y + Height / 2, Z + Depth / 2),
                Math.Max(Math.Max(Width, Height), Depth));
        }

        public virtual void PreDraw()
        {
            if (_needMatricesUpdate)
                UpdateMatrices();

            if (_dynamic)
                UpdateBoundingVolumes();

            _material.Update(ref _world, ref _view, ref _projection, ref _position);
        }

        /// <summary>
        /// Draw the shape
        /// </summary>
        /// <param name="device">Graphics device</param>
        public override void Draw(GraphicsDevice device)
        {
            PreDraw();

            device.SetVertexBuffer(_vertexBuffer);
            device.Indices = _indexBuffer;

            foreach (EffectPass pass in _material.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, _vertices.Length / 3);
            }

            device.SetVertexBuffer(null);
            device.Indices = null;
        }
    }
}
