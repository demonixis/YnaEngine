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
        protected ScreenManager screenManager;
        protected YnGui guiManager;

        #region Constructors

        public YnGame()
            : base()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.screenManager = new ScreenManager(this);
            this.guiManager = new YnGui(this);

            // Setup services
            ServiceHelper.Game = this;
            Components.Add(new KeyboardService(this));
            Components.Add(new MouseService(this));
            Components.Add(new GamepadService(this));
            Components.Add(screenManager);
            Components.Add(guiManager);

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
            YnG.AudioManager = AudioManager.Instance;
            YnG.Gui = guiManager;

            this.Window.Title = "YNA Game";
        }

        public YnGame(int width, int height, string title)
            : this()
        {
            SetScreenResolution(width, height);
            this.Window.Title = title;
        }

        public YnGame(int width, int height, string title, bool useScreenManager)
            : this(width, height, title)
        {
            UseScreenManager(useScreenManager);
        }

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
        }

        /// <summary>
        /// Set maximum resolution supported by the device, It use the desktop resolution
        /// </summary>
        /// <param name="fullscreen">Toggle in fullscreen mode</param>
        public virtual void SetScreenResolutionToMax(bool fullscreen)
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
                if (YnG.ScreenManager == null)
                {
                    YnG.ScreenManager = new ScreenManager(this);
                    this.Components.Add(YnG.ScreenManager);
                }
            }
            else
            {
                if (YnG.ScreenManager != null)
                {
                    this.Components.Remove(YnG.ScreenManager);
                    YnG.ScreenManager = null;
                }
            }
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

