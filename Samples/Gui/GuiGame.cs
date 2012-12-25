using System;
using Yna.Framework;

namespace Yna.Samples
{
    public class GuiGame : YnGame
    {

        public GuiGame()
            : base (800, 600, "Yna Framework : Graphical User Interface Sample")
        {

        }

        protected override void Initialize()
        {
            base.Initialize();

            GuiState guiState = new GuiState("gui");
            YnG.StateManager.Add(guiState);
        }

        public static void Main(string[] args)
        {
            using (GuiGame game = new GuiGame())
            {
                game.Run();
            }
        }
    }
}