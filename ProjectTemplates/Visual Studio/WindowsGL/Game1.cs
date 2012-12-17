using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna;
using Yna.State;

namespace $safeprojectname$
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : YnGame
    {
        public Game1() 
			: base(1024, 768, "Yna Game Framework")
        {

        }

        protected override void Initialize()
        {
            base.Initialize();
			
			// You code here
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.loadContent();
			
			// Your code here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
			
			if (YnG.Keys.JustPressed(Keys.Escape))
				YnG.Exit();
				
			// Your code here
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
			
			// Your code here
        }
    }
}
