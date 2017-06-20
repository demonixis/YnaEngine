using System;
using System.Collections.Generic;
using Yna.Engine;
using Yna.Engine.Graphics.Component;
using Yna.Samples;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class TilemapGame : YnGame
    {
        YnMenu menu;
        MenuEntry[] menuItems;

        public TilemapGame ()
            : base(800, 600, "Yna : Tilemap")
        {
            menuItems = new MenuEntry[]
            {
                new MenuEntry("basicMap", "Basic tilemap 2D", "In this sample we create a simple tilemap 2D with a camera"),
                new MenuEntry("isoMap", "Isometric tilemap 2D", "In this sample we create a simple isometric tilemap with a camera"),
            };
        }

        protected override void Initialize()
        {
            base.Initialize();

            menu = new YnMenu("menu", "Tilemap", menuItems);
            stateManager.Add(menu, true);
            stateManager.Add(new TilemapSample("basicMap"), false);
            stateManager.Add(new IsometricMapSample("isoMap"), false);
        }

#if !WINDOWS_PHONE
        public static void Main(string[] args)
        {
            using (TilemapGame game = new TilemapGame())
            {
                game.Run();
            }
        }
#endif
    }
}
