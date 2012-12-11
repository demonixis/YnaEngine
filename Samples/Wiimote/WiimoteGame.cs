using System;
using Yna;

namespace Yna.Samples
{
    public class WiimoteGame : YnGame
    {
        public WiimoteGame ()
            : base(SamplesConfiguration.Width, SamplesConfiguration.Height, "Yna : Wiimote")
        {

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
