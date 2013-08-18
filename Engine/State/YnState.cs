// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.State
{
    /// <summary>
    /// A basic state used with the state manager
    /// A state represents a game screen as a menu, a scene or a score screen.
    /// </summary>
    public abstract class YnState : YnGameEntity
    {
        #region Private declarations

        private static int ScreenCounter = 0;
        protected bool _reinitializeAfterActivation;
        protected SpriteBatch spriteBatch;
        protected StateManager stateManager;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Screen Manager
        /// </summary>
        public StateManager StateManager
        {
            get { return stateManager; }
            set
            {
                stateManager = value;
                spriteBatch = value.SpriteBatch;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Triggered when the state has begin to load content.
        /// </summary>
        public event EventHandler<EventArgs> ContentLoadingStarted = null;

        /// <summary>
        /// Triggered when the state has finish to load content.
        /// </summary>
        public event EventHandler<EventArgs> ContentLoadingFinished = null;

        /// <summary>
        /// Called when the state has begin to load content.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnContentLoadingStarted(EventArgs e)
        {
            if (ContentLoadingStarted != null)
                ContentLoadingStarted(this, e);
        }

        /// <summary>
        /// Called when the state has finish to load content.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnContentLoadingFinished(EventArgs e)
        {
            if (ContentLoadingFinished != null)
                ContentLoadingFinished(this, e);
        }

        #endregion

        #region Constructors

        public YnState()
            : base()
        {
            _name = "State_" + (ScreenCounter++);
            _reinitializeAfterActivation = true;
            _enabled = true;
            _visible = true;
            _assetLoaded = false;
        }

        public YnState(string name)
            : this()
        {
            _name = name;
        }

        #endregion

        public override void Initialize()
        {
            base.Initialize();
            if (!_initialized)
                _initialized = true;
        }

        /// <summary>
        /// Load state content.
        /// </summary>
        public override void LoadContent()
        {
            if (!_assetLoaded)
            {
                spriteBatch = new SpriteBatch(stateManager.Game.GraphicsDevice);
                _assetLoaded = true;

            }
        }

        /// <summary>
        /// Unload state content.
        /// </summary>
        public override void UnloadContent()
        {
            if (_assetLoaded)
            {
                spriteBatch.Dispose();
                _assetLoaded = false;
            }
        }

        /// <summary>
        /// Draw the state on screen.
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Draw(GameTime gameTime);

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Draw(gameTime);
        }

        /// <summary>
        /// Quit the state and remove it from the ScreenManager
        /// </summary>
        public virtual void Kill()
        {
            UnloadContent();
            stateManager.Remove(this);
        }
    }
}