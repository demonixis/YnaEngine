﻿using System;
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
    public class NasaSample : YnState3D
    {
        YnImage background;

        YnModel stationModel;

        FirstPersonCamera camera;
        FirstPersonControl control;

        public NasaSample()
            : base()
        {
            background = new YnImage("Backgrounds/earth");

            camera = new FirstPersonCamera();
            Add(camera);

            stationModel = new YnModel("CommandCenter");
            Add(stationModel);

            control = new FirstPersonControl(camera);
            Add(control);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            background.LoadContent();
            background.SetFullScreen();
        }

        public override void Initialize()
        {
            base.Initialize();

            camera.Position = new Vector3(50.0f, 0.0f, -50.0f);
            camera.Yaw = -(float)Math.PI / 4;
        }

        public override void Update(GameTime gameTime)
        {
            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.SwitchState(new GameMenu());
            
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

            base.Draw(gameTime);
        }
    }
}
