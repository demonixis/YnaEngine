using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Material;
using Yna.Engine.Graphics;
#if DEBUG
namespace Yna.Engine.Graphics3D
{
    public class YnBillboard : YnMeshGeometry
    {
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;
        private YnEntity _entity;

        public YnBillboard(YnEntity entity)
        {
            _entity = entity;
            _material = new BasicMaterial(YnGraphics.CreateTexture(Color.White, 1, 1));
            _rotation = new Vector3(-MathHelper.PiOver2, 0, MathHelper.Pi);
        }

        public override void LoadContent()
        {
            _entity.LoadContent();
            _entity.Initialize();
            _position.Y = 1;
            _geometry = new PlaneGeometry(new Vector3(1, 1, 1));
            _spriteBatch = new SpriteBatch(YnG.GraphicsDevice);
            _renderTarget = new RenderTarget2D(YnG.GraphicsDevice, (int)_entity.Width, (int)_entity.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 1, RenderTargetUsage.PlatformContents);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateEntity(gameTime, YnG.GraphicsDevice);
        }

        public virtual void UpdateEntity(GameTime gameTime, GraphicsDevice device)
        {
            _entity.Update(gameTime);

            device.SetRenderTarget(_renderTarget);
            device.Clear(Color.Transparent);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            _entity.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

            device.SetRenderTarget(null);

            _material.Texture = (Texture2D)_renderTarget;

            YnG3.RestoreGraphicsDeviceStates();
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, BaseCamera camera)
        {
            PreDraw(camera);

            _geometry.Draw(device, _material);
        }
    }
}
#endif