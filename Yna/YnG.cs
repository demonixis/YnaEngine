using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Yna.Audio;
using Yna.Content;
using Yna.Display;
using Yna.Display.Gui;
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
        public static ScreenManager ScreenManager { get; set; }

        /// <summary>
        /// Get or Set the audio manager
        /// </summary>
        public static AudioManager AudioManager { get; set; }

        /// <summary>
        /// Get or set a custom content manager, who allow you to don't use XNA's ContentManager for loading
        /// Texture2D, Song & SoundEffect. It is disabled by defaut but it can be automatically activated if you load
        /// an asset from stream in a YnManager*
        /// </summary>
        public static YnContent YnContent { get; set; }

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

        /// The default UI skin
        /// </summary>
        public static YnSkin DefaultSkin { get; set; }

        /// <summary>
        /// The GUI manager
        /// </summary>
        public static YnGui Gui { get; set; }

        /// <summary>
        /// Get or Set the version of XNA or MonoGame you are using
        /// </summary>
        public static MonoGameContext MonoGameContext { get; set; }

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
        public static void DetermineBestResolution(bool fullscreen)
        {
            (Game as YnGame).DetermineBestResolution(true);
        }

        #endregion

        #region Properties for ScreenManager

        /// <summary>
        /// Switch to a new state, just pass a new instance of a state and 
        /// the StateManager will clear all other state and use your new state
        /// </summary>
        /// <param name="state">New state</param>
        public static void SwitchState(Screen state)
        {
            if (ScreenManager != null)
                ScreenManager.SwitchState(state);
        }

        public static void SetScreenActive(string name, bool desactiveOtherScreens)
        {
            if (ScreenManager != null)
                ScreenManager.SetScreenActive(name, desactiveOtherScreens);
        }

        #endregion

        #region Collide detection

        /// <summary>
        /// Simple test collision with rectangles
        /// </summary>
        /// <param name="sceneObjectA">Sprite 1</param>
        /// <param name="sceneObjectB">Sprite 2</param>
        /// <returns></returns>
        public static bool Collide(YnObject sceneObjectA, YnObject sceneObjectB)
        {
            return sceneObjectA.Rectangle.Intersects(sceneObjectB.Rectangle);
        }

        public static bool CollideOneWithGroup(YnObject sceneObject, YnGroup group)
        {
            bool collide = false;
            int size = group.Count;
            int i = 0;

            while (i < size && !collide)
            {
                if (sceneObject.Rectangle.Intersects(group[i].Rectangle))
                    collide = true;
                
				i++; 
            }

            return collide;
        }

        public static bool CollideGroupWithGroup(YnGroup groupA, YnGroup groupB)
        {
            bool collide = false;
            int i = 0;
            int j = 0;
            int groupASize = groupA.Count;
            int groupBSize = groupB.Count;

            while (i < groupASize && !collide)
            {
                while (j < groupBSize && !collide)
                {
                    if (groupA[i].Rectangle.Intersects(groupB[j].Rectangle))
                        collide = true;
					
					j++;
                }
				i++;
            }

            return collide;
        }

        /// <summary>
        /// Perfect pixel test collision
        /// </summary>
        /// <param name="sceneObjectA">Sprite 1</param>
        /// <param name="sceneObjectB">Sprite 2</param>
        /// <returns></returns>
        public static bool PerfectPixelCollide(YnObject sceneObjectA, YnObject sceneObjectB)
        {
            int top = Math.Max(sceneObjectA.Rectangle.Top, sceneObjectB.Rectangle.Top);
            int bottom = Math.Min(sceneObjectA.Rectangle.Bottom, sceneObjectB.Rectangle.Bottom);
            int left = Math.Max(sceneObjectA.Rectangle.Left, sceneObjectB.Rectangle.Left);
            int right = Math.Min(sceneObjectA.Rectangle.Right, sceneObjectB.Rectangle.Right);

            for (int y = top; y < bottom; y++)  // De haut en bas
            {
                for (int x = left; x < right; x++)  // de gauche à droite
                {
                    int index_A = (x - sceneObjectA.Rectangle.Left) + (y - sceneObjectA.Rectangle.Top) * sceneObjectA.Rectangle.Width;
                    int index_B = (x - sceneObjectB.Rectangle.Left) + (y - sceneObjectB.Rectangle.Top) * sceneObjectB.Rectangle.Width;

                    Color[] colorsSpriteA = GraphicsHelper.GetTextureData(sceneObjectA);
                    Color[] colorsSpriteB = GraphicsHelper.GetTextureData(sceneObjectB);

                    Color colorSpriteA = colorsSpriteA[index_A];
                    Color colorSpriteB = colorsSpriteB[index_B];

                    if (colorSpriteA.A != 0 && colorSpriteB.A != 0)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Optimised perfect collide test
        /// </summary>
        /// <param name="sceneObjectA">Sprite 1</param>
        /// <param name="sceneObjectB">Sprite 2</param>
        /// <returns></returns>
        public static bool PerfectCollide(YnObject sceneObjectA, YnObject sceneObjectB)
        {
            return Collide(sceneObjectA, sceneObjectB) && PerfectPixelCollide(sceneObjectA, sceneObjectB);
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

        /// <summary>
        /// Get the current platform context 
        /// </summary>
        /// <returns>MonoGame/XNA platform used</returns>
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
