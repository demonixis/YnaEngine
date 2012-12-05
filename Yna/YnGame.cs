using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Yna.Audio;
using Yna.State;
using Yna.Helpers;
using Yna.Input;
using Yna.Input.Service;
using Yna.Display;
using Yna.Display3D;
using Yna.Display.Gui;

namespace Yna
{
    /// <summary>
    /// Global Game container
    /// </summary>
    public class YnGame : Game
    {
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;
        protected ScreenManager screenManager = null;
        protected string gameTitle = "Yna Game";
        protected string gameVersion = "0.1";

        #region Events 

        /// <summary>
        /// Triggered when the screen resolution changed
        /// </summary>
        public event EventHandler<ScreenChangedEventArgs> ScreenResolutionChanged = null;

        private void OnScreenResolutionChanged(ScreenChangedEventArgs e)
        {
            if (ScreenResolutionChanged != null)
                ScreenResolutionChanged(this, e);
        }

        #endregion

        #region Constructors

        public YnGame()
            : base()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.screenManager = new ScreenManager(this);

            // Setup services
            ServiceHelper.Game = this;
            Components.Add(new KeyboardService(this));
            Components.Add(new MouseService(this));
            Components.Add(new GamepadService(this));
            Components.Add(screenManager);

            // Registry globals objects
            YnG.Game = this;
            YnG.GraphicsDeviceManager = this.graphics;
            YnG.DeviceWidth = this.graphics.PreferredBackBufferWidth;
            YnG.DeviceHeight = this.graphics.PreferredBackBufferHeight;
            YnG.Keys = new YnKeyboard();
            YnG.Mouse = new YnMouse();
            YnG.Gamepad = new YnGamepad();
            YnG.MonoGameContext = YnG.GetPlateformContext();
            YnG.ScreenManager = screenManager;
            YnG.AudioManager = new AudioManager();

            this.Window.Title = String.Format("{0} - v{1}", gameTitle, gameVersion);

            ScreenHelper.ScreenWidthReference = graphics.PreferredBackBufferWidth;
            ScreenHelper.ScreenHeightReference = graphics.PreferredBackBufferHeight;

#if WINDOWS_PHONE_7
            // La fréquence d’image est de 30 i/s pour le Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Augmenter la durée de la batterie sous verrouillage.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
#endif
        }

#if !WINDOWS8 && !WINDOWS_PHONE_7 && !WINDOWS_PHONE_8

        public YnGame(int width, int height, string title)
            : this()
        {
            SetScreenResolution(width, height);
          
            this.Window.Title = title;

            ScreenHelper.ScreenWidthReference = width;
            ScreenHelper.ScreenHeightReference = height;
        }

        public YnGame(int width, int height, string title, bool useScreenManager)
            : this(width, height, title)
        {
            UseScreenManager(useScreenManager);
        }

#endif

        #endregion

        #region GameState pattern

        protected override void LoadContent()
        {
            base.LoadContent();
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        #endregion

        #region Resolution setup

        /// <summary>
        /// Change the screen resolution
        /// </summary>
        /// <param name="width">Screen width</param>
        /// <param name="height">Screen height</param>
        public virtual void SetScreenResolution(int width, int height)
        {
            this.graphics.PreferredBackBufferWidth = width;
            this.graphics.PreferredBackBufferHeight = height;
            this.graphics.ApplyChanges();

            OnScreenResolutionChanged(new ScreenChangedEventArgs(width, height, this.graphics.IsFullScreen));
        }

        /// <summary>
        /// Set maximum resolution supported by the device, It use the desktop resolution
        /// </summary>
        /// <param name="fullscreen">Toggle in fullscreen mode</param>
        public virtual void DetermineBestResolution(bool fullscreen)
        {
            SetScreenResolution(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

            if (!graphics.IsFullScreen && fullscreen)
                graphics.ToggleFullScreen();
        }

        #endregion

        /// <summary>
        /// Active or desactive the Screen Manager
        /// </summary>
        /// <param name="active">True for activing, false for desactivating</param>
        public void UseScreenManager(bool active)
        {
            if (active)
            { 
                if (this.screenManager == null)
                {
                    this.screenManager = new ScreenManager(this);
                    this.Components.Add(this.screenManager);
                }
            }
            else
            {
                if (this.screenManager != null)
                {
                    this.Components.Remove(this.screenManager);
                    this.screenManager = null;
                }
            }

            YnG.ScreenManager = this.screenManager;
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

