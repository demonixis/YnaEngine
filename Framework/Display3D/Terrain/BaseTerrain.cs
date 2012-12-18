using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D;
using Yna.Framework.Display3D.Camera;
using Yna.Framework.Display3D.Primitive;
using Yna.Framework.Display3D.Light;

namespace Yna.Framework.Display3D.Terrain
{
    /// <summary>
    /// Abstract class that represent a basic Terrain
    /// </summary>
    public abstract class BaseTerrain : BasePrimitive
    {
        #region Private declarations

        protected VertexPositionNormalTexture[] _vertices;
        protected VertexBuffer _vertexBuffer;
        protected short[] _indices;
        protected IndexBuffer _indexBuffer;

        #endregion

        #region Properties

        /// <summary>
        /// Vertices that compose the terrain
        /// </summary>
        public VertexPositionNormalTexture[] Vertices
        {
            get { return _vertices; }
            set { _vertices = value; }
        }

        #endregion

        /// <summary>
        /// Basic initialization for an abstract terrain
        /// </summary>
        public BaseTerrain()
            : base(new Vector3(0, 0, 0))
        {
            _colorEnabled = true;
            _textureEnabled = false;
            _initialized = false;
        }

        public BaseTerrain(Vector3 position)
            : this()
        {
            _position = position;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (!_initialized)
            {
                if (_textureName != String.Empty && _texture == null)
                {
                    _texture = YnG.Content.Load<Texture2D>(_textureName);
                    _textureEnabled = true;
                    _colorEnabled = false;
                    _initialized = true;
                }
            }
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

            _vertexBuffer = new VertexBuffer(YnG.GraphicsDevice, typeof(VertexPositionNormalTexture), _vertices.Length, BufferUsage.WriteOnly);
            _vertexBuffer.SetData(_vertices);

            _indexBuffer = new IndexBuffer(YnG.GraphicsDevice, IndexElementSize.SixteenBits, _indices.Length, BufferUsage.WriteOnly);
            _indexBuffer.SetData(_indices);
        }

        public virtual void ComputeNormals()
        {
            for (int i = 0; i < _vertices.Length; i++)
                _vertices[i].Normal = Vector3.Zero;

            for (int i = 0; i < _indices.Length / 3; i++)
            {
                int index1 = _indices[i * 3];
                int index2 = _indices[i * 3 + 1];
                int index3 = _indices[i * 3 + 2];

                // Select the face
                Vector3 side1 = _vertices[index1].Position - _vertices[index3].Position;
                Vector3 side2 = _vertices[index1].Position - _vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                _vertices[index1].Normal += normal;
                _vertices[index2].Normal += normal;
                _vertices[index3].Normal += normal;
            }

            for (int i = 0; i < _vertices.Length; i++)
                _vertices[i].Normal.Normalize();
        }

        #endregion

        #region Bounding volumes

        public override void UpdateBoundingVolumes()
        {
            UpdateMatrix();

            _boundingBox = new BoundingBox(new Vector3(float.MaxValue), new Vector3(float.MinValue));

            for (int i = 0; i < _vertices.Length; i++)
            {
                _boundingBox.Min.X = Math.Min(_boundingBox.Min.X, _vertices[i].Position.X);
                _boundingBox.Min.Y = Math.Min(_boundingBox.Min.Y, _vertices[i].Position.Y);
                _boundingBox.Min.Z = Math.Min(_boundingBox.Min.Z, _vertices[i].Position.Z);
                _boundingBox.Max.X = Math.Max(_boundingBox.Max.X, _vertices[i].Position.X);
                _boundingBox.Max.Y = Math.Max(_boundingBox.Max.Y, _vertices[i].Position.Y);
                _boundingBox.Max.Z = Math.Max(_boundingBox.Max.Z, _vertices[i].Position.Z);
            }

            _boundingBox.Min.X *= _scale.X;
            _boundingBox.Min.Y *= _scale.Y;
            _boundingBox.Min.Z *= _scale.Z;
            _boundingBox.Max.X *= _scale.X;
            _boundingBox.Max.Y *= _scale.Y;
            _boundingBox.Max.Z *= _scale.Z;

            _width = (_boundingBox.Max.X - _boundingBox.Min.X) * _scale.X;
            _height = (_boundingBox.Max.Y - _boundingBox.Min.Y) * _scale.Y;
            _depth = (_boundingBox.Max.Z - _boundingBox.Min.Z) * _scale.Z;

            _boundingSphere.Center = new Vector3(X + Width / 2, Y + Height / 2, Z + Depth / 2);
            _boundingSphere.Radius = Math.Max(Math.Max(_width, _height), _depth) / 2;
        }

        #endregion

        /// <summary>
        /// Draw Terrain
        /// </summary>
        /// <param name="device"></param>
        public override void Draw(GraphicsDevice device)
        {
            base.Draw(device);

            //device.SetVertexBuffer(_vertexBuffer);
            //device.Indices = _indexBuffer;

            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //device.DrawPrimitives(PrimitiveType.TriangleList, 0, _vertices.Length / 3);
                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length, _indices, 0, _indices.Length / 3);
            }

            //device.SetVertexBuffer(null);
            //device.Indices = null;
        }

        public virtual void MoveVertex(int x, int z, float deltaY)
        {
            int index = x + z * (int)(Width +1);
            VertexPositionNormalTexture vpmt = _vertices[index];

            vpmt.Position = new Vector3(
                vpmt.Position.X,
                vpmt.Position.Y + deltaY,
                vpmt.Position.Z
                );
            _vertices[index] = vpmt;

            ComputeNormals();

            SetupShader();

            UpdateBoundingVolumes();
            UpdateMatrix();
        }
    }
}
