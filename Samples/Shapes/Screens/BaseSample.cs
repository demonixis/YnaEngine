using Microsoft.Xna.Framework;
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
    public class BaseSample : YnState3D
    {
        protected SimpleTerrain terrain;
        protected FirstPersonControl control;
        protected SkyBox skybox;

        public BaseSample(string name)
            : this(name, false)
        {

        }

        public BaseSample(string name, bool night)
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
            control.RotationSpeed = 0.45f;
            control.MaxVelocityPosition = 0.95f;
            control.MaxVelocityRotation = 0.96f;
            Add(control);

            // 3 - Create a simple terrain with a size of 50x50 with 1x1 space between each vertex
            terrain = new SimpleTerrain("Textures/sand", 100, 100);
            // Repeat the ground texture 8x
            terrain.TextureRepeat = new Vector2(8.0f);
            Add(terrain);

            string[] skyboxTextures;

            if (night)
            {
                skyboxTextures = new string[6]
                {
                    "Textures/galaxy/galaxy-X",
                    "Textures/galaxy/galaxy+X",
                    "Textures/galaxy/galaxy-Y",
                    "Textures/galaxy/galaxy+Y",
                    "Textures/galaxy/galaxy-Z",
                    "Textures/galaxy/galaxy+Z"
                };
            }
            else
            {
                skyboxTextures = new string[6]
                {
                    "Textures/skybox/nx",
                    "Textures/skybox/px",
                    "Textures/skybox/ny",
                    "Textures/skybox/py",
                    "Textures/skybox/nz",
                    "Textures/skybox/pz"
                };
            }

            skybox = new SkyBox(null, Vector3.Zero, 100, skyboxTextures);
            Add(skybox);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Camera.Position = new Vector3(terrain.Width / 2, 2, terrain.Depth / 6);
            skybox.Position = new Vector3(terrain.Width / 2, skybox.Height / 3, terrain.Depth / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Camera.Y < 1)
                Camera.Y = 1;
            else if (Camera.Y > 50)
                Camera.Y = 50;

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.StateManager.SetStateActive("menu", true);
        }

        public override void Draw(GameTime gameTime)
        {
            YnG.GraphicsDevice.Clear(Color.Black);

            // Restore default states for 3D
            YnG3.RestoreGraphicsDeviceStates();

            base.Draw(gameTime);
        }
    }
}