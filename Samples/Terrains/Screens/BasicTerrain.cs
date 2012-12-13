using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display;
using Yna.Display3D;
using Yna.Display3D.Camera;
using Yna.Display3D.Terrain;
using Yna.Display3D.Controls;

namespace Yna.Samples.Screens
{
    public class BasicTerrain : YnState3D
    {
        YnImage sky;
        YnText textInfo;
        SimpleTerrain terrain;
        FirstPersonControl control;

        RasterizerState rasterizerState;

        public BasicTerrain(string name)
            : base(name)
        {
            // 1 - Create an FPSCamera
            _camera = new FirstPersonCamera();
            _camera.SetupCamera();
            Add(_camera);

            // 2 - Create a controler (Keyboard + Gamepad + mouse)
            // --- Setup move/rotate speeds
            control = new FirstPersonControl((FirstPersonCamera)_camera);
            control.MoveSpeed = 0.1f;
            control.StrafeSpeed = 0.05f;
            control.RotateSpeed = 0.6f;
            Add(control);

            // 3 - Create a simple terrain with a size of 100x100 with 1x1 space between each vertex
            terrain = new SimpleTerrain("heightmapTexture", 50, 50, 1, 1);
            Add(terrain);

            // Sky & debug text ;)
            sky = new YnImage("Sky");
            textInfo = new YnText("Font", "F1 - Wireframe mode\nF2 - Normal mode");

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

            // Set the camera position at the middle of the terrain
            _camera.Position = new Vector3(terrain.Width / 2, 2, terrain.Depth / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.ScreenManager.SetScreenActive("menu", true);

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
            YnG3.RestoreGraphicsDeviceStates();

            // Wirefram or solid fillmode
            YnG.GraphicsDevice.RasterizerState = rasterizerState;

            base.Draw(gameTime);
        }
    }
}