using System;
using Yna.Framework;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class Models3DGame : YnGame
    {
        public Models3DGame()
            : base(800, 600, "Yna Samples : Models 3D Sample")
        {

        }

        protected override void Initialize()
        {
            base.Initialize();

            DungeonState dungeonState = new DungeonState("dungeon");
            PickingObjectsSample pickingState = new PickingObjectsSample("picking");

            stateManager.Add(dungeonState, true);
            stateManager.Add(pickingState, false);
        }

#if !WINDOWS_PHONE
        public static void Main(string[] args)
        {
            using (Models3DGame game = new Models3DGame())
            {
                game.Run();
            }
        }
#endif
    }
}
