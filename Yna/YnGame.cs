using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Yna.State;
using Yna.Helpers;
using Yna.Input;
using Yna.Display;

namespace Yna
{
	public class YnGame : Game
	{
		protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;
        protected StateManager stateManager;

        public YnGame()
        {
        	this.graphics = new GraphicsDeviceManager (this);
			this.Content.RootDirectory = "Content";
            this.stateManager = new StateManager(this);
			
			 // Définition des services
            ServiceHelper.Game = this;
            Components.Add(new KeyboardService(this));
            Components.Add(new MouseService(this));
            
             // Portée global pour l'ensemble du jeu
            YnG.Game = this;
            YnG.GraphicsDeviceManager = this.graphics;
            YnG.DeviceWidth = this.graphics.PreferredBackBufferWidth;
            YnG.DeviceHeight = this.graphics.PreferredBackBufferHeight;
            YnG.Keys = new YnKeyboard();
            YnG.Mouse = new YnMouse();
            YnG.Camera = new Camera2D();
            YnG.StateManager = stateManager;
            
            this.Window.Title = "YNA Game";
        }
        
		public YnGame (int width, int height, string title)
			: this()
		{
			SetScreenResolution(width, height);
			this.Window.Title = title;
		}

        #region GameState Pattern
        protected override void LoadContent()
        {
            base.LoadContent();

            stateManager.LoadContent();
        }

        /// <summary>
        /// Initialisation des ressources
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            stateManager.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            stateManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            stateManager.Draw(gameTime);
            base.Draw(gameTime);
        }
        #endregion

        /// <summary>
		/// Change la résolution d'affichage
		/// </summary>
		/// <param name="width">Longueur</param>
		/// <param name="height">Largeur</param>
        public void SetScreenResolution(int width, int height)
        {
            this.graphics.PreferredBackBufferWidth = width;
            this.graphics.PreferredBackBufferHeight = height;
            this.graphics.ApplyChanges();
        }

        /// <summary>
        /// Remplace l'écran d'état courant par l'état passé en paramètre
        /// </summary>
        /// <param name="nextState">Prochain écran</param>
        public void SwitchState(YnState nextState)
        {
            if (!nextState.IsPopup)
                YnG.StateManager.Clear();

            YnG.StateManager.Add(nextState); 
        }

        protected override void UnloadContent()
        {
            stateManager.UnloadContent();
            base.UnloadContent();
        }

        /// <summary>
        /// Quitte le programme
        /// </summary>
        public void ExitGame()
        {
            Exit();
        }
    }
}

