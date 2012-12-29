using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework;
using Yna.Framework.Display;
using Yna.Framework.Display3D;
using Yna.Framework.Display3D.Primitive;
using Yna.Framework.Display3D.Camera;
using Yna.Framework.Display3D.Terrain;
using Yna.Framework.Display3D.Controls;

namespace Yna.Samples.Screens
{
    public class CubeSample : YnState3D
    {
        YnEntity sky;
        SimpleTerrain terrain;
        FirstPersonControl control;
        YnGroup3D groupCube;

        public CubeSample(string name)
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
            terrain = new SimpleTerrain("pattern55_diffuse", 50, 50, 1, 1);
            // Repeat the ground texture 8x
            terrain.TextureRepeat = new Vector2(8.0f);
            Add(terrain);

            groupCube = new YnGroup3D(null);

            CubeShape cube = null;
            Vector3 cubePosition = new Vector3(0, 1, 35);

            for (int i = 0; i < 8; i++)
            {
                cubePosition.X +=  5;             

                cube = new CubeShape("pattern02_diffuse", Vector3.One, cubePosition);
                groupCube.Add(cube);
            }

            Add(groupCube);

            // Sky
            sky = new YnEntity("Sky");
        }

        public override void LoadContent()
        {
            base.LoadContent();

            // Sky
            sky.LoadContent();
            sky.SetFullScreen();

            // Set the camera position at the middle of the terrain
            _camera.Position = new Vector3(terrain.Width / 2, 2, terrain.Depth / 6);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0, l = groupCube.Count; i < l; i++)
                groupCube[i].RotateY(0.01f * (i + 1) * gameTime.ElapsedGameTime.Milliseconds);

            if (YnG.Mouse.Click(Framework.Input.MouseButton.Left))
            {
                Vector2 mousePosition = YnG.Mouse.Position;
                Vector3 position = YnG3.GetScreenToWorldPosition(Camera, ref mousePosition);
                Add(new CubeShape("pattern02_diffuse", Vector3.One, position));
            }

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.StateManager.SetScreenActive("menu", true);
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