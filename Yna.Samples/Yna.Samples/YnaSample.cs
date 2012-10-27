﻿using System;
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
        public YnaSample () 
		    : base(640, 480, "YNA Framework : Samples 2D") 
        {

        }
#endif

        protected override void Initialize()
        {
            base.Initialize();

            YnState menu = new Menu();
            menu.Name = "menu";
            YnState sample1 = new AnimatedSprites();
            YnState sample2 = new BasicSprites();
            YnState sample3 = new TiledMap2DSample();
            YnState sample4 = new IsoTiledMapSample();

            YnG.SwitchState(new Menu());
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
