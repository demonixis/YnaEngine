using Yna.Engine;
using Yna.Engine.Graphics.Component;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class ShapesGame : YnGame
    {
        private YnMenu _menu;
        private MenuEntry[] _menuItems;
     
        public ShapesGame()
            : base()
        {
            Window.Title = "Yna Samples : Custom Shapes";

            _menuItems = new MenuEntry[]
            {
                new MenuEntry("cubeShapeSample", "Cube", "In this sample we create some cubes with differents materials"),
                new MenuEntry("cylinderSample", "Cylinder", "In this sample we create a textured cylinder with a custom material"),
                new MenuEntry("icosphereSample", "Icoshpere", "In this sample we create a textured icosphere with a custom material"),
                new MenuEntry("planeSample", "Plane", "In this sample we create some textured planes with a custom material"),
                new MenuEntry("pyramidSample", "Pyramid", "In this sample we create a textured pyramid with a custom material"),
                new MenuEntry("sphereSample", "Sphere", "In this sample we create a textured sphere with a custom material"),
                new MenuEntry("torusSample", "Torus", "In this sample we create a textured torus with a custom material"),
                new MenuEntry("materialSample", "Material", "Custom materials"),
            };   
        }

        protected override void Initialize()
        {
            base.Initialize();

            _menu = new YnMenu("menu", "Custom Shapes", _menuItems);

            _stateManager.Add(_menu, true);
            _stateManager.Add(new CubeSample("cubeShapeSample"), false);
            _stateManager.Add(new CylinderSample("cylinderSample"), false);
            _stateManager.Add(new IcosphereSample("icosphereSample"), false);
            _stateManager.Add(new PlaneSample("planeSample"), false);
            _stateManager.Add(new PyramidSample("pyramidSample"), false);
            _stateManager.Add(new SphereSample("sphereSample"), false);
            _stateManager.Add(new TorusSample("torusSample"), false);
            _stateManager.Add(new MaterialSample("materialSample"), false);
        }
        public static void Main(string[] args)
        {
            using (var game = new ShapesGame())
                game.Run();
        }
    }
}
