using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework;
using Yna.Framework.Display;
using Yna.Framework.Display3D;
using Yna.Framework.Display3D.Camera;
using Yna.Framework.Display3D.Terrain;
using Yna.Framework.Display3D.Controls;

namespace Yna.Samples
{
    public class DungeonState : YnState3D
    {
        YnEntity sky;
        SimpleTerrain terrain;
        YnModel dungeon;
        FirstPersonControl control;

        public DungeonState(string name)
            : base(name)
        {
            // 1 - Create an FPSCamera
            _camera = new FirstPersonCamera();
            _camera.SetupCamera();
            Add(_camera);

            // 2 - Create a controler (Keyboard + Gamepad + mouse)
            // --- Setup move/rotate speeds
            control = new FirstPersonControl((FirstPersonCamera)_camera);
            control.MoveSpeed = 1.2f;
            control.RotateSpeed = 1.2f;
            control.StrafeSpeed = 0.4f;
            Add(control);

            // 3 - Create a simple terrain with a size of 100x100 with 5x5 space between each vertex
            terrain = new SimpleTerrain("Backgrounds/textureMap", 100, 100, 5, 5);
            Add(terrain);

            // A dungeon !
            dungeon = new YnModel("Models/Dungeon/dungeon");
            dungeon.RotationX = -(float)Math.PI / 2;
            dungeon.Scale = new Vector3(20.0f);
            Add(dungeon);

            // Sky & debug text ;)
            sky = new YnEntity("Backgrounds/sky");
        }

        public override void LoadContent()
        {
            base.LoadContent();

            // Sky
            sky.LoadContent();
            sky.SetFullScreen();

            dungeon.Position = new Vector3(terrain.Width / 2, 5, terrain.Depth / 2);
            _camera.Position = new Vector3(229, 12, 232);
            _camera.Yaw = 1.9163f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.StateManager.SetScreenActive("menu", true);
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw 2D part
            spriteBatch.Begin();
            sky.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            // Restore default state before draw 3D
            YnG3.RestoreGraphicsDeviceStates();

            base.Draw(gameTime);
        }
    }
}

