using Yna.Framework;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class SpritesGame : YnGame
    {
        Menu menu;
        MenuEntry[] menuItems;

        public SpritesGame()
            : base(800, 600, "Yna : Sprite samples")
        {
            menuItems = new MenuEntry[]
            {
                new MenuEntry("basicSample", "Sprite generation", "In this sample we create some sprites and move/rotate it"),
                new MenuEntry("spritesheetSample", "Animated sprites", "In this sample we create animated sprites with spritesheets"),
            };
        }

        protected override void Initialize()
        {
            base.Initialize();

            menu = new Menu("menu", "Tilemap", menuItems);
            stateManager.Add(menu, true);
            stateManager.Add(new BasicSprites("basicSample"), false);
            stateManager.Add(new AnimatedSprites("spritesheetSample"), false);
        }

#if !WINDOWS_PHONE
        public static void Main(string[] args)
        {
            using (SpritesGame game = new SpritesGame())
            {
                game.Run();
            }
        }
#endif
    }
}
