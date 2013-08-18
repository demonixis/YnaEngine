// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Yna.Engine.Graphics3D.Material;
using Yna.Engine.Graphics3D.Camera;

namespace Yna.Engine.Graphics3D
{
    /// <summary>
    /// A mesh object that contains a geometry and a material.
    /// </summary>
    public abstract class YnMesh : YnEntity3D
    {
        protected BaseMaterial _material;

        /// <summary>
        /// Gets or sets the material for this object
        /// </summary>
        public BaseMaterial Material
        {
            get { return _material; }
            set
            {
                _material = value;
                if (_initialized && !_material.Loaded)
                    _material.LoadContent();
            }
        }

        /// <summary>
        /// Update world matrix. (Scale, Rotation, Translation)
        /// </summary>
        public override void UpdateMatrix()
        {
            _world = Matrix.CreateScale(Scale) *
                Matrix.CreateFromYawPitchRoll(_rotation.Y, _rotation.X, _rotation.Z) *
                Matrix.CreateTranslation(Position);
        }

        /// <summary>
        /// Update BoundingBox and BoundingSphere
        /// </summary>
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

        /// <summary>
        /// Update matrix world, bounding volumes if the mesh is dynamic and update the material.
        /// </summary>
        public virtual void PreDraw(BaseCamera camera)
        {
            UpdateMatrix();
			
            if (_dynamic)
                UpdateBoundingVolumes();

            _material.Update(camera, ref _world);
        }
    }
}
