using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Material;
using Yna.Engine.Graphics;

namespace Yna.Engine.Graphics3D
{
    public class YnBillboard : YnMeshGeometry
    {
        private SpriteBatch spriteBatch;
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
            spriteBatch = new SpriteBatch(YnG.GraphicsDevice);
            renderTarget = new RenderTarget2D(YnG.GraphicsDevice, (int)_entity.Width, (int)_entity.Height, true, SurfaceFormat.Color, DepthFormat.Depth24);
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device)
        {
            PreDraw();
            
            RenderTargetBinding[] oldRenderTargets = device.GetRenderTargets();

            device.SetRenderTarget(renderTarget);
            device.Clear(Color.Transparent);
            
            spriteBatch.Begin();
            _entity.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            device.SetRenderTarget((RenderTarget2D)null);
 
            _material.Texture = renderTarget;

            _geometry.Draw(device, _material);
        }
    }
}
