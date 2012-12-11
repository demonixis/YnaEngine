using System;
using System.Collections.Generic;
using Yna;

namespace Yna.Samples
{
    public class TilemapGame : YnGame
    {
        public TilemapGame ()
            : base(SamplesConfiguration.Width, SamplesConfiguration.Height, "Yna : Tilemap")
        {

        }

        public static void Main(string[] args)
        {
            using (TilemapGame game = new TilemapGame())
            {
                game.Run();
            }
        }
    }
}
