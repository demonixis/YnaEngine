// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Engine.Graphics3D
{
    public class YnBillboard : YnMeshGeometry
    {
        private bool _isFixed;

        public YnBillboard(string textureName, float width, float height)
        {
            _material = new BasicMaterial(textureName);
            _rotation = new Vector3(-MathHelper.PiOver2, 0, MathHelper.Pi);
            _width = width;
            _height = height;
            _isFixed = true;
        }

        public override void LoadContent()
        {
            _width = _width <= 0 ? 1 : _width;
            _height = _height <= 0 ? 1 : _height;


            _position.Y = 1;
            _geometry = new PlaneGeometry(new Vector3(_width, 1, _height));
            base.LoadContent();
        }

        public void UpdateRotation(BaseCamera camera)
        {
            Vector3 reference = Vector3.Backward;
            Vector3 lookDirection = Vector3.Normalize(camera.Position - camera.Target);
            float dot = Vector3.Dot(reference, lookDirection);
            float angle = (float)Math.Acos(dot / (reference.Length() * lookDirection.Length()));

            if (lookDirection.X < reference.X)
                angle = -angle;

            _world = Matrix.CreateScale(_scale) * 
                Matrix.CreateFromYawPitchRoll(angle + _rotation.Y, _rotation.X, _rotation.Z) * 
                Matrix.CreateTranslation(_position);
        }

        public override void PreDraw(BaseCamera camera)
        {
            if (_isFixed)
            {
                _world = Matrix.CreateScale(_scale) *
                    Matrix.CreateFromYawPitchRoll(_rotation.Y, _rotation.X, _rotation.Z) *
                    Matrix.CreateTranslation(_position);
            }

            if (_dynamic)
                UpdateBoundingVolumes();

            _material.Update(camera, ref _world);
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, BaseCamera camera)
        {
            PreDraw(camera);

            device.BlendState = BlendState.AlphaBlend;

            _geometry.Draw(device, _material);

            device.BlendState = BlendState.Opaque;
        }
    }
}