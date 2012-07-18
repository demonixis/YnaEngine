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
        

        public YnGame()
        {
        	this.graphics = new GraphicsDeviceManager (this);
			this.Content.RootDirectory = "Content";
			
			 // D�finition des services
            ServiceHelper.Game = this;
            Components.Add(new KeyboardService(this));
            Components.Add(new MouseService(this));
            
             // Port�e global pour l'ensemble du jeu
            YnG.Game = this;
            YnG.GraphicsDeviceManager = this.graphics;
            #if WINDOWS_PHONE
            SetScreenResolution(480, 800);
            #elif NETFX_CORE
            SetScreenResolution(1024, 768);
            #else
            SetScreenResolution(800, 600);
            #endif
            
            YnG.Keys = new YnKeyboard();
            YnG.Mouse = new YnMouse();
            YnG.Camera = new Camera2D();
            
            this.Window.Title = "YNA Game";
        }
        
		public YnGame (int width, int height, string title, bool useScreenManager = true)
			: this()
		{
			SetScreenResolution(width, height);
			
			this.Window.Title = title;
		
            // D�finition du ScreenManager
            UseScreenManager = useScreenManager;
		}
		
		/// <summary>
		/// Active ou d�sactive le gestionnaire d'�tat
		/// </summary>
        public bool UseScreenManager
        {
            get { return (YnG.ScreenManager != null); }
            set
            {
                if (value)
                {
                    if (YnG.ScreenManager == null)
                    {
                        YnG.ScreenManager = new StateManager(this);
                        Components.Add(YnG.ScreenManager);
                    }
                    else
                    {
                        Components.Remove(YnG.ScreenManager);
                        YnG.ScreenManager.Dispose();
                        YnG.ScreenManager = null;
                    }
                }
            }
        }

        /// <summary>
        /// Initialisation des ressources
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
        }

		/// <summary>
		/// Change la r�solution d'affichage
		/// </summary>
		/// <param name="width">Longueur</param>
		/// <param name="height">Largeur</param>
        public void SetScreenResolution(int width, int height)
        {
            this.graphics.PreferredBackBufferWidth = width;
            this.graphics.PreferredBackBufferHeight = height;
            //YnG.GraphicsDeviceManager.ApplyChanges();
        }

        /// <summary>
        /// Remplace l'�cran d'�tat courant par l'�tat pass� en param�tre
        /// </summary>
        /// <param name="nextState">Prochain �cran</param>
        public void SwitchState(YnState nextState)
        {
            if (YnG.ScreenManager != null)
            {
                // Si ce n'est pas une popup tous les �cran en cours passent � l'�tat "TransitionOff"
                // Et son automatiquement vid�s et supprim�s
                if (!nextState.IsPopup)
                    YnG.ScreenManager.Clear();

                YnG.ScreenManager.Add(nextState);
            }
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

