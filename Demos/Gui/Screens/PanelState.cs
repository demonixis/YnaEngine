using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Gui.Widgets;
using Yna.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine.Graphics.Gui;

namespace Yna.Samples.Screens
{
    /// <summary>
    /// Panel sample
    /// </summary>
    public class PanelState : YnState2D
    {
        private YnGui Gui;

        public PanelState(string name)
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
            Gui.LoadContent();
        }

        private void BuildGUI()
        {
            // This example is very simple and show the use of panels. They're simple layout 
            // managers. The GUI does not provide any layout management. YnPanel is the exception.
            // With this widget, you can easily build menus.

            // The panel can be created like this. Note that the default orientation is horizontal
            YnPanel hPanel = new YnPanel();
            hPanel.X = 5;
            hPanel.Y = 5;
            Gui.Add(hPanel);

            // Let's add buttons to the panel :
            hPanel.Add(new YnTextButton("H Button 1"));
            hPanel.Add(new YnTextButton("H Button 2"));
            hPanel.Add(new YnTextButton("H Button 3"));
            hPanel.Add(new YnTextButton("H Button 4"));

            // This method will move panel's children to fit in the defined orientation. As the panel
            // will modify children coordinates, you should not initialize them : the Layout method
            // overrides them.
            hPanel.Layout();
            // And that's done! The panel will take care of it's children positions according to the
            // orientation.

            // You can also do the same for vertical menus. The only thing to change is the orientation
            // this can be set in the constructor or with the Orientation property.
            YnPanel vPanel = new YnPanel(YnOrientation.Vertical);
            vPanel.X = 25;
            vPanel.Y = 75;
            Gui.Add(vPanel);

            vPanel.Add(new YnTextButton("V Button 1"));
            vPanel.Add(new YnTextButton("V Button 2"));
            vPanel.Add(new YnTextButton("V Button 3"));
            vPanel.Add(new YnTextButton("V Button 4"));

            vPanel.Layout();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Gui.Update(gameTime);

            if (YnG.Gamepad.Back(PlayerIndex.One) || YnG.Keys.JustPressed(Keys.Escape))
                YnG.SetStateActive("menu", true);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Gui.Draw(gameTime, spriteBatch);
        }
    }
}
