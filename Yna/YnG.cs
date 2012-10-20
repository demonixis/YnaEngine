using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Display;
using Yna.Display3D;
using Yna.Helpers;
using Yna.State;
using Yna.Input;

namespace Yna
{
    /// <summary>
    /// Determine the MonoGame platform
    /// </summary>
    public enum MonoGameContext
    {
        Unknow = 0, XNA, Windows, Windows8, Linux, Mac 
    }

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
        public static ScreenManager StateManager { get; set; }
		
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
        /// Get or Set the version of XNA or MonoGame you are using
        /// </summary>
        public static MonoGameContext MonoGameContext { get; set; }

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
            set { GraphicsDeviceManager.PreferredBackBufferWidth = value; }
        }

		/// <summary>
		/// Get or Set the height of the screen
		/// </summary>
		public static int DeviceHeight
        {
            get { return GraphicsDeviceManager.PreferredBackBufferHeight; }
            set { GraphicsDeviceManager.PreferredBackBufferHeight = value; }
        }

		/// <summary>
		/// Show of hide the mouse pointer
		/// </summary>
        public static bool ShowMouse
        {
            get { return Game.IsMouseVisible; }
            set { Game.IsMouseVisible = value; }
        }

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
            get { return Game.Window.ClientBounds.Width / 2; }
        }

        /// <summary>
        /// Get the center of the screen on Y axis
        /// </summary>
        public static int ScreenCenterY
        {
            get { return Game.Window.ClientBounds.Height / 2; }
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
        public static void SetScreenResolutionToMax(bool fullscreen)
        {
            (Game as YnGame).SetScreenResolutionToMax(true);
        }

        /// <summary>
        /// Switch to a new state, just pass a new instance of a state and 
        /// the StateManager will clear all other state and use your new state
        /// </summary>
        /// <param name="state">New state</param>
        public static void SwitchState(YnState state)
        {
            (Game as YnGame).SwitchState(state);
        }

        public static void SwitchState(YnState3D state)
        {
            (Game as YnGame).SwitchState(state);
        }

        /// <summary>
        /// Close the game
        /// </summary>
        public static void Exit()
        {
            Game.Exit();
        }

        /// <summary>
        /// Get the platform context 
        /// </summary>
        /// <returns>MonoGame platform used</returns>
        public static MonoGameContext GetPlateformContext()
        {
#if NETFX_CORE
            return MonoGameContext.Windows8;
#else
            MonoGameContext context = MonoGameContext.Unknow; // Default

            Assembly[] AssembliesLoaded = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in AssembliesLoaded)
            {
                if (assembly.FullName.Contains("MonoGame.Framework.Windows"))
                    context = MonoGameContext.Windows;
                else if (assembly.FullName.Contains("MonoGame.Framework.Windows8"))
                    context = MonoGameContext.Windows8;
                else if (assembly.FullName.Contains("MonoGame.Framework.Linux"))
                    context = MonoGameContext.Linux;
                else if (assembly.FullName.Contains("Microsoft.XNA.Framework"))
                    context = MonoGameContext.XNA;
            }

            return context;
#endif
        }
    }
}
