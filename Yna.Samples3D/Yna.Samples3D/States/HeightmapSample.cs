using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Yna;
using Yna.State;
using Yna.Display;
using Yna.Display3D;
using Yna.Display3D.Camera;
using Yna.Display3D.Terrain;
using Yna.Display3D.Controls;

namespace Yna.Samples3D.States
{
    public class HeightmapSample : YnState
    {
        YnImage sky;
        YnText textInfo;
        YnText inputInfo;

        Heightmap heightmap;
        FirstPersonCamera camera;
        FirstPersonControl control;

        RasterizerState rasterizerState;

        public HeightmapSample()
            : base()
        {
            camera = new FirstPersonCamera();
            camera.SetupCamera();

            control = new FirstPersonControl(camera);

            heightmap = new Heightmap("Backgrounds/heightfield", "Backgrounds/textureMap");
            heightmap.Camera = camera;

            sky = new YnImage("Backgrounds/sky");
            textInfo = new YnText("Fonts/MenuFont", "F1 - Wireframe mode\nF2 - Normal mode");

            inputInfo = new YnText("Fonts/MenuFont", "A/E for Up/Down the camera\nZQSD for moving\nArrow keys for rotate\nPageUp/Down for pich Up/Down");

            rasterizerState = new RasterizerState();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            sky.LoadContent();
            sky.SetFullScreen();
            
            textInfo.LoadContent();
            textInfo.Position = new Vector2(10, 10);
            textInfo.Color = Color.Wheat;
            textInfo.Scale = new Vector2(1.1f);

            inputInfo.LoadContent();
            inputInfo.Position = new Vector2(YnG.Width - inputInfo.Width - 5, 10);
            inputInfo.Color = Color.Wheat;
            inputInfo.Scale = new Vector2(0.9f);

            heightmap.LoadContent();

            camera.Position = new Vector3(heightmap.Width / 2, 15, heightmap.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (YnG.Keys.JustPressed(Keys.Escape))
                YnG.SwitchState(new GameMenu());

            control.Update(gameTime);

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

            spriteBatch.Begin();
            sky.Draw(gameTime, spriteBatch);
            textInfo.Draw(gameTime, spriteBatch);
            inputInfo.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            YnG.GraphicsDevice.BlendState = BlendState.Opaque;
            YnG.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            YnG.GraphicsDevice.RasterizerState = rasterizerState;

            heightmap.Draw(YnG.GraphicsDevice);

            base.Draw(gameTime);
        }
    }
}
