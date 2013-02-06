﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Terrain;
using Yna.Engine.Graphics3D.Controls;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Samples.Screens
{
    public class IcosphereSample : YnState3D
    {
        YnEntity sky;
        SimpleTerrain terrain;
        FirstPersonControl control;
        IcoSphereGeometry icosphere;

        public IcosphereSample(string name)
            : base(name)
        {
            // 1 - Create an FPSCamera
            _camera = new FirstPersonCamera();
            _camera.SetupCamera();
            Add(_camera);

            // 2 - Create a controler (Keyboard + Gamepad + mouse)
            // --- Setup move/rotate speeds
            control = new FirstPersonControl((FirstPersonCamera)_camera);
            control.MoveSpeed = 0.15f;
            control.StrafeSpeed = 0.05f;
            control.RotateSpeed = 0.45f;
            control.MaxVelocityPosition = 0.95f;
            control.MaxVelocityRotation = 0.96f;
            Add(control);

            // 3 - Create a simple terrain with a size of 50x50 with 1x1 space between each vertex
            terrain = new SimpleTerrain("Textures/pattern55_diffuse", 50, 50, 1, 1);
            // Repeat the ground texture 8x
            terrain.TextureRepeat = new Vector2(8.0f);
            Add(terrain);

            icosphere = new IcoSphereGeometry("Textures/icosphere_map", 32, 4, false);
            icosphere.Scale = new Vector3(3.5f);
            Add(icosphere);

            // Use another material
            //BasicMaterial icoMaterial = new BasicMaterial("icosphere_map");
            EnvironmentMapMaterial icoMaterial = new EnvironmentMapMaterial("Textures/icosphere_map", new string[] { "Textures/Sky" });
            icoMaterial.SpecularColor = new Color(5, 5, 5).ToVector3();
            icoMaterial.AmbientColor = Color.White.ToVector3();
            icoMaterial.Amount = 0.35f;
            icoMaterial.FresnelFactor = 0.25f;
            icoMaterial.FogColor = Color.White.ToVector3();
            icoMaterial.FogStart = 15.0f;
            icoMaterial.FogEnd = 65.0f;
            icoMaterial.EnableFog = true;

            // Set the new material to the sphere
            icosphere.Material = icoMaterial;

            BasicMaterial terrainMaterial = new BasicMaterial("Textures/pattern55_diffuse");
            terrainMaterial.FogColor = Color.White.ToVector3();
            terrainMaterial.FogStart = 15.0f;
            terrainMaterial.FogEnd = 65.0f;
            terrainMaterial.EnableFog = true;
            terrain.Material = terrainMaterial;

            // Sky
            sky = new YnEntity("Textures/Sky");
        }

        public override void LoadContent()
        {
            base.LoadContent();

            // Sky
            sky.LoadContent();
            sky.SetFullScreen();

            // Set the camera position at the middle of the terrain
            icosphere.Position = new Vector3(terrain.Width / 2, icosphere.Scale.Y, terrain.Depth / 2);

            _camera.Position = new Vector3(terrain.Width / 2, 2, terrain.Depth / 6);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            icosphere.RotateY(gameTime.ElapsedGameTime.Milliseconds * 0.1f);

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.StateManager.SetStateActive("menu", true);
        }

        public override void Draw(GameTime gameTime)
        {
            YnG.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            sky.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            // Restore default states for 3D
            YnG3.RestoreGraphicsDeviceStates();

            base.Draw(gameTime);
        }
    }
}