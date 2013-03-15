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
    /// A mesh object that contains a geometry and a material.
    /// </summary>
    public class YnMesh : YnEntity3D
    {
        protected  BaseMaterial _material;
		
        /// <summary>
        /// Update matrices world and view. There are 3 updates
        /// 1 - Scale
        /// 2 - Rotation on Y axis (override if you wan't more)
        /// 3 - Translation
        /// </summary>
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
    }
}
