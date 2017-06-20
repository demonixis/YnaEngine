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
        private YnGui Gui;

        public GuiState(string name)
            : base(name, false, true)
        {
            YnG.ShowMouse = true;
            // YnSkinGenerator.Generate(Color.DodgerBlue, "Fonts/Font")
            Gui = new YnGui();
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

            YnWidgetProperties toolbarProps = new YnWidgetProperties();
            toolbarProps.Width = tileSize;
            toolbarProps.Height = tileSize;

            YnTextButton newButton = new YnTextButton(toolbarProps);
            newButton.Text = "New";
            newButton.TextAlign = YnTextAlign.Center;
            toolbar.Add(newButton);

            YnTextButton loadButton = new YnTextButton(toolbarProps);
            loadButton.Text = "Load";
            loadButton.TextAlign = YnTextAlign.Center;
            toolbar.Add(loadButton);

            YnTextButton settingsButton = new YnTextButton(toolbarProps);
            settingsButton.Text = "Settings";
            settingsButton.TextAlign = YnTextAlign.Center;
            toolbar.Add(settingsButton);

            YnTextButton storeButton = new YnTextButton(toolbarProps);
            storeButton.Text = "Store";
            storeButton.TextAlign = YnTextAlign.Center;
            toolbar.Add(storeButton);

            YnTextButton exitButton = new YnTextButton(toolbarProps);
            exitButton.Text = "Exit";
            exitButton.TextAlign = YnTextAlign.Center;
            exitButton.MouseClicked += (s, e) => YnG.Exit();
            toolbar.Add(exitButton);

            toolbar.Layout();

            // Button with custom skin
            YnTextButton customButton = new YnTextButton("Custom skin button");
            customButton.Width = toolbar.Width - 2 * padding;
            customButton.Height = 50;
            customButton.SkinName = "chocolateSkin";
            customButton.TextAlign = YnTextAlign.Center;
            customButton.Position = new Vector2(toolbar.Position.X, toolbar.Position.Y + tileSize + padding * 2);
            Gui.Add(customButton);

            
            progress = new YnProgressBar();
            progress.Width = 400;
            progress.MaxValue = 400;
            progress.Height = 5;
            progress.Position = new Vector2(YnG.Width / 2 - progress.Width / 2, 100);
            Gui.Add(progress);

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

            /*
            YnProgressBar verticalGauge = new YnProgressBar();
            verticalGauge.Orientation = YnOrientation.Vertical;
            Gui.Add(verticalGauge);

            YnWidgetProperties gaugeProps = new YnWidgetProperties();
            gaugeProps.Width = 30;
            gaugeProps.Height = 30;

            YnTextButton plusButton = new YnTextButton(gaugeProps);
            plusButton.Text = "+";
            plusButton.MouseClick += (o, e) =>
            {
                verticalGauge.Value++;

            };
            Gui.Add(plusButton);

            YnTextButton minusButton = new YnTextButton(gaugeProps);
            minusButton.Text = "-";
            minusButton.MouseClick += (o, e) =>
            {
                verticalGauge.Value--;
                Console.WriteLine(verticalGauge.Value);
            };
            Gui.Add(minusButton);
            */
            

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
            /*
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
            */
            /*
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
            */
        }

        public override void Initialize()
        {
            base.Initialize();
            Gui.Initialize();
            BuildGui();
        }


        public override void LoadContent()
        {
            base.LoadContent();
            Gui.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Gui.Update(gameTime);

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

                YnG.StateManager.SetActive("menu", true);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Gui.Draw(gameTime, spriteBatch);
        }
    }
}
