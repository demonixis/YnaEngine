using System;
using Yna;
using Yna.Display;
using Yna.Sample.States;

namespace Yna.Samples
{
    /// <summary>
    /// Type principal pour votre jeu
    /// </summary>
    public class YnaSample : Yna.YnGame
    {
        public YnaSample () 
		    : base(1024, 600, "YNA Framework : Samples 2D") { }

        protected override void Initialize()
        {
            base.Initialize();
            
            SwitchState(new GameMenu());
        }
    }
}
