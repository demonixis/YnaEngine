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
using Yna.Engine.Graphics;

namespace Yna.Engine
{
    /// <summary>
    /// The game class
    /// </summary>
    public class YnGame : Game
    {
        public static string GameTitle = "Yna Game";
        public static string GameVersion = "1.0.0.0";
        protected GraphicsDeviceManager _graphicsDevice = null;
        protected SpriteBatch _spriteBatch;
        protected StateManager _stateManager;
        private Texture2D _debugOverlay;
        private SpriteFont _debugSpriteFont;
        private bool _debugEnabled;

        #region Constructors

        /// <summary>
        /// Create and setup the game engine
        /// Graphics, Services and helpers are initialized
        /// </summary>
        public YnGame()
            : base()
        {
            Content.RootDirectory = "Content";
            
            _graphicsDevice = new GraphicsDeviceManager(this);
            _graphicsDevice.GraphicsProfile = GraphicsProfile.HiDef;

            _stateManager = new StateManager(this);

            // Registry globals objects
            YnG.Game = this;
            YnG.GraphicsDeviceManager = _graphicsDevice;
            YnG.Keys = new YnKeyboard(this); ;
            YnG.Mouse = new YnMouse(this); ;
            YnG.Gamepad = new YnGamepad(this); ;
            YnG.Touch = new YnTouch(this); ;
            YnG.StateManager = _stateManager;
            YnG.StorageManager = new StorageManager(this);
            YnG.AudioManager = new AudioManager();
            ScreenHelper.ScreenWidthReference = _graphicsDevice.PreferredBackBufferWidth;
            ScreenHelper.ScreenHeightReference = _graphicsDevice.PreferredBackBufferHeight;
        }

        #endregion

        #region GameState pattern

        /// <summary>
        /// Load assets from content manager
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GraphicsDevice.Viewport = new Viewport(0, 0, _graphicsDevice.PreferredBackBufferWidth, _graphicsDevice.PreferredBackBufferHeight);
            Window.Title = String.Format("{0} - v{1}", GameTitle, GameVersion);
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

            if (!_debugEnabled)
                return;
            
            var metrics = GraphicsDevice.Metrics;
            var strings = new string[]
            {
                $"ClearCount: {metrics.ClearCount}",
                $"DrawCount: {metrics.DrawCount}",
                $"SpriteCount: {metrics.SpriteCount}",
                $"TargetCount: {metrics.TargetCount}",
                $"TextureCount: {metrics.TextureCount}",
                $"PrimitiveCount: {metrics.PrimitiveCount}",
                $"VertexShaderCount: {metrics.VertexShaderCount}",
                $"PixelShaderCount: {metrics.PixelShaderCount}"
            };

            var longer = 0;
            var index = 0;

            for (var i = 0; i < strings.Length; i++)
            {
                if (strings[i].Length > longer)
                {
                    longer = strings[i].Length;
                    index = i;
                }
            }

            var size = _debugSpriteFont.MeasureString(strings[index]);
            var offset = size.Y;

            _spriteBatch.Begin();

            _spriteBatch.Draw(_debugOverlay, new Rectangle(0, 0, (int)(5 + size.X), (int)(strings.Length * offset + 5)), Color.White);

            for (var i = 0; i < strings.Length; i++)
                _spriteBatch.DrawString(_debugSpriteFont, strings[i], new Vector2(5, 5 + (offset * i)), Color.LightGreen);
                                        
            _spriteBatch.End();
		}

		#endregion

        public void SetDebugLayerEnabled(SpriteFont font)
        {
            _debugSpriteFont = font;
            _debugEnabled = font != null;

            if (_debugOverlay != null)
                return;

            var color = Color.Black;
            color.A = 125;

            _debugOverlay = YnGraphics.CreateTexture(color, 8, 8);
        }

		#region Resolution setup

		/// <summary>
		/// Change the screen resolution
		/// </summary>
		/// <param name="width">Screen width</param>
		/// <param name="height">Screen height</param>
		public virtual void SetScreenResolution(int width, int height)
        {
            this._graphicsDevice.PreferredBackBufferWidth = width;
            this._graphicsDevice.PreferredBackBufferHeight = height;
            this._graphicsDevice.ApplyChanges();
        }

        /// <summary>
        /// Set maximum resolution supported by the device, It use the desktop resolution
        /// </summary>
        /// <param name="fullscreen">Toggle in fullscreen mode</param>
        public virtual void DetermineBestResolution(bool fullscreen)
        {
            SetScreenResolution(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            
            if (!_graphicsDevice.IsFullScreen && fullscreen)
                _graphicsDevice.ToggleFullScreen();
        }

        #endregion
    }
}

