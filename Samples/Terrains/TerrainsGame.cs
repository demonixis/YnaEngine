using System;
using Yna.Engine;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class TerrainsGame : YnGame
    {
        Menu menu;
        MenuEntry[] menuItems;

        public TerrainsGame()
                : base(640, 408, "Yna : Heightmap")
        {
            menuItems = new MenuEntry[]
            {
                new MenuEntry("basicSample", "Simple terrain", "In this sample we create a simple terrain and add a texture on it"),
                new MenuEntry("heightmapSample", "Heightmap terrain", "In this sample we create an heightmap terrain with two texture"),
                new MenuEntry("randomHeightmapSample", "Random Heightmap", "In this sample we create a random heightmap with a generated texture"),
            };
        }

        protected override void Initialize()
        {
            base.Initialize();

            menu = new Menu("menu", "Terrains", menuItems);
            stateManager.Add(menu, true);
            stateManager.Add(new BasicTerrain("basicSample"), false);
            stateManager.Add(new HeighmapTerrain("heightmapSample"), false);
            stateManager.Add(new RandomHeightmap("randomHeightmapSample"), false);
        }
#if !WINDOWS_PHONE && !MACOSX
        public static void Main(string[] args)
        {
            using (TerrainsGame game = new TerrainsGame())
            {
                game.Run();
            }
        }
#elif MACOSX
		static void Main (string[] args)
		{
			MonoMac.AppKit.NSApplication.Init ();
			
			using (var p = new MonoMac.Foundation.NSAutoreleasePool ()) 
			{
				MonoMac.AppKit.NSApplication.SharedApplication.Delegate = new AppDelegate ();
				MonoMac.AppKit.NSApplication.Main (args);
			}
		}
		
		class AppDelegate : MonoMac.AppKit.NSApplicationDelegate
		{
			TerrainsGame game;
			public override void FinishedLaunching (MonoMac.Foundation.NSObject notification)
			{
				game = new TerrainsGame();
				game.Run();
			}
			
			public override bool ApplicationShouldTerminateAfterLastWindowClosed (MonoMac.AppKit.NSApplication sender)
			{
				return true;
			}
		}
#endif
    }
}
