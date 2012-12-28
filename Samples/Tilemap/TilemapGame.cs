using System;
using System.Collections.Generic;
using Yna.Framework;
using Yna.Samples;
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

            stateManager.Add(new TilemapSample("sample_1"), false);
            stateManager.Add(new IsometricMapSample("sample_2"), true);
        }

#if !WINDOWS_PHONE
        public static void Main(string[] args)
        {
            using (TilemapGame game = new TilemapGame())
            {
                game.Run();
            }
        }
#endif
    }
}
