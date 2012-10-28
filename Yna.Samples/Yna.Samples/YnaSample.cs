using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna;
using Yna.Display;
using Yna.State;
using Yna.Samples.States;

namespace Yna.Samples
{
    public class YnaSample : Yna.YnGame
    {
#if NETFX_CORE
        public YnaSample()
        {

        }
#else
        public YnaSample()
            : base(1280, 800, "YNA Framework : Samples 2D")
        {

        }
#endif

        protected override void Initialize()
        {
            base.Initialize();

            YnState menu = new Menu("menu");

            YnState[] screens = new YnState[]
            {
                new AnimatedSprites("animated_sample"),
                new BasicSprites("basic_sample"),
                new TiledMap2DSample("tilemap_sample"),
                new IsoTiledMapSample("iso_sample"),
                new UIExample("ui_sample")
            };

            // We don't show the gui 
            // TODO : Make it more user friendly :)
            YnG.Gui.Enabled = false;
            YnG.Gui.Visible = false;

            screenManager.Add(menu, true);
            screenManager.Add(screens, false);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (YnG.Keys.JustPressed(Keys.F5))
                graphics.ToggleFullScreen();
        }

        public static void Main(string[] args)
        {
#if NETFX_CORE
            var factory = new MonoGame.Framework.GameFrameworkViewSource<YnaSample>();
            Windows.ApplicationModel.Core.CoreApplication.Run(factory);
#else
            using (YnaSample game = new YnaSample())
            {
                game.Run();
            }
#endif
        }
    }

}
