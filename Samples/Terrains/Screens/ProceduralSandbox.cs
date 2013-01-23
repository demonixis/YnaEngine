using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework;
using Yna.Framework.Display;
using Yna.Framework.Display.Component;
using Yna.Framework.Display3D;
using Yna.Framework.Display3D.Camera;
using Yna.Framework.Display3D.Terrain;
using Yna.Framework.Display3D.Controls;
using Yna.Framework.Display3D.Primitive;
using Yna.Framework.Display3D.Procedural;

namespace Yna.Samples.Screens
{
    public class ProceduralSandbox : YnState3D
    {
        FirstPersonControl control;
        YnEntity sky;
        YnText textInfo;
        SimpleTerrain terrain;
        RasterizerState rasterizerState;
        YnVirtualPadController virtualPad;

        YnGroup3D _procVolumeGroup;
        ProcRuleset _ruleset;

        YnGroup _generatedMesh;

        public ProceduralSandbox(string name)
            : base(name)
        {
            // 1 - Create an FPSCamera
            _camera = new FirstPersonCamera();
            _camera.SetupCamera();
            Add(_camera);

            // 2 - Create a controler (Keyboard + Gamepad + mouse)
            // --- Setup move/rotate speeds
            control = new FirstPersonControl((FirstPersonCamera)_camera);
            control.RotateSpeed = 0.25f;
            control.MoveSpeed = 0.15f;
            control.StrafeSpeed = 0.25f;
            control.MaxVelocityPosition = 0.96f;
            control.MaxVelocityRotation = 0.96f;
            Add(control);

            // 3 - Create a simple terrain with a size of 100x100 with 1x1 space between each vertex
            terrain = new SimpleTerrain("terrains/heightmapTexture", 50, 50);
            //Add(terrain);

            // Sky & debug text ;)
            sky = new YnEntity("Textures/Sky");
            textInfo = new YnText("Fonts/DefaultFont", "F1 - Wireframe mode\nF2 - Normal mode");

            rasterizerState = new RasterizerState();

            virtualPad = new YnVirtualPadController();

            //_procVolume = new CubeShape("Textures/pattern02_diffuse", new Vector3(3, 3, 6), Vector3.Zero);
            //Add(_procVolume);
            _procVolumeGroup = new YnGroup3D(null);
            Add(_procVolumeGroup);

            _ruleset = new ProcRuleset();

            MeshRule meshRule = new MeshRule(_ruleset);
            meshRule.Add("Models/wall");

            RepeatXRule repeatRule = new RepeatXRule(_ruleset);
            repeatRule.RepeatSize = 1;
            repeatRule.RepeatedRule = meshRule;

            RepeatYRule repeatYRule = new RepeatYRule(_ruleset);
            repeatYRule.RepeatSize = 1;
            repeatYRule.RepeatedRule = repeatRule;

            MeshRule edgeMeshRule = new MeshRule(_ruleset);
            edgeMeshRule.Add("Models/corner");

            RepeatYRule edgeRepeatRule = new RepeatYRule(_ruleset);
            edgeRepeatRule.RepeatedRule = edgeMeshRule;
            edgeRepeatRule.RepeatSize = 1;

            FaceRule faceRule = new FaceRule(_ruleset);
            faceRule.MainRule = repeatYRule;
            faceRule.EdgeRule = edgeRepeatRule;

            TopBottomRule topBottomRule = new TopBottomRule(_ruleset);
            topBottomRule.MiddleRule = faceRule;

            _ruleset.RootRule = topBottomRule;
            Add(_ruleset.GeneratedMesh);
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

            virtualPad.LoadContent();

            terrain.Position = new Vector3(-terrain.Width / 2, 0, -terrain.Depth / 2);
            // Set the camera position at the middle of the terrain
            //_procVolume.Position = new Vector3(terrain.Width / 2, _procVolume.Height / 2, terrain.Depth / 6);

            //_camera.Position = new Vector3(_procVolume.X, 6, _procVolume.Z - 25);
            _camera.Position = new Vector3(0, 1, -5);

            Generate();
        }

        Vector3 virtalPadMovement = Vector3.Zero;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            virtualPad.Update(gameTime);

            virtalPadMovement = Vector3.Zero;

            if (virtualPad.Up)
                virtalPadMovement.Z = control.MoveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.001f;

            if (virtualPad.Down)
                virtalPadMovement.Z = -control.MoveSpeed * gameTime.ElapsedGameTime.Milliseconds * 0.001f;

            control.VelocityPosition += virtalPadMovement;
            control.UpdatePhysics(gameTime);
            Camera.Update(gameTime);

            if (YnG.Keys.JustPressed(Keys.Escape) || virtualPad.ButtonPause)
                YnG.StateManager.SetStateActive("menu", true);

            // Choose if you wan't wireframe or solid rendering
            if (YnG.Keys.JustPressed(Keys.F1) || YnG.Keys.JustPressed(Keys.F2))
            {
                if (YnG.Keys.JustPressed(Keys.F1))
                    rasterizerState = new RasterizerState() { FillMode = FillMode.WireFrame };

                else if (YnG.Keys.JustPressed(Keys.F2))
                    rasterizerState = new RasterizerState() { FillMode = FillMode.Solid };
            }

             if (YnG.Keys.JustPressed(Keys.Space))
             {
                 Generate();
             }
        }

        public override void Draw(GameTime gameTime)
        {
            YnG.GraphicsDevice.Clear(Color.Black);

            // Draw 2D part
            spriteBatch.Begin();
            sky.Draw(gameTime, spriteBatch);
            textInfo.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            // Restore default states for 3D
            YnG3.RestoreGraphicsDeviceStates();

            // Wirefram or solid fillmode
            YnG.GraphicsDevice.RasterizerState = rasterizerState;

            base.Draw(gameTime);

            virtualPad.DrawOnSingleBatch(gameTime, spriteBatch);
        }

        private void Generate()
        {
            _procVolumeGroup.Clear();
            _procVolumeGroup.Add(_ruleset.ProcVolume);
            _ruleset.Generate();

        }
    }
}