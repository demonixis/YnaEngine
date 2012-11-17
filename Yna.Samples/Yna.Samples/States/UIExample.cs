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
            gui = new YnGui();
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

            YnTextButton button = toolbar.Add(new YnTextButton() { Text = "Exit", Width = tileSize, Height = tileSize, Pack = false });
            button.MouseClick += (s, e) =>
            {
                if (!e.JustClicked) 
                    return;

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

            /*
            // Simple Label example
            YnLabel simpleLabel = Gui.Add(new YnLabel());
            simpleLabel.Text = "Simple Label";
            simpleLabel.Position = new Vector2(10, 10);
            

            // Simple vertical panel
            YnLabel simpleTitle = Gui.Add(new YnLabel());
            simpleTitle.Text = "Automatic resize of panels :";
            simpleTitle.Position = new Vector2(10, 30);

            YnPanel panel = Gui.Add(new YnPanel());
            panel.Orientation = YnOrientation.Vertical;
            panel.Bounds = new Rectangle(10, 50, 50, 50);
            panel.Padding = 5;

            panel.Add(new YnLabel() { Text = "Short" });
            panel.Add(new YnLabel() { Text = "Some basic text" });
            panel.Add(new YnLabel() { Text = "Some extreme and dangerously long label" });
            

            // Complex panel
            YnLabel complexTitle = Gui.Add(new YnLabel());
            complexTitle.Text = "Complex panel management :";
            complexTitle.Position = new Vector2(350, 80);

            YnPanel complexPanel = Gui.Add(new YnPanel());
            complexPanel.Orientation = YnOrientation.Horizontal;
            complexPanel.Position = new Vector2(350, 100);
            complexPanel.Padding = 3;

            YnPanel leftPanel = complexPanel.Add(new YnPanel());
            leftPanel.Padding = 3;
            leftPanel.Orientation = YnOrientation.Vertical;
            leftPanel.Add(new YnLabel() { Text = "Left label A with long text" });
            leftPanel.Add(new YnLabel() { Text = "Left label B" });

            YnPanel RightPanel = complexPanel.Add(new YnPanel());
            RightPanel.Padding = 3;
            RightPanel.Orientation = YnOrientation.Vertical;
            RightPanel.Add(new YnLabel() { Text = "Right label A" });
            RightPanel.Add(new YnLabel() { Text = "Right label B withe loooooong text" });
        
            // Panel without padding
            YnPanel noPaddingPanel = Gui.Add(new YnPanel());
            noPaddingPanel.Position = new Vector2(10, 180);
            noPaddingPanel.Add(new YnLabel() { Text = "Panel without padding" });

            // Panel with padding
            YnPanel paddedPanel = Gui.Add(new YnPanel());
            paddedPanel.Position = new Vector2(10, 210);
            paddedPanel.Padding = 20;
            paddedPanel.Add(new YnLabel() { Text = "Panel without big padding" });
            
            // Buttons in a toolbar
            YnPanel toolbar = Gui.Add(new YnPanel());
            toolbar.Orientation = YnOrientation.Horizontal;
            toolbar.Position = new Vector2(300, 300);
            toolbar.Padding = 5;

            toolbar.Add(new YnTextButton() { Text = "Load" });
            toolbar.Add(new YnTextButton() { Text = "Save" });
            toolbar.Add(new YnTextButton() { Text = "Options" });
            toolbar.Add(new YnTextButton() { Text = "Exit" });
             */

            gui.PrepareWidgets();
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
