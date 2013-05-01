using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine;
using Yna.Engine.Graphics.Component;
using Yna.Samples;
using Yna.Samples.Screens;

namespace YnPhoneGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : YnGame
    {
        public Game1()
            : base()
        {
            Microsoft.Xna.Framework.Input.Touch.TouchPanel.EnableMouseGestures = true;
            Microsoft.Xna.Framework.Input.Touch.TouchPanel.EnableMouseTouchPoint = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            MenuEntry[] menuItems = new MenuEntry[]
            {
                new MenuEntry("sprites", "Animated Sprites", "In this sample you'll learn how to\nplay with Sprites"),
                new MenuEntry("geometry", "Custom Geometries", "In this 3D sample you'll learn how to\ncreate custom 3D geometries"),
                new MenuEntry("heightmap", "Heightmap", "In this sample you'll learn how to\ncreate an heightmap")
            };

            YnMenu menu = new YnMenu("menu", "Yna Engine for Windows Phone 8", menuItems);

            stateManager.Add(menu, true);
            stateManager.Add(new AnimatedSpriteVirtualPad("sprites"), false);
            stateManager.Add(new CubeSample("geometry"), false);
            stateManager.Add(new HeighmapTerrain("heightmap"), false);

            base.Initialize();
        }
    }
}
