using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna;
using Yna.Display;
using Yna.Input;
using Yna.State;
using Yna.Display3D;
using Yna.Display3D.Camera;
using Yna.Display3D.Controls;

namespace Yna.Samples3D.States
{
    public class NasaSample : YnState
    {
        YnImage background;

        Model stationModel;
        Vector3 modelPosition;
        Vector3 modelRotation; 

        FirstPersonCamera camera;
        FirstPersonControl control;

        public NasaSample()
            : base()
        {
            background = new YnImage("Backgrounds/earth");

            modelPosition = Vector3.Zero;
            modelRotation = Vector3.Zero;

            camera = new FirstPersonCamera();
            control = new FirstPersonControl(camera);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            stationModel = YnG.Content.Load<Model>("Models/ISSComplete1");
            background.LoadContent();
            background.SetFullScreen();
        }

        public override void Initialize()
        {
            base.Initialize();

            camera.Position = new Vector3(50.0f, 0.0f, -50.0f);
            camera.Yaw = -0.69f;
        }

        public override void Update(GameTime gameTime)
        {
            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.SwitchState(new GameMenu());

            control.Update(gameTime);
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            background.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            // When we draw with a SpriteBatch, BlendState & DepthStencilState are changed
            // Reset defaults states
            YnG.GraphicsDevice.BlendState = BlendState.Opaque;
            YnG.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            // Setup transforms
            Matrix[] transforms = new Matrix[stationModel.Bones.Count];
            stationModel.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw each meshes off the model
            foreach (ModelMesh mesh in stationModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = camera.World * transforms[mesh.ParentBone.Index];

                    effect.View = camera.View;

                    effect.Projection = camera.Projection;
                }
                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
