using System;
using Yna;
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

            screenManager.Add(new BasicTerrain("sample_1"), false);
            screenManager.Add(new HeighmapTerrain("sample_2"), true);
        }

        public static void Main(string[] args)
        {
            using (TerrainsGame game = new TerrainsGame())
            {
                game.Run();
            }
        }
    }
}
