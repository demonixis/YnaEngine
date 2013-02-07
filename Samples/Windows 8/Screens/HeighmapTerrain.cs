using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine;
using Yna.Engine.Input;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Component;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Terrain;
using Yna.Engine.Graphics3D.Controls;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Samples.Screens
{
    public class HeighmapTerrain : YnState3D
    {
        private YnEntity sky;
        private YnText textInfo;

        private Heightmap heightmap;
        private FirstPersonCamera camera;
        private FirstPersonControl control;

        private RasterizerState rasterizerState;

        private YnVirtualPadController virtualPadController;

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
            heightmap.Scale = new Vector3(2.5f, 2.5f, 2.5f);
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

            // Virtual pad
            virtualPadController = new YnVirtualPadController();
            virtualPadController.VirtualPad.InverseDirectionStrafe = true;
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

            virtualPadController.LoadContent();

            // Set the camera position at the middle of the terrain
            camera.Position = new Vector3(heightmap.Width / 2, 0, heightmap.Depth / 2);
            camera.Y = heightmap.GetTerrainHeight(camera.X, camera.Y, camera.Z) + 5;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            virtualPadController.Update(gameTime);

            // Naive Collide detection with ground
            // This method get the current segment height on the terrain and set the Y position of the camera at this value
            // We add 2 units because the camera is a bit higher than the ground
            camera.Y += (heightmap.GetTerrainHeight(camera.X, 0, camera.Z) + 5 - camera.Y) * 0.2f;


            // Move
            if (virtualPadController.Pressed(PadButtons.Up))
                Camera.Translate(0.0f, 0.0f, 0.6f);
            else if (virtualPadController.Pressed(PadButtons.Down))
                Camera.Translate(0.0f, 0.0f, -0.6f);

            // Strafe
            if (virtualPadController.Pressed(PadButtons.StrafeLeft))
                Camera.Translate(0.5f, 0.0f, 0.0f);
            else if (virtualPadController.Pressed(PadButtons.StrafeRight))
                Camera.Translate(-0.5f, 0.0f, 0.0f);

            // Rotate 
            if (virtualPadController.Pressed(PadButtons.Left))
                Camera.RotateY(0.8f);
            else if (virtualPadController.Pressed(PadButtons.Right))
                Camera.RotateY(-0.8f);

            // Choose if you wan't wireframe or solid rendering
            if (virtualPadController.Pressed(PadButtons.Pause))
            {
                if (rasterizerState.FillMode == FillMode.Solid)
                    rasterizerState = new RasterizerState() { FillMode = FillMode.WireFrame };
                else
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
            virtualPadController.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}