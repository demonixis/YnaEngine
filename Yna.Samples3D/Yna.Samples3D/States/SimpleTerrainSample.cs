using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Yna;
using Yna.State;
using Yna.Display;
using Yna.Display3D;
using Yna.Display3D.Camera;
using Yna.Display3D.Terrain;
using Yna.Display3D.Controls;

namespace Yna.Samples3D.States
{
    public class SimpleTerrainSample : YnState
    {
        YnImage sky;
        YnText textInfo;

        SimpleTerrain terrain;
        FirstPersonCamera camera;
        FirstPersonControl control;

        RasterizerState rasterizerState;

        public SimpleTerrainSample()
            : base()
        {
            // 1 - Create an FPSCamera
            camera = new FirstPersonCamera();
            camera.SetupCamera();

            // 2 - Create a controler (Keyboard + Gamepad + mouse)
            // --- Setup move/rotate speeds
            control = new FirstPersonControl(camera);
            control.MoveSpeed = 0.1f;
            control.RotateSpeed = 0.3f;

            // 3 - Create a simple terrain with a size of 100x100 
            terrain = new SimpleTerrain(100, 100, "Backgrounds/textureMap");
            terrain.Camera = camera; // The terrain use this camera

            // Sky & debug text ;)
            sky = new YnImage("Backgrounds/sky");
            textInfo = new YnText("Fonts/MenuFont", "F1 - Wireframe mode\nF2 - Normal mode");

            rasterizerState = new RasterizerState();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            // Sky
            sky.LoadContent();
            sky.SetFullScreen();
            
            // Debug text config
            textInfo.LoadContent();
            textInfo.Position = new Vector2(10, 10);
            textInfo.Color = Color.Wheat;
            textInfo.Scale = new Vector2(1.1f);

            // Load texture's terrain
            terrain.LoadContent();

            // Set the camera position at the middle of the terrain
            camera.Position = new Vector3(terrain.Width / 2, 2, terrain.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.SwitchState(new GameMenu());

            // Update control
            control.Update(gameTime);

            // Choose if you wan't wireframe or solid rendering
            if (YnG.Keys.JustPressed(Keys.F1) || YnG.Keys.JustPressed(Keys.F2))
            {
                if (YnG.Keys.JustPressed(Keys.F1))
                    rasterizerState = new RasterizerState() { FillMode = FillMode.WireFrame };

                else if (YnG.Keys.JustPressed(Keys.F2))
                    rasterizerState = new RasterizerState() { FillMode = FillMode.Solid };
            }
        }

        public override void Draw(GameTime gameTime)
        {
            YnG.GraphicsDevice.Clear(Color.Black);
             
            // Draw 2D part
            spriteBatch.Begin();
            sky.Draw(gameTime, spriteBatch);
            textInfo.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            // Restore default states for 3D
            YnG.GraphicsDevice.BlendState = BlendState.Opaque;
            YnG.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            // Wirefram or solid fillmode
            YnG.GraphicsDevice.RasterizerState = rasterizerState;

            // Draw the terrain
            terrain.Draw(YnG.GraphicsDevice);

            base.Draw(gameTime);
        }
    }
}
