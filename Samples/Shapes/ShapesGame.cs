using System;
using System.Collections.Generic;
using Yna.Engine;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class ShapesGame : YnGame
    {
        Menu menu;
        MenuEntry[] menuItems;
     
        public ShapesGame()
            : base(1440, 900, "Yna Samples : Custom Shapes")
        {
            menuItems = new MenuEntry[]
            {
                new MenuEntry("cubeShapeSample", "Cube", "In this sample we create some cubes with differents materials"),
                new MenuEntry("cylinderSample", "Cylinder", "In this sample we create a textured cylinder with a custom material"),
                new MenuEntry("icosphereSample", "Icoshpere", "In this sample we create a textured icosphere with a custom material"),
                new MenuEntry("planeSample", "Plane", "In this sample we create some textured planes with a custom material"),
                new MenuEntry("pyramidSample", "Pyramid", "In this sample we create a textured pyramid with a custom material"),
                new MenuEntry("sphereSample", "Sphere", "In this sample we create a textured sphere with a custom material"),
                new MenuEntry("torusSample", "Torus", "In this sample we create a textured torus with a custom material"),
            };   
        }

        protected override void Initialize()
        {
            base.Initialize();

            menu = new Menu("menu", "Custom Shapes", menuItems);

            stateManager.Add(menu, true);
            stateManager.Add(new CubeSample("cubeShapeSample"), false);
            stateManager.Add(new CylinderSample("cylinderSample"), false);
            stateManager.Add(new IcosphereSample("icosphereSample"), false);
            stateManager.Add(new PlaneSample("planeSample"), false);
            stateManager.Add(new PyramidSample("pyramidSample"), false);
            stateManager.Add(new SphereSample("sphereSample"), false);
            stateManager.Add(new TorusSample("torusSample"), false);
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
