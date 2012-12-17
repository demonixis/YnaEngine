using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Audio;
using Yna.Framework.State;
using Yna.Framework.Helpers;
using Yna.Framework.Input;
using Yna.Framework.Input.Service;
using Yna.Framework.Display;
using Yna.Framework.Display.Event;
using Yna.Framework.Display3D;
using Yna.Framework.Display.Gui;
using Yna.Framework.Storage;

namespace Yna.Framework
{
    /// <summary>
    /// The game class
    /// </summary>
    public class YnGame : Game
    {
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;
        protected StateManager stateManager = null;
        public static string GameTitle = "Yna Game";
        public static string GameVersion = "1.0.0.0";

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

        /// <summary>
        /// Create and setup the game engine
        /// Graphics, Services and helpers are initialized
        /// </summary>
        public YnGame()
            : base()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.stateManager = new StateManager(this);

            // Setup services
            ServiceHelper.Game = this;
            Components.Add(new KeyboardService(this));
            Components.Add(new MouseService(this));
            Components.Add(new GamepadService(this));
            Components.Add(new TouchService(this));
            Components.Add(stateManager);

            // Registry globals objects
            YnG.Game = this;
            YnG.GraphicsDeviceManager = this.graphics;
            YnG.Keys = new YnKeyboard();
            YnG.Mouse = new YnMouse();
            YnG.Gamepad = new YnGamepad();
            YnG.Touch = new YnTouch();
            YnG.StateManager = stateManager;
#if !WINDOWS_PHONE_8
            YnG.AudioManager = new AudioManager();
#endif
            YnG.StorageManager = new StorageManager();
#if !ANDROID
            this.Window.Title = String.Format("{0} - v{1}", GameTitle, GameVersion);
#endif
            ScreenHelper.ScreenWidthReference = graphics.PreferredBackBufferWidth;
            ScreenHelper.ScreenHeightReference = graphics.PreferredBackBufferHeight;

#if WINDOWS_PHONE_7
            // 30 FPS for Windows Phone 7
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Battery save when screen suspend
            InactiveSleepTime = TimeSpan.FromSeconds(1);
#endif
        }

#if !WINDOWS8 && !WINDOWS_PHONE_7 && !WINDOWS_PHONE_8 && !ANDROID

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
            SetStateManagerActive(useScreenManager);
        }

#endif

        #endregion

        #region GameState pattern

        /// <summary>
        /// Load assets from content manager
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// Unload assets off content manager and suspend managers
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();

            YnG.AudioManager.Dispose();
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
        public void SetStateManagerActive(bool active)
        {
            if (active)
            { 
                if (this.stateManager == null)
                {
                    this.stateManager = new StateManager(this);
                    this.Components.Add(this.stateManager);
                }
            }
            else
            {
                if (this.stateManager != null)
                {
                    this.Components.Remove(this.stateManager);
                    this.stateManager = null;
                }
            }

            YnG.StateManager = this.stateManager;
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

