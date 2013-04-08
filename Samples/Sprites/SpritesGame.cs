using System;
using Yna.Engine;
using Yna.Engine.Graphics.Component;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class SpritesGame : YnGame
    {
        YnMenu menu;
        MenuEntry[] menuItems;

        public SpritesGame()
            : base(800, 600, "Yna : Sprite samples")
        {
            menuItems = new MenuEntry[]
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
            menu = new YnMenu("menu", "Tilemap", menuItems);
            stateManager.Add(menu, true);
            stateManager.Add(new BasicSprites("basicSample"), false);
            stateManager.Add(new ParticlesSample("particleSample"), false);
            stateManager.Add(new AnimatedSprites("spritesheetSample"), false);
            stateManager.Add(new AnimatedSpriteVirtualPad("virtualPadSample"), false);
        }

#if !WINDOWS_PHONE
        [STAThread]
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
