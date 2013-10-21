using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Samples.Screens.Score;

namespace Yna.Samples
{
    public class StorageScreen : YnState2D
    {
        private const string ScoreContainerName = "scores_container";
        private const string ScoreFileName = "scores.sav";

        private PlayerScore playerScore;
        private YnText playerDebugText;

        public StorageScreen(string name)
            : base(name)
        {
            // Create a simple debug text to draw some informations
            playerDebugText = new YnText("Fonts/DefaultFont", "Player informations");
            Add(playerDebugText);
        }

        public override void Initialize()
        {
            base.Initialize();

            // Get the player score previously saved, if not exist we create a new instance
           // playerScore = YnG.StorageManager.LoadDatas<PlayerScore>(ScoreContainerName, ScoreFileName);

            if (playerScore == null)
                playerScore = new PlayerScore();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            // Load and initialize the debut text 
            playerDebugText.Scale = new Vector2(1.5f);
            playerDebugText.Position = new Vector2(YnG.Width / 2 - playerDebugText.Width / 2, YnG.Height / 2 - playerDebugText.Height / 2);
            playerDebugText.Color = Color.BlanchedAlmond;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Press S to Save datas
            if (YnG.Keys.JustPressed(Keys.S))
            {
                YnG.StorageManager.Save<PlayerScore>(ScoreContainerName, ScoreFileName, playerScore);
            }

            // Press L to Load datas
            if (YnG.Keys.JustPressed(Keys.L))
            {
                playerScore = YnG.StorageManager.Load<PlayerScore>(ScoreContainerName, ScoreFileName);
            }

            // Press R to change datas with random values
            if (YnG.Keys.JustPressed(Keys.R))
            {
                Random random = new Random();
                playerScore.TimePlayed = random.Next(0, 1500);
                playerScore.Scores.Add(random.Next(100, 3500));
            }

            // Update text
            playerDebugText.Text = playerScore.ToString();
        }
    }
}
