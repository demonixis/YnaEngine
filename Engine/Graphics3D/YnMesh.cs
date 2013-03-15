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
    public abstract class YnMesh : YnEntity3D
    {
        protected BaseMaterial _material;

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
           UpdateMatrix();

            if (_dynamic)
                UpdateBoundingVolumes();

            _material.Update(Camera, ref _world);
        }
    }
}
