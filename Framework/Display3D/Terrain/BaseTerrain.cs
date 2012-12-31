using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D;
using Yna.Framework.Display3D.Camera;
using Yna.Framework.Display3D.Primitive;
using Yna.Framework.Display3D.Lighting;
using Yna.Framework.Display3D.Material;

namespace Yna.Framework.Display3D.Terrain
{
    /// <summary>
    /// Abstract class that represent a basic Terrain
    /// </summary>
    public abstract class BaseTerrain : Shape<VertexPositionNormalTexture>
    {
        /// <summary>
        /// Basic initialization for an abstract terrain
        /// </summary>
        public BaseTerrain()
            : base()
        {
            _initialized = false;
        }

        public BaseTerrain(Vector3 position)
            : this()
        {
            _position = position;
        }

        /// <summary>
        /// Load texture if not already loaded. If you wan't to reload a new texture,
        /// set the Initialized property to true before calling this method
        /// </summary>
        public override void LoadContent()
        {
            if (_material == null)
                _material = new BasicMaterial(_textureName);
            _material.LoadContent();
        }

        /// <summary>
        /// Create indices with vertex array
        /// </summary>
        protected override void CreateIndices()
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

        public virtual void MoveVertex(int x, int z, float deltaY)
        {
            _vertices[x + z * Width].Position.Y += deltaY;

            // TODO : compute vertex normal only for this vertex
            ComputeNormals(ref _vertices);

            UpdateBoundingVolumes();
        }

        #region Bounding volumes

        public override void UpdateBoundingVolumes()
        {
            UpdateMatrices();

            // Reset bounding box to min/max values
            _boundingBox.Min = new Vector3(float.MaxValue);
            _boundingBox.Max = new Vector3(float.MinValue);

            for (int i = 0; i < _vertices.Length; i++)
            {
                _boundingBox.Min.X = Math.Min(_boundingBox.Min.X, _vertices[i].Position.X);
                _boundingBox.Min.Y = Math.Min(_boundingBox.Min.Y, _vertices[i].Position.Y);
                _boundingBox.Min.Z = Math.Min(_boundingBox.Min.Z, _vertices[i].Position.Z);
                _boundingBox.Max.X = Math.Max(_boundingBox.Max.X, _vertices[i].Position.X);
                _boundingBox.Max.Y = Math.Max(_boundingBox.Max.Y, _vertices[i].Position.Y);
                _boundingBox.Max.Z = Math.Max(_boundingBox.Max.Z, _vertices[i].Position.Z);
            }

            // Apply scale on the object
            _boundingBox.Min *= _scale;
            _boundingBox.Max *= _scale;

            // Update size of the object
            _width = _boundingBox.Max.X - _boundingBox.Min.X;
            _height = _boundingBox.Max.Y - _boundingBox.Min.Y;
            _depth = _boundingBox.Max.Z - _boundingBox.Min.Z;

            // Update bouding sphere
            _boundingSphere.Center.X = X + (_width / 2);
            _boundingSphere.Center.Y = Y + (_height / 2);
            _boundingSphere.Center.Z = Z + (_depth / 2);
            _boundingSphere.Radius = Math.Max(Math.Max(_width, _height), _depth) / 2;
        }

        #endregion

        /// <summary>
        /// Draw Terrain
        /// </summary>
        /// <param name="device"></param>
        public override void Draw(GraphicsDevice device)
        {
            PreDraw();

            foreach (EffectPass pass in _material.Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length, _indices, 0, _indices.Length / 3);
            }
        }
    }
}
