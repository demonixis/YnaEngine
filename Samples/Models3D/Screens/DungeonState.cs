using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Terrain;
using Yna.Engine.Graphics3D.Controls;

namespace Yna.Samples.Screens
{
    public class DungeonState : YnState3D
    {
        YnEntity sky;
        SimpleTerrain terrain;
        YnMeshModel dungeon;
        FirstPersonCamera camera;
        FirstPersonControl control;

        public DungeonState(string name)
            : base(name)
        {
            // 1 - Create an FPSCamera
            camera = new FirstPersonCamera();
            camera.SetupCamera();
            Add(camera);

            // 2 - Create a controler (Keyboard + Gamepad + mouse)
            // --- Setup move/rotate speeds
            control = new FirstPersonControl(camera);
            control.MoveSpeed = 1.2f;
            control.RotationSpeed = 1.2f;
            control.StrafeSpeed = 0.4f;
            Add(control);

            // 3 - Create a simple terrain with a size of 100x100 with 5x5 space between each vertex
            terrain = new SimpleTerrain("Backgrounds/textureMap", 100, 100, 5, 5);
            Add(terrain);

            // A dungeon !
            dungeon = new YnMeshModel("Models/Dungeon/dungeon");
            dungeon.RotationX = -(float)Math.PI / 2;
            dungeon.Scale = new Vector3(20.0f);
            Add(dungeon);

            Scene.SceneLight.AmbientIntensity = 1;
            Scene.SceneLight.DirectionalLights[0].Direction = new Vector3(1, -1, 0.5f);
            Scene.SceneLight.DirectionalLights[0].DiffuseColor = Color.White.ToVector3();
            Scene.SceneLight.DirectionalLights[0].SpecularColor = Color.Red.ToVector3();

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
            camera.Position = new Vector3(229, 12, 232);
            camera.Yaw = 1.9163f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.StateManager.SetStateActive("menu", true);
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

