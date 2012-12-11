using System;
using Yna;

namespace Yna.Samples
{
    public class KinectGame : YnGame
    {
        public KinectGame ()
            : base(SamplesConfiguration.Width, SamplesConfiguration.Height, "Yna : Kinect")
        {

        }

        public static void Main(string[] args)
        {
            using (KinectGame game = new KinectGame())
            {
                game.Run();
            }
        }
    }
}
