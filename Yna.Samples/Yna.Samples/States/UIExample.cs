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
        private Texture2D Background { get; set; }

        private YnProgressBar progress;
        private YnLabel progressLabel;

        public UIExample(string name)
            : base(name)
        {
            YnG.ShowMouse = true;
        }

        public override void BuildGui()
        {
            // Buttons in a toolbar
            int tileSize = 100;
            int padding = 10;
            int tileCount = 5;

            // TODO : Move that in initialize method

            YnPanel toolbar = YnG.Gui.Add(new YnPanel());
            toolbar.WithBackground = false;
            toolbar.Orientation = YnOrientation.Horizontal;
            toolbar.Padding = padding;
            toolbar.Height = tileSize;
            toolbar.Position = new Vector2(YnG.Width / 2 - (tileSize * tileCount + padding * (tileCount + 1)) / 2, YnG.Height / 2 - toolbar.Height / 2);

            toolbar.Add(new YnTextButton() { Text = "New", Width = tileSize, Height = tileSize, Pack = false });
            toolbar.Add(new YnTextButton() { Text = "Load", Width = tileSize, Height = tileSize, Pack = false });
            toolbar.Add(new YnTextButton() { Text = "Options", Width = tileSize, Height = tileSize, Pack = false });

            toolbar.Add(new YnTextButton() { Text = "Store", Width = tileSize, Height = tileSize, Pack = false });
            YnTextButton button = toolbar.Add(new YnTextButton() { Text = "Exit", Width = tileSize, Height = tileSize, Pack = false });
            button.MouseClick += delegate(object w, MouseClickSpriteEventArgs evt)
            {
                if (!evt.JustClicked) return;
                YnG.Gui.Clear();
                YnG.ScreenManager.SetScreenActive("menu");
            };

            progress = YnG.Gui.Add(new YnProgressBar());
            progress.Width = 400;
            progress.MaxValue = 400;
            progress.Height = 5;
            progress.Position = new Vector2(YnG.Width / 2 - progress.Width / 2, 100);

            progressLabel = YnG.Gui.Add(new YnLabel() { Text = "Fake loading..." });
            progressLabel.Position = new Vector2(YnG.Width / 2 - progress.Width / 2, 75);

            YnPanel container = YnG.Gui.Add(new YnPanel());
            container.Orientation = YnOrientation.Horizontal;
            container.WithBackground = false;
            container.Position = new Vector2(20, 20);
            YnLabel modeLabel = container.Add(new YnLabel());
            modeLabel.Text = "Hard Mode ";
            YnCheckbox check = container.Add(new YnCheckbox());

            // Slider
            YnSlider slider = YnG.Gui.Add(new YnSlider());
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

            YnG.Gui.PrepareWidgets();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

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
                YnG.Gui.Active = false;

                YnG.ScreenManager.SetScreenActive("menu");
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            // Initialized is call when a screen is re/enabled
            YnG.Gui.Active = true;
            _initialized = false;
   
            // TODO : We must add events like OnStart / OnStop / OnPause / etc...
        }

        public override void LoadContent()
        {

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();
            // Nothing!
            spriteBatch.End();


            // Draw the HUD
            //UiManager.Draw(gameTime, spriteBatch);
        }
    }
}
