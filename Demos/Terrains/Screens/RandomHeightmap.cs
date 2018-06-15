using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine;
using Yna.Engine.Input;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Cameras;
using Yna.Engine.Graphics3D.Terrains;
using Yna.Engine.Graphics3D.Controls;
using Yna.Engine.Graphics3D.Materials;

namespace Yna.Samples.Screens
{
    public class RandomHeightmap : YnState3D
    {
        private YnEntity2D _sky;
        private YnText _textInfo;
        private Terrain _heightmap;
        private FirstPersonCamera _fpsCamera;
        private FirstPersonControl _control;
        private RasterizerState rasterizerState;

        public RandomHeightmap(string name)
            : base(name)
        {
            // 1 - Creating an FPSCamera
            _fpsCamera = new FirstPersonCamera();
            _fpsCamera.SetupCamera();
            Add(_fpsCamera);

            // 2 - Creating a controler (Keyboard + Gamepad + mouse)
            _control = new FirstPersonControl(_fpsCamera);
            _control.RotationSpeed = 0.45f;
            _control.MoveSpeed = 0.15f;
            _control.StrafeSpeed = 0.45f;
            _control.PhysicsPosition.MaxVelocity = 0.96f;
            _control.PhysicsRotation.MaxVelocity = 0.96f;
            Add(_control);

            // 3 - Create an Heigmap with 2 textures
            // -- 1. heightfield texture
            // -- 2. map texture applied on the terrain
            // Note : If you're using MonoGame and don't use xnb, you must use a jpg image for the heightfield

            var randomTexture = YnGraphics.CreateRandomTexture(128);

            _heightmap = new Terrain(randomTexture, "Textures/heightmapTexture", new Vector3(25, 1, 25));            
            _heightmap.Material = new BasicMaterial("Textures/heightmapTexture");
            Add(_heightmap);

            // Sky & debug info
            _sky = new YnEntity2D("Textures/Sky");
            _textInfo = new YnText("Fonts/DefaultFont", "F1 - Wireframe mode\nF2 - Normal mode");

            rasterizerState = new RasterizerState();

            SetFog(true, Color.White, 25, 100);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            // Sky
            _sky.LoadContent();
            _sky.SetFullScreen();

            // Debug text config
            _textInfo.LoadContent();
            _textInfo.Position = new Vector2(10, 10);
            _textInfo.Color = Color.Wheat;
            _textInfo.Scale = new Vector2(1.1f);
            _heightmap.UpdateBoundingVolumes();

            // Set the camera position at the middle of the terrain
            _fpsCamera.Position = new Vector3(_heightmap.Width / 2, 0, _heightmap.Depth / 2);
            _fpsCamera.Y = _heightmap.GetTerrainHeight(_fpsCamera.X, _fpsCamera.Y, _fpsCamera.Z) + 5;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.StateManager.SetActive("menu", true);

            // Naive Collide detection with ground
            // This method get the current segment height on the terrain and set the Y position of the camera at this value
            // We add 2 units because the camera is a bit higher than the ground
            _fpsCamera.Y = _heightmap.GetTerrainHeight(_fpsCamera.X, 0, _fpsCamera.Z) + 2;

            // Move the camera with a click
            if (YnG.Mouse.Click(MouseButton.Left))
            {
                Camera.Translate(0.0f, 0.0f, 0.4f);
            }

            // Rotate the camera with a click
            if (YnG.Mouse.Click(MouseButton.Right) || YnG.Mouse.Click(MouseButton.Left))
            {
                Camera.RotateY(-YnG.Mouse.Delta.X * 0.1f);
                Camera.RotateX(YnG.Mouse.Delta.Y * 0.1f);
            }
            // Restore default value
            else if (YnG.Mouse.Click(MouseButton.Middle))
            {
                Camera.Yaw = 0.0f;
                Camera.Pitch = 0.0f;
            }

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
            _sky.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            // Restore default states for 3D
            YnG.RestoreGraphicsDeviceStates();

            // Wirefram or solid fillmode
            YnG.GraphicsDevice.RasterizerState = rasterizerState;

            // Draw 3D
            base.Draw(gameTime);

            // Draw 2D part
            spriteBatch.Begin();
            _textInfo.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}