// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Material;
using Yna.Engine.Graphics3D.Camera;

namespace Yna.Engine.Graphics3D
{
    /// <summary>
    /// A mesh who use a procedural geometry.
    /// </summary>
    public class YnMeshGeometry : YnMesh
    {
        protected BaseGeometry<VertexPositionNormalTexture> _geometry;
        protected Vector2 _textureRepeat;
        protected bool _worldMatrixIsMaster;

        /// <summary>
        /// Gets or sets the repeat value for texture.
        /// </summary>
        public Vector2 TextureRepeat
        {
            get { return _geometry.TextureRepeat; }
            set { _geometry.TextureRepeat = value; }
        }

        /// <summary>
        /// Gets the geometry of the mesh.
        /// </summary>
        public BaseGeometry<VertexPositionNormalTexture> Geometry
        {
            get { return _geometry; }
            protected set { _geometry = value; }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public bool WorldMatrixIsMaster
        {
            get { return _worldMatrixIsMaster; }
            set { _worldMatrixIsMaster = value; }
        }

        #region Constructors

        /// <summary>
        /// Create an YnMeshGeometry without geometry and material. Don't forget to set it before draw.
        /// </summary>
        public YnMeshGeometry()
            : this(null, "")
        {

        }

        /// <summary>
        /// Create an YnMeshGeoemtry with a geometry and a BasicMaterial without texture.
        /// </summary>
        /// <param name="geometry"></param>
        public YnMeshGeometry(BaseGeometry<VertexPositionNormalTexture> geometry)
            : this(geometry, "")
        {

        }

        /// <summary>
        /// Create an YnMeshGeometry with a geometry and a BasicMaterial.
        /// </summary>
        /// <param name="geometry">Geometry to use.</param>
        /// <param name="textureName">Texture name to use with BasicMaterial</param>
        public YnMeshGeometry(BaseGeometry<VertexPositionNormalTexture> geometry, string textureName)
            : this(geometry, new BasicMaterial(textureName))
        {

        }

        /// <summary>
        /// Create an YnMeshGeometry with a geometry and a material.
        /// </summary>
        /// <param name="geometry">Geometry to use.</param>
        /// <param name="material">Material to use with geometry.</param>
        public YnMeshGeometry(BaseGeometry<VertexPositionNormalTexture> geometry, BaseMaterial material)
            : base()
        {
            _geometry = geometry;
            _material = material;
            _worldMatrixIsMaster = false;
        }

        #endregion

        /// <summary>
        /// Update BoundingBox and BoundingSphere
        /// </summary>
        public override void UpdateBoundingVolumes()
        {
            _boundingBox = new BoundingBox(new Vector3(float.MaxValue), new Vector3(float.MinValue));

            for (int i = 0; i < _geometry.Vertices.Length; i++)
            {
                _boundingBox.Min.X = Math.Min(_boundingBox.Min.X, _geometry.Vertices[i].Position.X + X);
                _boundingBox.Min.Y = Math.Min(_boundingBox.Min.Y, _geometry.Vertices[i].Position.Y + Y);
                _boundingBox.Min.Z = Math.Min(_boundingBox.Min.Z, _geometry.Vertices[i].Position.Z + Z);

                _boundingBox.Max.X = Math.Max(_boundingBox.Max.X, _geometry.Vertices[i].Position.X + X);
                _boundingBox.Max.Y = Math.Max(_boundingBox.Max.Y, _geometry.Vertices[i].Position.Y + Y);
                _boundingBox.Max.Z = Math.Max(_boundingBox.Max.Z, _geometry.Vertices[i].Position.Z + Z);
            }

            _width = _boundingBox.Max.X - _boundingBox.Min.X;
            _height = _boundingBox.Max.Y - _boundingBox.Min.Y;
            _depth = _boundingBox.Max.Z - _boundingBox.Min.Z;

            int radius = (int)Math.Max(Math.Max(_width, _height), _depth);

            _boundingSphere = new BoundingSphere(
                new Vector3(X + Width / 2, Y + Height / 2, Z + Depth / 2),
                radius);
        }

        /// <summary>
        /// Generate geometry and load material.
        /// </summary>
        public override void LoadContent()
        {
            _geometry.GenerateGeometry();
            _material.LoadContent();
            UpdateBoundingVolumes();
        }

        /// <summary>
        /// Update lights.
        /// </summary>
        /// <param name="light"></param>
        public override void UpdateLighting(SceneLight light)
        {
            if (_material != null)
                _material.Light = light;
        }

        /// <summary>
        /// Update world matrix. (Scale, Rotation, Translation)
        /// </summary>
        public override void UpdateMatrix()
        {
            if (!_worldMatrixIsMaster)
            {
                _world = Matrix.CreateScale(Scale) *
                    Matrix.CreateFromYawPitchRoll(_rotation.Y, _rotation.X, _rotation.Z) *
                    Matrix.CreateTranslation(Position);
            }
        }

        /// <summary>
        /// Draw mesh.
        /// </summary>
        /// <param name="device"></param>
        public override void Draw(GameTime gameTime, GraphicsDevice device, BaseCamera camera)
        {
            PreDraw(camera);
            _geometry.Draw(device, _material);
        }
    }
}
