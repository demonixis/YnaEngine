using System;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Yna.Display;
using Yna.Helpers;
using Yna.State;
using Yna.Input;

namespace Yna
{
    public enum MonoGameContext
    {
        Unknow = 0, XNA, Windows, Windows8, Linux, Mac 
    }

    public static class YnG
    {
        public static Game Game { get; set; }
        
        public static GraphicsDevice GraphicsDevice 
        { 
            get { return Game.GraphicsDevice; } 
        }

        public static GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        
        public static ContentManager Content 
        { 
            get { return Game.Content; } 
        }

        public static StateManager StateManager { get; set; }
		
        public static YnKeyboard Keys { get; set; }

        public static YnMouse Mouse { get; set; }
        
        public static Camera2D Camera { get; set; }

        public static MonoGameContext MonoGameContext { get; set; }

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
        /// Obtient la largeur de l'écran
        /// </summary>
		public static int DeviceWidth 
        {
            get { return GraphicsDeviceManager.PreferredBackBufferWidth; }
            set { GraphicsDeviceManager.PreferredBackBufferWidth = value; }
        }

		/// <summary>
		/// Obtient la hauteur d'écran
		/// </summary>
		public static int DeviceHeight
        {
            get { return GraphicsDeviceManager.PreferredBackBufferHeight; }
            set { GraphicsDeviceManager.PreferredBackBufferHeight = value; }
        }

        public static void SetScreenResolution(int width, int height)
        {
        	(Game as YnGame).SetScreenResolution(width, height);
        }

        public static void SwitchState(YnState state)
        {
            (Game as YnGame).SwitchState(state);
        }

        public static void AddScreen(GameState gameState)
        {
            StateManager.Add(gameState);
        }

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
        }
    }
}
