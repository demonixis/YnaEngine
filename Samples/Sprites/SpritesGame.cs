﻿using System;
using Yna.Engine;
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
#if LINUX
            SetScreenResolution(640, 480);
#endif
            menuItems = new MenuEntry[]
            {
                new MenuEntry("basicSample", "Sprite generation", "In this sample we create some sprites and move/rotate it"),
                new MenuEntry("spritesheetSample", "Animated sprites", "In this sample we create animated sprites with spritesheets"),
                new MenuEntry("virtualPadSample", "Virtual Pad", "This is the same sample as \"Animated Sprites\" but \nwe use a virtual pad for moving the player"),
            };
        }

        protected override void Initialize()
        {
            base.Initialize();

            menu = new Menu("menu", "Tilemap", menuItems);
            stateManager.Add(menu, true);
            stateManager.Add(new BasicSprites("basicSample"), false);
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
