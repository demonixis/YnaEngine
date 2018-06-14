using Yna.Engine;
using Yna.Engine.Graphics.Component;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class SpritesGame : YnGame
    {
        private YnMenu _menu;
        private MenuEntry[] _menuItems;

        public SpritesGame()
            : base()
        {
            Window.Title = "Yna : Sprite samples";

            _menuItems = new MenuEntry[]
            {
                new MenuEntry("basicSample", "Sprite generation", "In this sample we create some sprites and move/rotate it"),
                new MenuEntry("particleSample", "Particle system", "Show you how to use the particle system"),
                new MenuEntry("spritesheetSample", "Animated sprites", "In this sample we create animated sprites with spritesheets"),
                new MenuEntry("virtualPadSample", "Virtual Pad", "This is the same sample as \"Animated Sprites\" but \nwe use a virtual pad for moving the player"),
            };
        }

        protected override void Initialize()
        {
            base.Initialize();

            YnG.ShowMouse = true;

            _menu = new YnMenu("menu", "Tilemap", _menuItems);
            _stateManager.Add(_menu, true);
            _stateManager.Add(new BasicSprites("basicSample"), false);
            _stateManager.Add(new ParticlesSample("particleSample"), false);
            _stateManager.Add(new AnimatedSprites("spritesheetSample"), false);
            _stateManager.Add(new AnimatedSpriteVirtualPad("virtualPadSample"), false);
        }

        public static void Main(string[] args)
        {
            using (SpritesGame game = new SpritesGame())
                game.Run();
        }
    }
}