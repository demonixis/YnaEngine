﻿using System;
using Yna.Framework;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class TerrainsGame : YnGame
    {
        public TerrainsGame()
                : base(SamplesConfiguration.Width, SamplesConfiguration.Height, "Yna : Heightmap")
        {

        }

        protected override void Initialize()
        {
            base.Initialize();

            stateManager.Add(new BasicTerrain("sample_1"), true);
            stateManager.Add(new HeighmapTerrain("sample_2"), false);
        }
#if !WINDOWS_PHONE
        public static void Main(string[] args)
        {
            using (TerrainsGame game = new TerrainsGame())
            {
                game.Run();
            }
        }
#endif
    }
}
