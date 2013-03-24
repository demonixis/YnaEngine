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
        private RenderTarget2D renderTarget;
        private YnEntity _entity;

        private YnBillboard(int width, int height)
            : base()
        {
            _geometry = new PlaneGeometry(new Vector3(width, height, 0));
            _material = new BasicMaterial();
            _rotation = new Vector3(MathHelper.PiOver2, 0, 0);
        }

        public YnBillboard(YnEntity entity)
            : this(entity.Width, entity.Height)
        {
            _entity = entity;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _entity.LoadContent();
            _spriteBatch = new SpriteBatch(YnG.GraphicsDevice);
            renderTarget = new RenderTarget2D(YnG.GraphicsDevice, (int)_entity.Width, (int)_entity.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 1, RenderTargetUsage.PlatformContents);
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, BaseCamera camera)
        {
            PreDraw(camera);
            
            device.SetRenderTarget(renderTarget);
           // device.Clear(Color.Transparent);
            
            _spriteBatch.Begin();
            _entity.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

            device.SetRenderTarget(null);

            device.BlendState = BlendState.AlphaBlend;
            device.DepthStencilState = DepthStencilState.None;
            device.RasterizerState = RasterizerState.CullCounterClockwise;
            device.SamplerStates[0] = SamplerState.LinearClamp;
            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.Default;
            device.SamplerStates[0] = SamplerState.LinearWrap;
            device.Clear(Color.Transparent);
            _material.Texture = (Texture2D)renderTarget;

            _geometry.Draw(device, _material);
        }
    }
}
#endif