using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Engine.Graphics3D
{
    /// <summary>
    /// A mesh who use a procedural geometry.
    /// </summary>
    public class YnMeshGeometry : YnMesh
    {
        protected BaseGeometry<VertexPositionNormalTexture> _geometry;
        protected Vector2 _textureRepeat;

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
        }

        #endregion

        /// <summary>
        /// Generate geometry and load material.
        /// </summary>
        public override void LoadContent()
        {
            _geometry.GenerateGeometry();
            _material.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw Terrain
        /// </summary>
        /// <param name="device"></param>
        public override void Draw(GraphicsDevice device)
        {
            PreDraw();
            _geometry.Draw(device, _material);
        }
    }
}
