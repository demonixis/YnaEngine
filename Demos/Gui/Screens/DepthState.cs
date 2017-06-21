using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yna.Engine.Graphics;
using Yna.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine.Graphics.Gui.Widgets;
using Yna.Engine.Graphics.Gui;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Samples.Screens
{
    /// <summary>
    /// Depth management sample
    /// </summary>
    public class DepthState : YnState2D
    {
        private YnGui Gui;

        public DepthState(string name)
            : base(name, false, true)
        {
            Gui = new YnGui();
        }

        public override void Initialize()
        {
            base.Initialize();
            Gui.Initialize();
            BuildGUI();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Gui.LoadContent();
        }

        private void BuildGUI()
        {
            // Create some skins to see what we're doing
            YnSkin skinA = YnSkinGenerator.Generate(Color.Green, "Fonts/DefaultFont");
            YnGui.RegisterSkin("green", skinA);

            YnSkin skinB = YnSkinGenerator.Generate(Color.DeepPink, "Fonts/DefaultFont");
            YnGui.RegisterSkin("pink", skinB);

            YnSkin skinC = YnSkinGenerator.Generate(Color.Blue, "Fonts/DefaultFont");
            YnGui.RegisterSkin("chocolate", skinC);

            // Group A
            YnPanel panelA = new YnPanel(YnOrientation.Vertical);
            panelA.X = 50;
            panelA.Y = 50;
            panelA.Width = 200;
            panelA.Height = 300;
            panelA.SkinName = "green";
            panelA.HasBackground = true;
            Gui.Add(panelA);

            // Group B
            YnPanel panelB = new YnPanel(YnOrientation.Vertical);
            panelB.X = 150;
            panelB.Y = 75;
            panelB.Width = 200;
            panelB.Height = 200;
            panelB.SkinName = "pink";
            panelB.HasBackground = true;
            Gui.Add(panelB);

            // Group C
            YnPanel panelC = new YnPanel(YnOrientation.Vertical);
            panelC.X = 225;
            panelC.Y = 100;
            panelC.Width = 200;
            panelC.Height = 300;
            panelC.SkinName = "chocolate";
            panelC.HasBackground = true;
            Gui.Add(panelC);

            // Depth management buttons
            // Note that these panels are used out of the GUI system to have no deth
            // and don't disturb the depth example.
            YnPanel greenPanel = CreateDepthPanel("Green", panelA);
            greenPanel.Y = 425;
            Add(greenPanel);

            YnPanel pinkPanel = CreateDepthPanel("Pink", panelB);
            pinkPanel.X = 175;
            pinkPanel.Y = 425;
            Add(pinkPanel);

            YnPanel bluePanel = CreateDepthPanel("Blue", panelC);
            bluePanel.X = 350;
            bluePanel.Y = 425;
            Add(bluePanel);
        }

        private YnPanel CreateDepthPanel(string text, YnPanel panel)
        {
            YnPanel depthPanel = new YnPanel(YnOrientation.Vertical);
            Gui.Add(depthPanel);

            YnTextButton bottomButton = new YnTextButton(text + " to bottom");
            bottomButton.MouseClicked += (s, e) => Gui.DepthToBottom(panel);
            depthPanel.Add(bottomButton);

            YnTextButton upButton = new YnTextButton(text + " Up");
            upButton.MouseClicked += (s, e) => Gui.DepthUp(panel);
            depthPanel.Add(upButton);

            YnTextButton downButton = new YnTextButton(text + " Down");
            downButton.MouseClicked += (s, e) => Gui.DepthDown(panel);
            depthPanel.Add(downButton);

            YnTextButton topButton = new YnTextButton(text + " to top");
            topButton.MouseClicked += (s, e) => Gui.DepthToTop(panel);
            depthPanel.Add(topButton);

            depthPanel.Layout();

            return depthPanel;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Gui.Update(gameTime);

            if (YnG.Gamepad.Back(PlayerIndex.One) || YnG.Keys.JustPressed(Keys.Escape))
                YnG.SetStateActive("menu", true);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            spriteBatch.Begin();
            Gui.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}
