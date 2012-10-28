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
            YnG.StateManager = screenManager;
            YnG.AudioManager = AudioManager.Instance;
            YnG.Gui = new YnGui(this);

            this.Window.Title = "YNA Game";
        }

        public YnGame(int width, int height, string title)
            : this()
        {
            SetScreenResolution(width, height);
            this.Window.Title = title;
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
        }

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

        public void SetScreenResolutionToMax(bool fullscreen)
        {
            SetScreenResolution(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

            if (fullscreen)
                graphics.ToggleFullScreen();
        }

        public void ActiveStateManager(bool active)
        {
            if (active)
            {
                if (YnG.StateManager == null)
                {
                    YnG.StateManager = new ScreenManager(this);
                    this.Components.Add(YnG.StateManager);
                }
            }
            else
            {
                if (YnG.StateManager != null)
                {
                    this.Components.Remove(YnG.StateManager);
                    YnG.StateManager = null;
                }
            }
        }

        // TODO : Move that in ScreenManager.cs

        /// <summary>
        /// Switch to a new state, just pass a new instance of a state and 
        /// the StateManager will clear all other states and use the new state
        /// </summary>
        /// <param name="state">New state</param>
        /// <returns>True if the state manager has done the swith, false if it disabled</returns>
        public bool SwitchState(YnState nextState)
        {
            if (YnG.StateManager != null)
            {
                if (!nextState.IsPopup)
                    YnG.StateManager.Clear();

                YnG.StateManager.Add(nextState);

                return true;
            }

            return false;
        }

        public bool SwitchState(YnState3D nextState)
        {
            if (YnG.StateManager != null)
            {
                if (!nextState.IsPopup)
                    YnG.StateManager.Clear();

                YnG.StateManager.Add(nextState);

                return true;
            }

            return false;
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

