using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna;
using Yna.Input;
using Yna.State;
using Yna.Display3D;

namespace Yna.Samples3D.States
{
    public class NasaSample : YnState
    {
        Texture2D background;

        private float aspectRatio;

        Model stationModel;
        Vector3 modelPosition;
        Vector3 modelRotation; 

        Camera3D camera;

        public NasaSample()
            : base()
        {
            modelPosition = Vector3.Zero;
            modelRotation = Vector3.Zero;

            camera = new Camera3D(YnG.Game);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            stationModel = YnG.Content.Load<Model>("Models/ISSComplete1");
            aspectRatio = YnG.GraphicsDevice.Viewport.AspectRatio;
        }

        public override void Initialize()
        {
            base.Initialize();

            camera.Initialize();
            camera.Position = new Vector3(50.0f, 0.0f, -50.0f);
            camera.Yaw = -0.69f;
        }

        public override void Update(GameTime gameTime)
        {
            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.SwitchState(new GameMenu());

            camera.Update(gameTime);

            if (YnG.Keys.Pressed(Keys.Z) || YnG.Keys.Up)
                camera.Translate(new Vector3(0, 0, 0.65f));
            else if (YnG.Keys.Pressed(Keys.S) || YnG.Keys.Down)
                camera.Translate(new Vector3(0, 0, -0.65f));

            if (YnG.Keys.Pressed(Keys.Q))
                camera.Translate(new Vector3(0.65f, 0, 0));
            else if (YnG.Keys.Pressed(Keys.D))
                camera.Translate(new Vector3(-0.65f, 0, 0));

            if (YnG.Keys.Left)
                camera.RotateY(0.75f);
            else if (YnG.Keys.Right)
                camera.RotateY(-0.75f);
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // Setup transforms
            Matrix[] transforms = new Matrix[stationModel.Bones.Count];
            stationModel.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw each meshes off the model
            foreach (ModelMesh mesh in stationModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World =
                        transforms[mesh.ParentBone.Index];

                    effect.View = camera.View;

                    effect.Projection = camera.Projection;
                }
                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
