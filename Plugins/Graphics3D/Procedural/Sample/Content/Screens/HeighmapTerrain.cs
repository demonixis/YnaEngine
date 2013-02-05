using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework;
using Yna.Framework.Input;
using Yna.Framework.Display;
using Yna.Framework.Display3D;
using Yna.Framework.Display3D.Camera;
using Yna.Framework.Display3D.Terrain;
using Yna.Framework.Display3D.Controls;
using Yna.Framework.Display3D.Material;

namespace Yna.Samples.Screens
{
    public class HeighmapTerrain : YnState3D
    {

        YnEntity sky;
        YnText textInfo;

        Heightmap heightmap;
        FirstPersonCamera camera;
        FirstPersonControl control;

        RasterizerState rasterizerState;

        public HeighmapTerrain(string name)
            : base(name)
        {
            // 1 - Creating an FPSCamera
            camera = new FirstPersonCamera();
            camera.SetupCamera();
            Add(camera);

            // 2 - Creating a controler (Keyboard + Gamepad + mouse)
            control = new FirstPersonControl(camera);
            control.RotateSpeed = 0.45f;
            control.MoveSpeed = 0.15f;
            control.StrafeSpeed = 0.45f;
            control.MaxVelocityPosition = 0.96f;
            control.MaxVelocityRotation = 0.96f;
            Add(control);

            // 3 - Create an Heigmap with 2 textures
            // -- 1. heightfield texture
            // -- 2. map texture applied on the terrain
            // Note : If you're using MonoGame and don't use xnb, you must use a jpg image for the heightfield
            heightmap = new Heightmap("terrains/heightfield", "terrains/heightmapTexture");
            heightmap.Scale = new Vector3(2.5f, 1.0f, 2.5f);
            Add(heightmap);

            BasicMaterial heightmapMaterial = new BasicMaterial("terrains/heightmapTexture");
            heightmapMaterial.FogStart = 25.0f;
            heightmapMaterial.FogEnd = 75.0f;
            heightmapMaterial.EnableFog = true;
            heightmapMaterial.FogColor = Vector3.One * 0.3f;
            heightmap.Material = heightmapMaterial;

            // Sky & debug info
            sky = new YnEntity("Textures/Sky");
            textInfo = new YnText("Fonts/DefaultFont", "F1 - Wireframe mode\nF2 - Normal mode");

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
            heightmap.UpdateBoundingVolumes();

            // Set the camera position at the middle of the terrain
            camera.Position = new Vector3(heightmap.Width / 2, 0, heightmap.Depth / 2);
            camera.Y = heightmap.GetTerrainHeight(camera.X, camera.Y, camera.Z) + 5;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.StateManager.SetStateActive("menu", true);

            // Naive Collide detection with ground
            // This method get the current segment height on the terrain and set the Y position of the camera at this value
            // We add 2 units because the camera is a bit higher than the ground
            camera.Y = heightmap.GetTerrainHeight(camera.X, 0, camera.Z) + 2;

            // Move the camera with a click
            if (YnG.Mouse.Click(MouseButton.Left))
            {
                Camera.Translate(0.0f, 0.0f, 0.4f);
            }

            // Rotate the camera with a click
            if (YnG.Mouse.Click(MouseButton.Right) || YnG.Mouse.Click(MouseButton.Left))
            {
                Camera.RotateY(-YnG.Mouse.Delta.X * 0.1f);
                Camera.SetPitch(YnG.Mouse.Delta.Y * 0.1f);
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
            sky.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            // Restore default states for 3D
            YnG3.RestoreGraphicsDeviceStates();

            // Wirefram or solid fillmode
            YnG.GraphicsDevice.RasterizerState = rasterizerState;

            // Draw 3D
            base.Draw(gameTime);

            // Draw 2D part
            spriteBatch.Begin();
            textInfo.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}