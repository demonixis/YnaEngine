using System;
using Yna;

namespace Yna.Samples
{
    public class HeightmapGame : YnGame
    {
        public HeightmapGame()
                : base(SamplesConfiguration.Width, SamplesConfiguration.Height, "Yna : Heightmap")
        {

        }

        public static void Main(string[] args)
        {
            using (HeightmapGame game = new HeightmapGame())
            {
                game.Run();
            }
        }
    }
}
