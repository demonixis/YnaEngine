using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Gui;
using Yna.Engine.Graphics.Event;
using Yna.Engine.Graphics.Gui.Widgets;

namespace Yna.Samples
{
    /// <summary>
    /// Description of UIExample.
    /// </summary>
    public class GuiState : YnState2D
    {
        private Texture2D background;
        private YnProgressBar progress;
        private YnLabel progressLabel;

        public GuiState(string name)
            : base(name)
        {
            YnG.ShowMouse = true;
            // YnSkinGenerator.Generate(Color.DodgerBlue, "Fonts/Font")
            
            YnGui.RegisterSkin("chocolateSkin", YnSkinGenerator.Generate(Color.Chocolate, "Fonts/Font"));
        }

        public void BuildGui()
        {
            // Buttons in a toolbar
            int tileSize = 100;
            int padding = 10;
            int tileCount = 5;


            YnLabel label = new YnLabel();
            label.Text = "Test label";
            label.SkinName = "testSkin";
            Gui.Add(label);
            
            YnPanel toolbar = new YnPanel();
            Gui.Add(toolbar); // Add toolbar to Gui

            toolbar.HasBackground = false;
            toolbar.Padding = padding;
            toolbar.Height = tileSize;
            toolbar.Position = new Vector2(YnG.Width / 2 - (tileSize * tileCount + padding * (tileCount + 1)) / 2, YnG.Height / 2 - toolbar.Height / 2);

            toolbar.Add(new YnTextButton(tileSize, tileSize, "New"));
            toolbar.Add(new YnTextButton(tileSize, tileSize, "Load"));
            toolbar.Add(new YnTextButton(tileSize, tileSize, "Options"));
            toolbar.Add(new YnTextButton(tileSize, tileSize, "Store"));
            YnTextButton button = toolbar.Add(new YnTextButton(tileSize, tileSize, "Exit"));
            button.MouseClicked += (s, e) => YnG.Exit();

            toolbar.Layout();

            
            progress = new YnProgressBar();
            Gui.Add(progress);

            progress.Width = 400;
            progress.MaxValue = 400;
            progress.Height = 5;
            progress.Position = new Vector2(YnG.Width / 2 - progress.Width / 2, 100);

            progressLabel = new YnLabel();
            progressLabel.Text = "Fake loading...";
            progressLabel.Position = new Vector2(YnG.Width / 2 - progress.Width / 2, 75);
            Gui.Add(progressLabel);



            YnPanel container = new YnPanel();
            container.Position = new Vector2(20, 20);
            container.Orientation = YnOrientation.Horizontal;
            Gui.Add(container);

            YnLabel modeLabel = new YnLabel();
            modeLabel.Text = "Hard Mode ";
            container.Add(modeLabel);
            container.Add(new YnCheckbox());

            container.Layout();

            // Slider
            YnSlider slider = new YnSlider();
            slider.Width = 200;
            slider.Height = 20;
            slider.Position = new Vector2(20, 200);
            slider.MaxValue = 10;
            Gui.Add(slider);

            
            YnProgressBar verticalGauge = new YnProgressBar();
            verticalGauge.Orientation = YnOrientation.Vertical;
            Gui.Add(verticalGauge);

            YnTextButton plusButton = new YnTextButton(30, 30, "+");
            plusButton.MouseClick += (o, e) =>
            {
                verticalGauge.Value++;

            };
            Gui.Add(plusButton);

            YnTextButton minusButton = new YnTextButton(30, 30, "-");
            minusButton.MouseClick += (o, e) =>
            {
                verticalGauge.Value--;
                Console.WriteLine(verticalGauge.Value);
            };
            Gui.Add(minusButton);

            // Button with custom skin
            YnTextButton customButton = new YnTextButton(toolbar.Width - 2 * padding, 50, "Custom skin button");
            customButton.SkinName = "chocolateSkin";
            customButton.Position = new Vector2(toolbar.Position.X, toolbar.Position.Y + tileSize + padding * 2);
            Gui.Add(customButton);

            /*
            // Textbox
            string wrappedText = "Example of default character wrapping textbox";
            YnTextBox textBox = new YnTextBox(wrappedText, 150, true);
            textBox.Padding = 15;
            textBox.Position = new Vector2(0, 550);
            gui.Add(textBox);

            string text = "Example of word wrapping textbox";
            YnTextBox textBoxW = new YnTextBox(text, 150, true);
            textBoxW.Padding = 15;
            textBoxW.Position = new Vector2(200, 550);
            textBoxW.WordWrap = true;
            gui.Add(textBoxW);

            string elipsisText = "Elispsis textbox with long text";
            YnTextBox elipsisTextbox = new YnTextBox(elipsisText, 150, true);
            elipsisTextbox.Padding = 15;
            elipsisTextbox.Position = new Vector2(400, 550);
            elipsisTextbox.Wrap = false;
            gui.Add(elipsisTextbox);
            */

            plusButton.Position = new Vector2(YnG.Width - plusButton.Width, 0);
            minusButton.Position = new Vector2(YnG.Width - minusButton.Width, YnG.Height - minusButton.Height);

            verticalGauge.Width = plusButton.Width;
            verticalGauge.Height = YnG.Height - plusButton.Height - minusButton.Height;
            verticalGauge.Position = new Vector2(YnG.Width - plusButton.Width, plusButton.Height);
            
            // Widget with custom borders

            // Generate a basic base skin
            YnBorder borderDef = new YnBorder();
            borderDef.TopLeft = YnGraphics.CreateTexture(Color.PowderBlue, 15, 15);
            borderDef.Top = YnGraphics.CreateTexture(Color.Orange, 5, 5);
            borderDef.TopRight = YnGraphics.CreateTexture(Color.PowderBlue, 15, 15);
            borderDef.Right = YnGraphics.CreateTexture(Color.Orange, 5, 5);
            borderDef.BottomRight = YnGraphics.CreateTexture(Color.PowderBlue, 15, 15);
            borderDef.Bottom = YnGraphics.CreateTexture(Color.Orange, 5, 5);
            borderDef.BottomLeft = YnGraphics.CreateTexture(Color.PowderBlue, 15, 15);
            borderDef.Left = YnGraphics.CreateTexture(Color.Orange, 5, 5);

            YnSkin customSkin = YnSkinGenerator.Generate(Color.DodgerBlue, "Fonts/Font");
            customSkin.BorderDefault = borderDef;

            // Register the custom skin
            YnGui.RegisterSkin("customBorderSkin", customSkin);

            YnTextButton customBorderButton = new YnTextButton(150, 30, "Funky borders");
            customBorderButton.X = 50;
            customBorderButton.Y = 500;
            customBorderButton.SkinName = "customBorderSkin";
            customBorderButton.HasBorders = true;

            Gui.Add(customBorderButton);


            YnPanel depthPanel = new YnPanel();
            depthPanel.HasBackground = true;
            depthPanel.Width = YnG.Width;
            depthPanel.Height = 300;
            depthPanel.X = 0;
            depthPanel.Y = 150;
            Gui.Add(depthPanel);

            YnPanel depthButtonPanel = new YnPanel();
            depthButtonPanel.X = 350;
            depthButtonPanel.Y = 570;
            Gui.Add(depthButtonPanel);

            YnTextButton upButton = new YnTextButton(50, 20, "Up");
            upButton.MouseClicked += (s, e) => Gui.DepthUp(depthPanel);
            depthButtonPanel.Add(upButton);

            YnTextButton downButton = new YnTextButton(50, 20, "Down");
            downButton.MouseClicked += (s, e) => Gui.DepthDown(depthPanel);
            depthButtonPanel.Add(downButton);

            depthButtonPanel.Layout();

        }

        public override void Initialize()
        {
            base.Initialize();
            BuildGui();
        }


        public override void LoadContent()
        {
            base.LoadContent();
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

                YnG.StateManager.SetStateActive("menu", true);
            }
            
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
