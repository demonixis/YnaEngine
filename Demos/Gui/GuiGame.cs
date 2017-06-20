using System;
using Yna.Engine;
using Yna.Engine.Graphics.Component;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class GuiGame : YnGame
    {
        YnMenu menu;
        MenuEntry[] menuItems;

        public GuiGame()
            : base (800, 600, "Yna Framework : Graphical User Interface Samples")
        {
            YnG.ShowMouse = true; 

            menuItems = new MenuEntry[]
            {
                new MenuEntry("buttonSample", "Buttons & Skins", "See how work buttons and skins"),
                new MenuEntry("panelSample", "Panels", "Panels automatic layout"),
                new MenuEntry("depthSample", "Depth management", "The GUI integrates a depth management to order widgets on render"),
                //new MenuEntry("inputSample", "Input", "Input widgets"),
            };
        }

        protected override void Initialize()
        {
            base.Initialize();

            menu = new YnMenu("menu", "GUI Samples", menuItems);

            stateManager.Add(menu, true);
            stateManager.Add(new ButtonState("buttonSample"), false);
            stateManager.Add(new PanelState("panelSample"), false);
            stateManager.Add(new DepthState("depthSample"), false);
        }

#if !WINDOWS_PHONE
        public static void Main(string[] args)
        {
            using (GuiGame game = new GuiGame())
            {
                game.Run();
            }
        }
#endif
    }
}