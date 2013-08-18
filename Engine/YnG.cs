// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Audio;
using Yna.Engine.Input;
using Yna.Engine.State;
using Yna.Engine.Storage;

namespace Yna.Engine
{
    /// <summary>
    /// Static class that expose important object relative to the current context like
    /// Game, GraphicsDevice, Input, etc...
    /// </summary>
    public static class YnG
    {
        /// <summary>
        /// Gets or Set the Game instance
        /// </summary>
        public static Game Game { get; set; }

        /// <summary>
        /// Gets the GraphicsDevice instance relative to the Game object
        /// </summary>
        public static GraphicsDevice GraphicsDevice
        {
            get { return Game.GraphicsDevice; }
        }

        #region Managers

        /// <summary>
        /// Gets the GraphicsDeviceManager relative to the Game object
        /// </summary>
        public static GraphicsDeviceManager GraphicsDeviceManager { get; set; }

        /// <summary>
        /// Gets the ContentManager instance relative to the Game object
        /// </summary>
        public static ContentManager Content
        {
            get { return Game.Content; }
        }

        /// <summary>
        /// Gets or Set the State Manager
        /// </summary>
        public static StateManager StateManager { get; set; }

        /// <summary>
        /// Gets or Set the audio manager
        /// </summary>
        public static AudioManager AudioManager { get; set; }

        /// <summary>
        /// Gets or sets the storage manager
        /// </summary>
        public static StorageManager StorageManager { get; set; }

        #endregion

        #region Inputs

        /// <summary>
        /// Gets or Set the keyboard states
        /// </summary>
        public static YnKeyboard Keys { get; set; }

        /// <summary>
        /// Gets or Set the mouse states
        /// </summary>
        public static YnMouse Mouse { get; set; }

        /// <summary>
        /// Gets or Set the Gamepad states
        /// </summary>
        public static YnGamepad Gamepad { get; set; }

        /// <summary>
        /// Gets or Set the Touch states
        /// </summary>
        public static YnTouch Touch { get; set; }

        public static bool ShowMouse
        {
            get { return Game.IsMouseVisible; }
            set { Game.IsMouseVisible = value; }
        }

        

        #endregion

        #region Screen size and screen management

        /// <summary>
        /// Gets the width of the current viewport
        /// </summary>
        public static int Width
        {
            get
            {
                if (GraphicsDeviceManager != null)
                    return GraphicsDeviceManager.PreferredBackBufferWidth;

                return Game.GraphicsDevice.Viewport.Width;
            }
        }

        /// <summary>
        /// Gets the height of the current viewport
        /// </summary>
        public static int Height
        {
            get
            {
                if (GraphicsDeviceManager != null)
                    return GraphicsDeviceManager.PreferredBackBufferHeight;

                return Game.GraphicsDevice.Viewport.Height;
            }
        }

        /// <summary>
        /// Gets the rectangle that represent the screen size
        /// </summary>
        public static Rectangle ScreenRectangle
        {
            get { return new Rectangle(0, 0, YnG.Game.GraphicsDevice.Viewport.Width, YnG.Game.GraphicsDevice.Viewport.Height); }
        }

        /// <summary>
        /// Gets the center of the screen on X axis
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
        /// Gets the center of the screen on Y axis
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

        #region StateManager

        public static void SetStateActive(string name, bool desactiveOtherStates)
        {
            if (StateManager != null)
                StateManager.SetActive(name, desactiveOtherStates);
        }

        #endregion

        /// <summary>
        /// Close the game
        /// </summary>
        public static void Exit()
        {
            Game.Exit();
        }
    }
}
