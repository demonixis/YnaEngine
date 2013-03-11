﻿using System;
using Yna.Engine;
using Yna.Engine.Graphics.Component;
using Yna.Samples.Screens;

namespace Yna.Samples
{
    public class Models3DGame : YnGame
    {
        YnMenu menu;
        MenuEntry[] menuItems;

        public Models3DGame()
            : base(800, 600, "Yna Samples : Models 3D Sample")
        {
            menuItems = new MenuEntry[]
            {
                new MenuEntry("dungeonSample", "Dungeon", "In this sample we load a model (fbx) and add it to the scene"),
            };
        }

        protected override void Initialize()
        {
            base.Initialize();

            menu = new YnMenu("menu", "3D models", menuItems);

            stateManager.Add(menu, true);
            stateManager.Add(new DungeonState("dungeonSample"), false);
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
