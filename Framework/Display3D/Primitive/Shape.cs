using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
    /// 
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
        protected Texture2D _texture;
        protected string _textureName;
        protected Vector2 _textureRepeat;

        // Segments size
        protected Vector3 _segmentSizes;
        protected bool _useDefaultLightning;
        protected bool _useLightning;
        protected bool _useVertexColor;
        protected bool _useTexture;
        protected bool _constructed;

        // Update flags
        protected bool _needMatricesUpdate;
        protected bool _needShaderUpdate;
        protected bool _needLightUpdate;

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
        /// Enable or disable the default lightning on the shader
        /// </summary>
        public bool UseDefaultLightning
        {
            get { return _useDefaultLightning; }
            set
            {
                _useDefaultLightning = value;
                _needLightUpdate = true;
            }
        }

        /// <summary>
        /// Enable or disable the lightning 
        /// </summary>
        public bool UseLighning
        {
            get { return _useLightning; }
            set
            {
                _useLightning = value;
                _needLightUpdate = true;
            }
        }

        /// <summary>
        /// Enable or disable vertex color
        /// </summary>
        public bool UseVertexColor
        {
            get { return _useVertexColor; }
            set
            {
                _useVertexColor = value;
                _needShaderUpdate = true;
            }
        }

        /// <summary>
        /// Enable or disable texture
        /// </summary>
        public bool UseTexture
        {
            get { return _useTexture; }
            set
            {
                _useTexture = value;
                _needShaderUpdate = true;
            }
        }

        /// <summary>
        /// Texture to use with the terrain
        /// </summary>
        public Texture2D Texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                _textureName = _texture.Name;
            }
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
        /// Gets or sets the texture name
        /// </summary>
        public string TextureName
        {
            get { return _textureName; }
            set { _textureName = value; }
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

        /// <summary>
        /// Gets or sets the value of the flags who's used to update the shader.
        /// If set to true the shader will be updated
        /// </summary>
        public bool NeedShaderUpdate
        {
            get { return _needShaderUpdate; }
            set { _needShaderUpdate = value; }
        }

        /// <summary>
        /// Gets or sets the value of the flags who's used to update lightning.
        /// If set to true lightning will be updated
        /// </summary>
        public bool NeedLightUpdate
        {
            get { return _needLightUpdate; }
            set { _needLightUpdate = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a basic shape
        /// </summary>
        public Shape()
            : base ()
        {
            _texture = null;
            _textureName = String.Empty;
            _textureRepeat = Vector2.One;

            _segmentSizes = Vector3.One;
            _useDefaultLightning = false;
            _useVertexColor = false;
            _useTexture = false;
            _constructed = false;

            _needMatricesUpdate = true;
            _needShaderUpdate = true;
            _needLightUpdate = true;
        }

        /// <summary>
        /// Create a new shape with a texture
        /// </summary>
        /// <param name="textureName">The texture name</param>
        public Shape(string textureName)
            : this()
        {
            _textureName = textureName;
            _useTexture = true;
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
            base.LoadContent();

            if (_textureName != String.Empty && _texture == null)
            {
                _texture = YnG.Content.Load<Texture2D>(_textureName);
                _useTexture = true;
                _useVertexColor = false;
                _initialized = true;
            }

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
            UpdateShader();
            UpdateBoundingVolumes();
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
        /// Setup the basic effet. Note that the flag NeedShaderUpdate must be set to
        /// true for updating. If you wan't to force update use true on parameter
        /// </summary>
        /// <param name="forceUpdate">True for force update</param>
        protected virtual void UpdateShader(bool forceUpdate)
        {
            if (_needShaderUpdate || forceUpdate)
            {
                if (_useDefaultLightning)
                    _basicEffect.EnableDefaultLighting();
                else
                    UpdateLightning(true);

                _basicEffect.VertexColorEnabled = _useVertexColor;

                if (_texture != null)
                    _basicEffect.Texture = _texture;

                _basicEffect.TextureEnabled = _useTexture;
            }

            _needShaderUpdate = false;
        }

        /// <summary>
        /// Setup the basic effet. Note that the flag NeedShaderUpdate must be set to
        /// true for updating.
        /// </summary>
        protected virtual void UpdateShader()
        {
            UpdateShader(false);
        }

        /// <summary>
        /// Set the lightning. Note that the flag NeedLightUpdate must be set to
        /// true for updating. If you wan't force update use true on parameter
        /// </summary>
        /// <param name="forceUpdate">True for force update</param>
        protected virtual void UpdateLightning(bool forceUpdate)
        {
            if (_needLightUpdate || forceUpdate)
            {
                _basicEffect.LightingEnabled = !_useDefaultLightning;
                _basicEffect.DirectionalLight0.Enabled = true;
                _basicEffect.DirectionalLight0.Direction = _light.Direction;
                _basicEffect.DirectionalLight0.DiffuseColor = _light.Diffuse;
                _basicEffect.DirectionalLight0.SpecularColor = _light.Specular;
                _basicEffect.AmbientLightColor = _light.Ambient;
                _basicEffect.EmissiveColor = _light.Emissive;
                _basicEffect.Alpha = _light.Alpha;
            }
        }

        /// <summary>
        /// Set the lightning. Note that the flag NeedLightUpdate must be set to
        /// true for updating. If you wan't force update use true on parameter
        /// </summary>
        public virtual void UpdateLightning()
        {
            UpdateLightning(false);
        }

        /// <summary>
        /// Update matrices world and view. There are 3 updates
        /// 1 - Scale
        /// 2 - Rotation on Y axis (override if you wan't more)
        /// 3 - Translation
        /// </summary>
        public override void UpdateMatrices()
        {
            World = Matrix.CreateScale(Scale) *
                Matrix.CreateRotationY(_rotation.Y) *
                Matrix.CreateTranslation(Position);

            View = _camera.View;
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

            if (_needShaderUpdate)
                UpdateShader();

            if (_needLightUpdate)
                UpdateLightning();

            _basicEffect.World = World;
            _basicEffect.View = View;
            _basicEffect.Projection = _camera.Projection;
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

            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, _vertices.Length / 3);
            }

            device.SetVertexBuffer(null);
            device.Indices = null;
        }
    }
}
