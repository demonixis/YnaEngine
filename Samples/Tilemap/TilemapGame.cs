using System;
using System.Collections.Generic;
using Yna;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class TilemapGame : YnGame
    {
        public TilemapGame ()
            : base(SamplesConfiguration.Width, SamplesConfiguration.Height, "Yna : Tilemap")
        {

        }

        protected override void Initialize()
        {
            base.Initialize();

            screenManager.Add(new TilemapSample("sample_1"), true);
            screenManager.Add(new IsometricMapSample("sample_2"), false);
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
