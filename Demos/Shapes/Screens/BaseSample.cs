using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Cameras;
using Yna.Engine.Graphics3D.Terrains;
using Yna.Engine.Graphics3D.Controls;

namespace Yna.Samples.Screens
{
    public class BaseSample : YnState3D
    {
        protected Terrain terrain;
        protected FirstPersonCamera camera;
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
            camera = new FirstPersonCamera();
            camera.SetupCamera();
            Add(camera);

            // 2 - Create a controler (Keyboard + Gamepad + mouse)
            // --- Setup move/rotate speeds
            control = new FirstPersonControl(camera);
            control.MoveSpeed = 0.15f;
            control.StrafeSpeed = 0.05f;
            control.RotationSpeed = 0.45f;
            control.PhysicsPosition.MaxVelocity = 0.96f;
            control.PhysicsRotation.MaxVelocity = 0.96f;
            control.EnableMouse = false;
            control.EnableGamepad = false;
            Add(control);

            // 3 - Create a simple terrain with a size of 50x50 with 1x1 space between each vertex
            terrain = new Terrain("Textures/sand", 100, 100);
            // Repeat the ground texture 8x
            terrain.TextureRepeat = new Vector2(8.0f);
            Add(terrain);

            var skyboxTextures = new string[6]
            {
                "Textures/galaxy/galaxy-X",
                "Textures/galaxy/galaxy+X",
                "Textures/galaxy/galaxy-Y",
                "Textures/galaxy/galaxy+Y",
                "Textures/galaxy/galaxy-Z",
                "Textures/galaxy/galaxy+Z"
            };

            if (!night)
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

            skybox = new SkyBox(Vector3.Zero, 100, skyboxTextures);
            Add(skybox);

            SetFog(true, Color.WhiteSmoke);

            // Setup lighting
            SceneLight.DirectionalLights[0].DiffuseColor = Color.WhiteSmoke.ToVector3();
            SceneLight.DirectionalLights[0].DiffuseIntensity = 2.5f;
            SceneLight.DirectionalLights[0].Direction = new Vector3(1, 0, 0);
            SceneLight.SpecularColor = Color.Gray.ToVector3();
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
                YnG.StateManager.SetActive("menu", true);
        }

        public override void Draw(GameTime gameTime)
        {
            YnG.GraphicsDevice.Clear(Color.Black);

            // Restore default states for 3D
            YnG.RestoreGraphicsDeviceStates();

            base.Draw(gameTime);
        }
    }
}