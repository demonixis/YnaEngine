using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Display.Gui;
using Yna.Display.Event;

namespace Yna.Samples.States
{
    /// <summary>
    /// Description of UIExample.
    /// </summary>
    public class UIExample : YnState
    {
        private YnGui gui;
        private Texture2D background;
        private YnProgressBar progress;
        private YnLabel progressLabel;

        public UIExample(string name)
            : base(name, 0, 0)
        {
            YnG.ShowMouse = true;
            gui = new YnGui(YnSkinGenerator.Generate(Color.DodgerBlue, "Fonts/DefaultFont"));
        }

        public override void BuildGui()
        {
            // Buttons in a toolbar
            int tileSize = 100;
            int padding = 10;
            int tileCount = 5;

            YnPanel toolbar = new YnPanel();
            gui.Add(toolbar); // Add toolbar to Gui

            toolbar.WithBackground = false;
            toolbar.Orientation = YnOrientation.Horizontal;
            toolbar.Padding = padding;
            toolbar.Height = tileSize;
            toolbar.Position = new Vector2(YnG.Width / 2 - (tileSize * tileCount + padding * (tileCount + 1)) / 2, YnG.Height / 2 - toolbar.Height / 2);

            toolbar.Add(new YnTextButton("New", tileSize, tileSize, false));
            toolbar.Add(new YnTextButton("Load", tileSize, tileSize, false));
            toolbar.Add(new YnTextButton("Options", tileSize, tileSize, false));
            toolbar.Add(new YnTextButton("Store", tileSize, tileSize, false));

            YnTextButton button = toolbar.Add(new YnTextButton("Exit", tileSize, tileSize, false));
            button.MouseJustClicked += (s, e) =>
            {
                gui.Clear();

                YnG.ScreenManager.SetScreenActive("menu", true);
            };

            progress = new YnProgressBar();
            gui.Add(progress);

            progress.Width = 400;
            progress.MaxValue = 400;
            progress.Height = 5;
            progress.Position = new Vector2(YnG.Width / 2 - progress.Width / 2, 100);

            progressLabel = new YnLabel("Fake loading...");
            gui.Add(progressLabel);

            progressLabel.Position = new Vector2(YnG.Width / 2 - progress.Width / 2, 75);

            YnPanel container = new YnPanel();
            gui.Add(container);

            container.Orientation = YnOrientation.Horizontal;
            container.WithBackground = false;
            container.Position = new Vector2(20, 20);
            
            YnLabel modeLabel = container.Add(new YnLabel("Hard Mode "));
            YnCheckbox check = container.Add(new YnCheckbox());

            // Slider
            YnSlider slider = new YnSlider();
            gui.Add(slider);
            slider.Width = 200;
            slider.Height = 20;
            slider.Position = new Vector2(20, 200);
            slider.MaxValue = 10;

            YnProgressBar verticalGauge = new YnProgressBar();
            verticalGauge.Orientation = YnOrientation.Vertical;
            gui.Add(verticalGauge);

            YnTextButton plusButton = new YnTextButton();
            plusButton.Text = "+";
            plusButton.Width = 30;
            plusButton.Height = 30;
            plusButton.MouseClick += (o, e) =>
            {
                verticalGauge.Value++;
                Console.WriteLine(verticalGauge.Value);
            };
            gui.Add(plusButton);

            YnTextButton minusButton = new YnTextButton();
            minusButton.Text = "-";
            minusButton.Width = 30;
            minusButton.Height = 30;
            minusButton.MouseClick += (o, e) =>
            {
                verticalGauge.Value--;
                Console.WriteLine(verticalGauge.Value);
            };
            gui.Add(minusButton);


            // Button with custom skin
            YnTextButton customButton = new YnTextButton("Custom skin button");
            customButton.Position = new Vector2(toolbar.Position.X + padding, toolbar.Position.Y + tileSize + padding * 2);
            customButton.Skin = YnSkinGenerator.Generate(Color.Chocolate, "Fonts/MenuFont");
            gui.Add(customButton);

            gui.PrepareWidgets();

            customButton.Width = toolbar.Width - 2* padding;
            customButton.Height = 50;
            customButton.Pack = false;
            customButton.Layout();

            plusButton.Position = new Vector2(YnG.Width - plusButton.Width, 0);
            minusButton.Position = new Vector2(YnG.Width - minusButton.Width, YnG.Height - minusButton.Height);

            verticalGauge.Width = plusButton.Width;
            verticalGauge.Height = YnG.Height - plusButton.Height - minusButton.Height;
            verticalGauge.Position = new Vector2(YnG.Width - plusButton.Width, plusButton.Height);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            gui.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            gui.Update(gameTime);

            // Update the progress bar
            progress.Value++;

            if (progress.Value == progress.MaxValue)
            {
                progress.Value = 0;
            }

            // return to the menu if escape key is just pressed
            if (YnG.Keys.JustPressed(Keys.Escape))
            {
                // Stop the gui
                gui.Active = false;

                YnG.ScreenManager.SetScreenActive("menu", true);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            // Initialized is call when a screen is re/enabled
            gui.Active = true;
            _initialized = false;
   
            // TODO : We must add events like OnStart / OnStop / OnPause / etc...
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();
            gui.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            // Or if you wan't to draw it on another batch
            // gui.DrawGui(gameTime, spriteBatch);

            // Draw the HUD
            //UiManager.Draw(gameTime, spriteBatch);
        }
    }
}
