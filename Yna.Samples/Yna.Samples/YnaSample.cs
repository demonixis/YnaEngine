using System;
using Microsoft.Xna.Framework.Input;
using Yna;
using Yna.Display;
using Yna.Sample.States;
using Yna.Samples.Windows.States;

namespace Yna.Samples
{
    public class YnaSample : Yna.YnGame
    {
        public YnaSample () 
		    : base(1024, 600, "YNA Framework : Samples 2D") 
        {
 
        }

        protected override void Initialize()
        {
            base.Initialize();
            YnG.SwitchState(new GameMenu());
        }

        protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (YnG.Keys.JustPressed(Keys.F5))
                graphics.ToggleFullScreen();
        }

        public static void Main(string[] args)
        {
            using (YnaSample game = new YnaSample())
            {
                game.Run();
            }
        }
    }
}
