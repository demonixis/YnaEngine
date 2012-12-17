using Yna.Framework;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class SpritesGame : YnGame
    {
        public SpritesGame()
            : base()
        {

        }

        protected override void Initialize()
        {
            base.Initialize();

            stateManager.Add(new BasicSprites("sample_1"), false);
            stateManager.Add(new AnimatedSprites("sample_2"), true);
        }
    }
}
