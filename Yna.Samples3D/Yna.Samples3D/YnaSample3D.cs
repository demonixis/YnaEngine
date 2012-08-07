using System;
using System.Collections.Generic;
using System.Linq;
using Yna;
using Yna.Samples3D.States;

namespace Yna.Samples3D
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class YnaSample3D : YnGame
    {
        public YnaSample3D()
            : base()
        {
            this.SetScreenResolution(1280, 800);
            this.Window.Title = "YNA 3D Samples";  
        }

        protected override void Initialize()
        {
            base.Initialize();
            SwitchState(new GameMenu());
        }

#if WINDOWS || XBOX
        public static void Main(string[] args)
        {
            using (YnaSample3D game = new YnaSample3D())
            {
                game.Run();
            }
        }
#endif
    }
}
