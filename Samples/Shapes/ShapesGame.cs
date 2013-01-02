using System;
using System.Collections.Generic;
using Yna.Framework;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class ShapesGame : YnGame
    {
        Menu menu;
        MenuEntry[] menuItems;
     
        public ShapesGame()
            : base(800, 600, "Yna Samples : Custom Shapes")
        {
            menuItems = new MenuEntry[]
            {
                new MenuEntry("cubeShapeSample", "Cube", "In this sample we create some cubes with differents materials"),
                new MenuEntry("icosphereSample", "Icoshpere", "In this sample we create a textured icosphere with a custom material")
            };   
        }

        protected override void Initialize()
        {
            base.Initialize();

            menu = new Menu("menu", "Custom Shapes", menuItems);

            stateManager.Add(menu, true);
            stateManager.Add(new CubeSample("cubeShapeSample"), false);
            stateManager.Add(new IcosphereSample("icosphereSample"), false);
        }

#if !WINDOWS_PHONE
        public static void Main(string[] args)
        {
            using (ShapesGame game = new ShapesGame())
            {
                game.Run();
            }
        }
#endif
    }
}
