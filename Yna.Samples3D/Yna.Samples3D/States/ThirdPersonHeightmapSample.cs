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
    public class ThirdPersonHeightmapSample : YnState
    {
        YnImage sky;
        YnText textInfo;

        Model3D alien;
        Heightmap terrain;
        ThirdPersonCamera camera;
        ThirdPersonControl control;

        RasterizerState rasterizerState;

        public ThirdPersonHeightmapSample()
            : base()
        {
            // 1 - Create an FPSCamera
            camera = new ThirdPersonCamera(7, -15, 0);
            camera.SetupCamera();

            // 2 Create a 3D Alien
            alien = new Model3D("Models/alien1_L", Vector3.Zero);
            alien.Scale = new Vector3(0.05f); // Change the scale because this object is too big :O
            alien.Camera = camera;

            // This camera will follow alien position
            camera.FollowedObject = alien;

            // 3 - Create a controler (Keyboard + Gamepad + mouse)
            // --- Setup move/rotate speeds
            control = new ThirdPersonControl(camera, alien);
            control.MoveSpeed = 0.1f;
            control.RotateSpeed = 0.3f;

            // 4 - Create a simple terrain with a size of 100x100 
            // -- 1. heightfield texture
            // -- 2. map texture applied on the terrain
            terrain = new Heightmap("Backgrounds/heightfield", "Backgrounds/textureMap");
            terrain.Camera = camera;

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

            // Load alien texture
            alien.LoadContent();

            // Load texture's terrain
            terrain.LoadContent();

            // Set the alien's position at the middle of the terrain
            alien.Position = new Vector3(terrain.Width / 2, 0, terrain.Depth / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.SwitchState(new GameMenu());

            // Update control
            control.Update(gameTime);

            // Basic collide detection with the ground
            alien.Y = terrain.GetTerrainHeight(alien.X, alien.Z);

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
            spriteBatch.End();

            // Restore default states for 3D
            YnG.GraphicsDevice.BlendState = BlendState.Opaque;
            YnG.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            // Wirefram or solid fillmode
            YnG.GraphicsDevice.RasterizerState = rasterizerState;

            // Draw the terrain
            terrain.Draw(YnG.GraphicsDevice);

            // Draw the alien
            alien.Draw(YnG.GraphicsDevice);

            spriteBatch.Begin();
            textInfo.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

