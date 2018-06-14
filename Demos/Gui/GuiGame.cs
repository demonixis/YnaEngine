using Yna.Engine;
using Yna.Engine.Graphics.Component;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class GuiGame : YnGame
    {
        private YnMenu _menu;
        private MenuEntry[] _menuItems;

        public GuiGame()
            : base ()
        {
            Window.Title = "Yna Framework : Graphical User Interface Samples";

            YnG.ShowMouse = true; 

            _menuItems = new MenuEntry[]
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

            _menu = new YnMenu("menu", "GUI Samples", _menuItems);
            _stateManager.Add(_menu, true);
            _stateManager.Add(new ButtonState("buttonSample"), false);
            _stateManager.Add(new PanelState("panelSample"), false);
            _stateManager.Add(new DepthState("depthSample"), false);
        }

        public static void Main(string[] args)
        {
            using (GuiGame game = new GuiGame())
                game.Run();
        }
    }
}