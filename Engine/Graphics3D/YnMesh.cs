using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Engine.Graphics3D
{
    /// <summary>
    /// A mesh object that contains a geometry and a material.
    /// </summary>
    /// <typeparam name="T">Type of vertex to use with geometry</typeparam>
    public class YnMesh<T> : YnEntity3D where T : struct, IVertexType
    {
        private BaseGeometry<T> _geometry;
        private BaseMaterial _material;

        /// <summary>
        /// Create a new mesh with a geometry and a basic material without texture.
        /// </summary>
        /// <param name="geometry">Geometry to use.</param>
        public YnMesh(BaseGeometry<T> geometry)
            : this(geometry, "")
        {

        }

        /// <summary>
        /// Create a new mesh with a geometry and a basic material.
        /// </summary>
        /// <param name="geometry">Geometry to use.</param>
        /// <param name="textureName">Texture name to use with default material.</param>
        public YnMesh(BaseGeometry<T> geometry, string textureName)
            : this(geometry, new BasicMaterial(textureName))
        {

        }

        /// <summary>
        /// Create a new mesh with a geometry and a basic material without texture.
        /// </summary>
        /// <param name="geometry">Geometry to use.</param>
        /// <param name="material">Material to use.</param>
        public YnMesh(BaseGeometry<T> geometry, BasicMaterial material)
        {
            _geometry = geometry;
            _material = material;
            _width = geometry.SegmentSizes.X;
            _height = geometry.SegmentSizes.Y;
            _depth = geometry.SegmentSizes.Z;
        }

        public override void UpdateMatrix()
        {
            _world = Matrix.CreateScale(Scale) *
                Matrix.CreateFromYawPitchRoll(_rotation.Y, _rotation.X, _rotation.Z) *
                Matrix.CreateTranslation(Position);
        }

        public override void UpdateBoundingVolumes()
        {
            _boundingBox.Min.X = X;
            _boundingBox.Min.Y = Y;
            _boundingBox.Min.Z = Z;
            _boundingBox.Max.X = X + Width;
            _boundingBox.Max.Y = Y + Height;
            _boundingBox.Max.Z = Z + Depth;
            _boundingSphere.Center.X = X + (Width / 2);
            _boundingSphere.Center.Y = Y + (Height / 2);
            _boundingSphere.Center.Z = Z + (Depth / 2);
            _boundingSphere.Radius = Math.Max(Math.Max(Width, Height), Depth);
        }

        public override void LoadContent()
        {
            _material.LoadContent();
        }

        public virtual void PreDraw()
        {
            UpdateMatrix();

            if (_dynamic)
                UpdateBoundingVolumes();

            _material.Update(ref _world, Camera, ref _position);
        }

        public override void Draw(GraphicsDevice device)
        {
            PreDraw();
            _geometry.Draw(device, _material);
        }
    }
}
