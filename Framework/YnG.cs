using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Yna.Framework.Audio;
using Yna.Framework.Display;
using Yna.Framework.Helpers;
using Yna.Framework.Input;
using Yna.Framework.State;
using Yna.Framework.Storage;

namespace Yna.Framework
{
    /// <summary>
    /// Static class that expose important object relative to the current context like
    /// Game, GraphicsDevice, Input, etc...
    /// </summary>
    public static class YnG
    {
        /// <summary>
        /// Get or Set the Game instance
        /// </summary>
        public static Game Game { get; set; }
        

        /// <summary>
        /// Get the GraphicsDevice instance relative to the Game object
        /// </summary>
        public static GraphicsDevice GraphicsDevice 
        { 
            get { return Game.GraphicsDevice; } 
        }

        #region Properties for Managers

        /// <summary>
        /// Get the GraphicsDeviceManager relative to the Game object
        /// </summary>
        public static GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        
        /// <summary>
        /// Get the ContentManager instance relative to the Game object
        /// </summary>
        public static ContentManager Content 
        { 
            get { return Game.Content; } 
        }

        /// <summary>
        /// Get or Set the State Manager
        /// </summary>
        public static StateManager StateManager { get; set; }

        /// <summary>
        /// Get or Set the audio manager
        /// </summary>
        public static AudioManager AudioManager { get; set; }

        public static StorageManager StorageManager { get; set; }

        #endregion

        #region Properties for input

        /// <summary>
        /// Get or Set the keyboard states
        /// </summary>
        public static YnKeyboard Keys { get; set; }

        /// <summary>
        /// Get or Set the Gamepad states
        /// </summary>
        public static YnGamepad Gamepad { get; set; }

        /// <summary>
        /// Get or Set the mouse states
        /// </summary>
        public static YnMouse Mouse { get; set; }

        /// <summary>
        /// Get or Set the Touch states
        /// </summary>
        public static YnTouch Touch { get; set; }

        #endregion

        #region Properties for sizes

        /// <summary>
        /// Get the width of the current viewport
        /// </summary>
        public static int Width
        {
            get 
            {
                if (GraphicsDevice != null)
                    return GraphicsDevice.Viewport.Width;
                else
                    return DeviceWidth;
            }
        }

        /// <summary>
        /// Get the height of the current viewport
        /// </summary>
        public static int Height
        {
            get 
            {
                if (GraphicsDevice != null)
                    return GraphicsDevice.Viewport.Height;
                else
                    return DeviceHeight;
            }
        }

        /// <summary>
        /// Get or Set the width of the screen
        /// </summary>
		public static int DeviceWidth 
        {
            get { return GraphicsDeviceManager.PreferredBackBufferWidth; }
        }

		/// <summary>
		/// Get or Set the height of the screen
		/// </summary>
		public static int DeviceHeight
        {
            get { return GraphicsDeviceManager.PreferredBackBufferHeight; }
        }

		/// <summary>
		/// Show of hide the mouse pointer
		/// </summary>
        public static bool ShowMouse
        {
            get { return Game.IsMouseVisible; }
            set { Game.IsMouseVisible = value; }
        }

        #endregion

        #region Properties for Screen managment

        /// <summary>
        /// Get the rectangle that represent the screen size
        /// </summary>
        public static Rectangle ScreenRectangle
        {
            get { return new Rectangle(0, 0, Width, Height); }
        }
		
        /// <summary>
        /// Get the center of the screen on X axis
        /// </summary>
        public static int ScreenCenterX
        {
#if !ANDROID && !WINDOWS_PHONE_7 && !WINDOWS_PHONE_8
            get { return Game.Window.ClientBounds.Width / 2; }
#else
			get { return YnG.Width / 2; }
#endif
        }

        /// <summary>
        /// Get the center of the screen on Y axis
        /// </summary>
        public static int ScreenCenterY
        {
#if !ANDROID && !WINDOWS_PHONE_7 && !WINDOWS_PHONE_8
            get { return Game.Window.ClientBounds.Height / 2; }
#else
			get { return YnG.Height / 2; }
#endif
        }


        /// <summary>
        /// Change the screen resolution
        /// </summary>
        /// <param name="width">Screen width</param>
        /// <param name="height">Screen height</param>
        public static void SetScreenResolution(int width, int height)
        {
        	(Game as YnGame).SetScreenResolution(width, height);
        }

        /// <summary>
        /// Set the screen resolution to the same resolution used on desktop
        /// </summary>
        /// <param name="fullscreen"></param>
        public static void DetermineBestResolution(bool fullscreen)
        {
            (Game as YnGame).DetermineBestResolution(true);
        }

        #endregion

        #region Properties for StateManager

        /// <summary>
        /// Switch to a new state, just pass a new instance of a state and 
        /// the StateManager will clear all other state and use your new state
        /// </summary>
        /// <param name="state">New state</param>
        public static void SwitchState(BaseState state)
        {
            if (StateManager != null)
                StateManager.SwitchState(state);
        }

        public static void SetStateActive(string name, bool desactiveOtherScreens)
        {
            if (StateManager != null)
                StateManager.SetScreenActive(name, desactiveOtherScreens);
        }

        #endregion

        /// <summary>
        /// Get the registerd service for this game
        /// </summary>
        public static GameServiceContainer Services
        {
            get { return Game.Services; }
        }

        /// <summary>
        /// Get the components for this game
        /// </summary>
        public static GameComponentCollection Components
        {
            get { return Game.Components; }
        }

        /// <summary>
        /// Close the game
        /// </summary>
        public static void Exit()
        {
            Game.Exit();
        }
    }
}
