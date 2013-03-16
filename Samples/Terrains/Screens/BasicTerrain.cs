﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Component;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Terrain;
using Yna.Engine.Graphics3D.Controls;

namespace Yna.Samples.Screens
{
    public class BasicTerrain : YnState3D
    {
        // Two controls
        FirstPersonControl control;
        YnVirtualPadController virtualPad;

        YnEntity sky;
        SimpleTerrain terrain;
        YnModel alienModel;
        
        // VirtualPad vectors
        private Vector3 virtalPadMovement = Vector3.Zero;
        private Vector3 virtualPadRotation = Vector3.Zero;

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
            control.RotationSpeed = 0.45f;
            control.MoveSpeed = 0.15f;
            control.StrafeSpeed = 0.45f;
            control.MaxVelocityPosition = 0.96f;
            control.MaxVelocityRotation = 0.96f;
            Add(control);

            // 3 - The virtual pad
            virtualPad = new YnVirtualPadController();
     
            // 4 - Create a simple terrain with a size of 100x100 with 1x1 space between each vertex
            terrain = new SimpleTerrain("terrains/heightmapTexture", 50, 50, 10, 10);
            Add(terrain);

            // 5 - And add an alien
            alienModel = new YnModel("Models/Alien/alien1_L");
            Add(alienModel);

            // 6 - A fake sky
            sky = new YnEntity("Textures/Sky");
        }

        public override void LoadContent()
        {
            base.LoadContent();

            // Sky
            sky.LoadContent();
            sky.SetFullScreen();

            virtualPad.LoadContent();

            // Set the camera position at the middle of the terrain
            _camera.Position = new Vector3(terrain.Width / 2, 2, terrain.Depth / 6);

            alienModel.Position = new Vector3(
                (terrain.Width / 2) - (alienModel.Width / 2),
                0,
                (terrain.Depth / 2) - (alienModel.Depth / 2));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            virtualPad.Update(gameTime);

            virtalPadMovement = Vector3.Zero;
            virtualPadRotation = Vector3.Zero;

            if (virtualPad.Pressed(PadButtons.Up))
                virtalPadMovement.Z = control.MoveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.005f;

            else if (virtualPad.Pressed(PadButtons.Down))
                virtalPadMovement.Z = -control.MoveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.005f;

            if (virtualPad.Pressed(PadButtons.Left))
                virtalPadMovement.X = control.MoveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.005f;

            else if (virtualPad.Pressed(PadButtons.Right))
                virtalPadMovement.X = -control.MoveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.005f;

            if (virtualPad.Pressed(PadButtons.StrafeLeft))
                virtualPadRotation.Y = control.RotationSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.005f;

            else if (virtualPad.Pressed(PadButtons.StrafeRight))
                virtualPadRotation.Y = -control.RotationSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.005f;

            if (virtualPad.Pressed(PadButtons.ButtonA))
                virtalPadMovement.Y = control.MoveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.005f;

            else if (virtualPad.Pressed(PadButtons.ButtonB))
                virtalPadMovement.Y = -control.MoveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.005f;

            control.VelocityPosition += virtalPadMovement;
            control.VelocityRotation += virtualPadRotation;
            control.UpdatePhysics(gameTime);
            
            // Back to the menu
            if (YnG.Keys.JustPressed(Keys.Escape) || virtualPad.Pressed(PadButtons.Pause))
                YnG.StateManager.SetStateActive("menu", true);
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

            base.Draw(gameTime);

            virtualPad.DrawOnSingleBatch(gameTime, spriteBatch);
        }
    }
}