﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Engine;
using Yna.Engine.Graphics;

namespace Yna.Samples
{
    public class StorageGame : YnGame
    {
        private const string ScoreContainerName = "scores_container";
        private const string ScoreFileName = "scores.sav";

        private PlayerScore playerScore;
        private YnText playerDebugText;
#if !NETFX_CORE
        public StorageGame()
            : base(640, 480, "Storage sample", false)
#else
        public StorageGame()
            : base()
#endif
        {
            // We define a name for the game who's used by the storage manager for read/write in the good folder
            GameTitle = "Storage Sample";

            // Create a simple debug text to draw some informations
            playerDebugText = new YnText("Fonts/DefaultFont", "Player informations");
        }

        protected override void Initialize()
        {
            base.Initialize();

            // Get the player score previously saved, if not exist we create a new instance
            playerScore = YnG.StorageManager.Load<PlayerScore>(ScoreContainerName, ScoreFileName);

            if (playerScore == null)
                playerScore = new PlayerScore();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // Load and initialize the debut text 
            playerDebugText.LoadContent();
            playerDebugText.Scale = new Vector2(1.5f);
            playerDebugText.Position = new Vector2(YnG.Width / 2 - playerDebugText.Width / 2, YnG.Height / 2 - playerDebugText.Height / 2);
            playerDebugText.Color = Color.BlanchedAlmond;
        }

        protected override void Update(GameTime gameTime)
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

#if WINDOWS_PHONE
            if (YnG.Gamepad.JustPressed(PlayerIndex.One, Buttons.Back))
            {
                Random random = new Random();
                playerScore.TimePlayed = random.Next(0, 1500);
                playerScore.Scores.Add(random.Next(100, 3500));
                YnG.StorageManager.SaveDatas<PlayerScore>(ScoreContainerName, ScoreFileName, playerScore);
            }
#endif

            // Update text
            playerDebugText.Text = playerScore.ToString();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.Clear(Color.DarkSlateBlue);

            // Draw the player's information to the screen
            spriteBatch.Begin();
            playerDebugText.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

#if !NETFX_CORE && !WINDOWS_PHONE
        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        public static void Main(string[] args)
        {
            using (StorageGame game = new StorageGame())
            {
                game.Run();
            }
        }
#endif
    }
}