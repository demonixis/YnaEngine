using Yna.Engine;
using Yna.Engine.Graphics.Component;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class TilemapGame : YnGame
    {
        private YnMenu _menu;
        private MenuEntry[] _menuItems;

        public TilemapGame ()
            : base()
        {
            Window.Title = "Tilemap";
           
            _menuItems = new MenuEntry[]
            {
                new MenuEntry("basicMap", "Basic tilemap 2D", "In this sample we create a simple tilemap 2D with a camera"),
                new MenuEntry("isoMap", "Isometric tilemap 2D", "In this sample we create a simple isometric tilemap with a camera"),
            };
        }

        protected override void Initialize()
        {
            base.Initialize();

            _menu = new YnMenu("menu", "Tilemap", _menuItems);
            _stateManager.Add(_menu, true);
            _stateManager.Add(new TilemapSample("basicMap"), false);
            _stateManager.Add(new IsometricMapSample("isoMap"), false);
        }

        public static void Main(string[] args)
        {
            using (TilemapGame game = new TilemapGame())
                game.Run();
        }
    }
}
