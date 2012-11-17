using System;
using System.Collections.Generic;
using System.Linq;
using Yna;
using Yna.State;
using Yna.Samples3D.States;

namespace Yna.Samples3D
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class YnaSample3D : YnGame
    {
#if NETFX_CORE || WINDOWS_PHONE
        public YnaSample3D()
            : base()
        {

        }
#else
        public YnaSample3D()
            : base(1024, 600, "YNA Framework : 3D samples", true)
        {

        }
#endif

        protected override void Initialize()
        {
            base.Initialize();

            YnState menu = new GameMenu("menu");

            Screen[] states = new Screen[]
            {
                new SpaceGame("spacegame_sample"),
                new NasaSample("nasa_sample"),
                new SimpleTerrainSample("simpleterrain_sample"),
                new HeightmapSample("heightmap_sample"),
                new ThirdPersonSample("thirdperson_sample"),
                new ThirdPersonHeightmapSample("tp_heightmap_sample"),
                new DungeonSample("dungeon_sample")
            };

            screenManager.Add(menu, true);
            screenManager.Add(states, false);
        }

        public static void Main(string[] args)
        {
#if NETFX_CORE
            var factory = new MonoGame.Framework.GameFrameworkViewSource<YnaSample3D>();
            Windows.ApplicationModel.Core.CoreApplication.Run(factory);
#else
            using (YnaSample3D game = new YnaSample3D())
            {
                game.Run();
            }
#endif
        }

    }
}
