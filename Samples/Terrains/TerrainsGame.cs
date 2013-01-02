using System;
using Yna.Framework;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class TerrainsGame : YnGame
    {
        Menu menu;
        MenuEntry[] menuItems;

        public TerrainsGame()
                : base(800, 600, "Yna : Heightmap")
        {
            menuItems = new MenuEntry[]
            {
                new MenuEntry("basicSample", "Simple terrain", "In this sample we create a simple terrain and add a texture on it"),
                new MenuEntry("heightmapSample", "Heightmap terrain", "In this sample we create an heightmap terrain with two texture"),
            };
        }

        protected override void Initialize()
        {
            base.Initialize();

            menu = new Menu("menu", "Terrains", menuItems);
            stateManager.Add(menu, true);
            stateManager.Add(new BasicTerrain("basicSample"), false);
            stateManager.Add(new HeighmapTerrain("heightmapSample"), false);
        }
#if !WINDOWS_PHONE
        public static void Main(string[] args)
        {
            using (TerrainsGame game = new TerrainsGame())
            {
                game.Run();
            }
        }
#endif
    }
}
