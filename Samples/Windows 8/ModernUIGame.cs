using System;
using Microsoft.Xna.Framework.Input.Touch;
using Yna.Engine;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class ModernUIGame : YnGame
    {
        public ModernUIGame()
            : base()
        {
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            stateManager.Add(new AnimatedSpriteVirtualPad("spritesSample"), false);
            stateManager.Add(new CubeSample("cubesSample"), false);
            stateManager.Add(new HeighmapTerrain("heightmapSample"), false);
            stateManager.Add(new StorageScreen("storageSample"), false);
        }

        // Pause the current demo
        public void Pause()
        {
            stateManager.PauseAllStates();
        }

        // Launch a demo
        public void RunState(string name)
        {
            stateManager.SetActive(name, false);
        }
    }
}
