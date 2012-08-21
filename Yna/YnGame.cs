using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Yna.State;
using Yna.Helpers;
using Yna.Input;
using Yna.Input.Service;
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
            Components.Add(new GamepadService(this));
            
             // Portée global pour l'ensemble du jeu
            YnG.Game = this;
            YnG.GraphicsDeviceManager = this.graphics;
            YnG.DeviceWidth = this.graphics.PreferredBackBufferWidth;
            YnG.DeviceHeight = this.graphics.PreferredBackBufferHeight;
            YnG.Keys = new YnKeyboard();
            YnG.Mouse = new YnMouse();
            YnG.Gamepad = new YnGamepad();
            YnG.Camera = new Camera2D();
            YnG.MonoGameContext = YnG.GetPlateformContext();
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
        /// Change the screen resolution
        /// </summary>
        /// <param name="width">Screen width</param>
        /// <param name="height">Screen height</param>
        public void SetScreenResolution(int width, int height)
        {
            this.graphics.PreferredBackBufferWidth = width;
            this.graphics.PreferredBackBufferHeight = height;
            this.graphics.ApplyChanges();
        }

        /// <summary>
        /// Switch to a new state, just pass a new instance of a state and 
        /// the StateManager will clear all other state and use your new state
        /// </summary>
        /// <param name="state">New state</param>
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
        /// Close the game
        /// </summary>
        public void ExitGame()
        {
            Exit();
        }
    }
}

