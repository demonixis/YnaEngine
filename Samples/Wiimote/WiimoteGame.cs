using System;
using Yna.Framework;

namespace Yna.Samples
{
    public class WiimoteGame : YnGame
    {
        public WiimoteGame()
            : base(SamplesConfiguration.Width, SamplesConfiguration.Height, "Yna Framework : Wiimote Sample")
        {

        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        public static void Main(string[] args)
        {
            using (WiimoteGame game = new WiimoteGame())
            {
                game.Run();
            }
        }
    }
}
