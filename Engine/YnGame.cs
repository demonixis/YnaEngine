// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Audio;
using Yna.Engine.State;
using Yna.Engine.Helpers;
using Yna.Engine.Input;
using Yna.Engine.Storage;

namespace Yna.Engine
{
    /// <summary>
    /// The game class
    /// </summary>
    public class YnGame : Game
    {
        protected GraphicsDeviceManager graphics = null;
        protected SpriteBatch spriteBatch = null;
        protected StateManager stateManager = null;
        public static string GameTitle = "Yna Game";
        public static string GameVersion = "1.0.0.0";

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

            YnKeyboard keyboardComponent = new YnKeyboard(this);
            YnMouse mouseComponent = new YnMouse(this);
            YnGamepad gamepadComponent = new YnGamepad(this);
            YnTouch touchComponent = new YnTouch(this);

            Components.Add(keyboardComponent);
            Components.Add(mouseComponent);
            Components.Add(gamepadComponent);
            Components.Add(touchComponent);
            Components.Add(stateManager);

            // Registry globals objects
            YnG.Game = this;
            YnG.GraphicsDeviceManager = this.graphics;
            YnG.Keys = keyboardComponent;
            YnG.Mouse = mouseComponent;
            YnG.Gamepad = gamepadComponent;
            YnG.Touch = touchComponent;
            YnG.StateManager = stateManager;
            YnG.StorageManager = new StorageManager();
            YnG.AudioManager = new AudioManager();

#if !ANDROID
            this.Window.Title = String.Format("{0} - v{1}", GameTitle, GameVersion);
#endif
            ScreenHelper.ScreenWidthReference = graphics.PreferredBackBufferWidth;
            ScreenHelper.ScreenHeightReference = graphics.PreferredBackBufferHeight;

#if WINDOWS_PHONE_7
            // 30 FPS for Windows Phone 7
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Battery saving when screen suspended
            InactiveSleepTime = TimeSpan.FromSeconds(1);
#endif
        }

        public YnGame(int width, int height, string title)
            : this()
        {
#if XNA || MONOGAME && (OPENGL || DIRECTX || LINUX || MACOSX) ||SDL2
            SetScreenResolution(width, height);
          
            this.Window.Title = title;

            ScreenHelper.ScreenWidthReference = width;
            ScreenHelper.ScreenHeightReference = height;
#endif
        }

        public YnGame(int width, int height, string title, bool useStateManager)
            : this(width, height, title)
        {
            if (!useStateManager)
            {
                this.stateManager.Enabled = false;
                this.Components.Remove(this.stateManager);
            }
        }

        #endregion

        #region GameState pattern

        /// <summary>
        /// Load assets from content manager
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            GraphicsDevice.Viewport = new Viewport(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }

        /// <summary>
        /// Unload assets off content manager and suspend managers
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();

            YnG.AudioManager.Dispose();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
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
    }
}

