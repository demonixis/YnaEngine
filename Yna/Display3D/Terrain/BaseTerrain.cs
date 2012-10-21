using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display3D;
using Yna.Display3D.Camera;

namespace Yna.Display3D.Terrain
{
    /// <summary>
    /// Abstract class that represent a basic Terrain
    /// </summary>
    public abstract class BaseTerrain : YnObject3D
    {
        #region Private declarations

        protected Texture2D _texture;
        protected string _textureName;
        protected Vector2 _textureRepeat;
        protected Vector3 _segmentSizes;
        protected bool _constructed;

        protected VertexPositionColorTexture[] _vertices;
        protected short[] _indices;
        protected bool _lightningEnabled;
        protected bool _colorEnabled;
        protected bool _textureEnabled;

        #endregion

        #region Properties

        /// <summary>
        /// Width of the terrain
        /// </summary>
        public new int Width
        {
            get { return (int)_width; }
            protected set { _width = (float)value; }
        }

        /// <summary>
        /// Height of the terrain
        /// </summary>
        public new int Height
        {
            get { return (int)_height; }
            protected set { _height = (float)value; }
        }

        /// <summary>
        /// Depth of the terrain
        /// </summary>
        public new int Depth
        {
            get { return (int)_depth; }
            protected set { _depth = (float)value; }
        }

        /// <summary>
        /// Get or Set the segments size. It represent the space between two vertex.
        /// The terrain is reconstructed when you set a new value
        /// </summary>
        public Vector3 SegmentSizes
        {
            get { return _segmentSizes; }
            set
            {
                if (value != Vector3.Zero)
                {
                    _segmentSizes = value;

                    if (_constructed)
                    {
                        CreateVertices();
                        CreateIndices();
                    }
                }
            }
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

        public Vector2 TextureRepeat
        {
            get { return _textureRepeat; }
            set { _textureRepeat = value; }
        }

        /// <summary>
        /// Vertices that compose the terrain
        /// </summary>
        public VertexPositionColorTexture[] Vertices
        {
            get { return _vertices; }
            set { _vertices = value; }
        }

        /// <summary>
        /// Shader effect
        /// </summary>
        public BasicEffect BasicEffect
        {
            get { return _basicEffect; }
            set { _basicEffect = value; }
        }

        #endregion

        /// <summary>
        /// Basic initialization for an abstract terrain
        /// </summary>
        public BaseTerrain()
            : base(new Vector3(0, 0, 0))
        {
            _texture = null;
            _textureName = String.Empty;
            _textureRepeat = Vector2.One;
            _constructed = false;
            _segmentSizes = Vector3.One;

            _lightningEnabled = false;
            _colorEnabled = true;
            _textureEnabled = false;
            _initialized = false;
        }

        public BaseTerrain(Vector3 position)
            : this()
        {
            _position = position;
        }

        #region Vertices construction & Setup

        /// <summary>
        /// Create the vertex array
        /// </summary>
        protected abstract void CreateVertices();

        /// <summary>
        /// Create indices with vertex array
        /// </summary>
        protected virtual void CreateIndices()
        {
            _indices = new short[(Width - 1) * (Depth - 1) * 6];
            int counter = 0;

            for (int x = 0; x < Width - 1; x++)
            {
                for (int y = 0; y < Depth - 1; y++)
                {
                    short lowerLeft = (short)(x + y * Width);
                    short lowerRight = (short)((x + 1) + y * Width);
                    short topLeft = (short)(x + (y + 1) * Width);
                    short topRight = (short)((x + 1) + (y + 1) * Width);

                    _indices[counter++] = topLeft;
                    _indices[counter++] = lowerLeft;
                    _indices[counter++] = lowerRight;
                    _indices[counter++] = topLeft;
                    _indices[counter++] = lowerRight;
                    _indices[counter++] = topRight;
                }
            }
        }

        /// <summary>
        /// Setup the basic effet
        /// </summary>
        protected virtual void SetupShader()
        {
            _basicEffect.LightingEnabled = _lightningEnabled;

            _basicEffect.VertexColorEnabled = _colorEnabled;

            _basicEffect.Texture = _texture;

            _basicEffect.TextureEnabled = _textureEnabled;
        }

        #endregion

        #region Bounding volumes

        /// <summary>
        /// Create the bounding box of the object
        /// </summary>
        protected void CreateBoundingBox()
        {
            _boundingBox = new BoundingBox();

            for (int i = 0; i < _vertices.Length; i++)
            {
                _boundingBox.Min.X = _boundingBox.Min.X < _vertices[i].Position.X ? _boundingBox.Min.X : _vertices[i].Position.X;
                _boundingBox.Min.Y = _boundingBox.Min.Y < _vertices[i].Position.Y ? _boundingBox.Min.Y : _vertices[i].Position.Y;
                _boundingBox.Min.Z = _boundingBox.Min.Z < _vertices[i].Position.Z ? _boundingBox.Min.Z : _vertices[i].Position.Z;

                _boundingBox.Max.X = _boundingBox.Max.X > _vertices[i].Position.X ? _boundingBox.Max.X : _vertices[i].Position.X;
                _boundingBox.Max.Y = _boundingBox.Max.X > _vertices[i].Position.Y ? _boundingBox.Max.Y : _vertices[i].Position.Y;
                _boundingBox.Max.Z = _boundingBox.Max.X > _vertices[i].Position.Z ? _boundingBox.Max.Z : _vertices[i].Position.Z;
            }

            _width = _boundingBox.Max.X - _boundingBox.Min.X;
            _height = _boundingBox.Max.Y - _boundingBox.Min.Y;
            _depth = _boundingBox.Max.Z - _boundingBox.Min.Z;
        }

        /// <summary>
        /// Create the bounding sphere of the object
        /// </summary>
        protected void CreateBoundingSphere()
        {
            if (_boundingBox.Min.X == _boundingBox.Max.X && _boundingBox.Min.Y == _boundingBox.Max.Y && _boundingBox.Min.Z == _boundingBox.Max.Z)
                CreateBoundingBox();

            _boundingSphere.Center = new Vector3(X + Width / 2, Y + Height / 2, Z + Depth / 2);
            _boundingSphere.Radius = Math.Max(Math.Max(_width, _height), _depth) / 2;
        }

        public override void UpdateBoundingVolumes()
        {
            UpdateMatrix();

            CreateBoundingBox();

            CreateBoundingSphere();

            _boundingFrustrum = new BoundingFrustum(_camera.Projection * World);
        }

        #endregion

        public override void UpdateMatrix()
        {
            World = Matrix.CreateScale(Scale) *
                Matrix.CreateRotationX(Rotation.X) *
                Matrix.CreateRotationY(Rotation.Y) *
                Matrix.CreateRotationZ(Rotation.Z) *
                Matrix.CreateTranslation(Position);

            View = _camera.View;
        }

        /// <summary>
        /// Load terrain's texture if the _textureEnabled variable is true
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            if (!_initialized)
            {
                if (_textureName != String.Empty)
                {
                    _texture = YnG.Content.Load<Texture2D>(_textureName);
                    _initialized = true;
                }
            }
        }

        /// <summary>
        /// Draw Terrain
        /// </summary>
        /// <param name="device"></param>
        public override void Draw(GraphicsDevice device)
        {
            UpdateMatrix();

            _basicEffect.World = World;

            _basicEffect.View = View;

            _basicEffect.Projection = _camera.Projection;

            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length, _indices, 0, _indices.Length / 3);
            }
        }
    }
}
