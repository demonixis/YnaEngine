using System;
using Yna.Framework;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class ShapesGame : YnGame
    {
        public ShapesGame()
            : base(SamplesConfiguration.Width, SamplesConfiguration.Height, "Yna Samples : Custom Shapes")
        {

        }

        protected override void Initialize()
        {
            base.Initialize();

            stateManager.Add(new CubeSample("cubeShapeSample"), false);
            stateManager.Add(new IcosphereSample("icosphereSample"), true);
        }

        public static void Main(string[] args)
        {
            using (ShapesGame game = new ShapesGame())
            {
                game.Run();
            }
        }
    }
}
