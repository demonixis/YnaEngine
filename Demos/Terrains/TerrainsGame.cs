using Yna.Engine;
using Yna.Engine.Graphics.Component;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class TerrainsGame : YnGame
    {
        private YnMenu _menu;
        private MenuEntry[] _menuItems;

        public TerrainsGame()
                : base()
        {
            Window.Title = "Yna : Heightmap";

            _menuItems = new MenuEntry[]
            {
                new MenuEntry("heightmapSample", "Heightmap terrain", "In this sample we create an heightmap terrain with two texture"),
                new MenuEntry("randomHeightmapSample", "Random Heightmap", "In this sample we create a random heightmap with a generated texture"),
            };
        }

        protected override void Initialize()
        {
            base.Initialize();

            _menu = new YnMenu("menu", "Terrains", _menuItems);

            _stateManager.Add(_menu, true);
            _stateManager.Add(new HeighmapTerrain("heightmapSample"), false);
            _stateManager.Add(new RandomHeightmap("randomHeightmapSample"), false);
        }

        public static void Main(string[] args)
        {
            using (TerrainsGame game = new TerrainsGame())
                game.Run();
        }
    }
}
