using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.State
{
    /// <summary>
    /// A basic state used with the state manager
    /// A state represents a game screen as a menu, a scene or a score screen.
    /// </summary>
    public abstract class YnState : YnBase
    {
        #region Private declarations

        private static int ScreenCounter = 0;

        protected bool _reinitializeAfterActivation;
        protected bool _visible;
        protected bool _assetLoaded;
        protected SpriteBatch spriteBatch;
        protected StateManager stateManager;

        #endregion

        #region Properties

        /// <summary>
        /// Sets to true for reinitialize the state after an activation.
        /// </summary>
        public bool ReinitializeAfterActivation
        {
            get { return _reinitializeAfterActivation; }
            set { _reinitializeAfterActivation = value; }
        }

        /// <summary>
        /// Gets or sets the status of the asset loading.
        /// </summary>
        public bool AssetLoaded
        {
            get { return _assetLoaded; }
            set { _assetLoaded = value; }
        }

        /// <summary>
        /// Active or desactive this state. Enable & Visible
        /// </summary>
        public new bool Active
        {
            get { return _enabled && _visible; }
            set
            {
                _enabled = value;
                _visible = value;

                if (value)
                    OnActivated(EventArgs.Empty);
                else
                    OnDesactivated(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the visibility value
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

       
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
        /// Triggerd when the Active property is set to true
        /// </summary>
        public event EventHandler<EventArgs> Activated = null;

        /// <summary>
        /// Triggered when the Active property is set to false
        /// </summary>
        public event EventHandler<EventArgs> Desactivated = null;

        /// <summary>
        /// Triggered when the state has begin to load content.
        /// </summary>
        public event EventHandler<EventArgs> ContentLoadingStarted = null;

        /// <summary>
        /// Triggered when the state has finish to load content.
        /// </summary>
        public event EventHandler<EventArgs> ContentLoadingFinished = null;

        /// <summary>
        /// Called when the state is activated (not when the state is created).
        /// If the property ReinitializeAfterAction is setted to true then the state will
        /// be reinitialized with a call of the Initialize() method.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnActivated(EventArgs e)
        {
            if (Activated != null)
                Activated(this, e);

            if (_reinitializeAfterActivation)
                Initialize();
        }

        /// <summary>
        /// Called when the state is desactivated.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDesactivated(EventArgs e)
        {
            if (Desactivated != null)
                Desactivated(this, e);
        }

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

        /// <summary>
        /// Initialize state logic.
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Load state content.
        /// </summary>
        public virtual void LoadContent()
        {
            spriteBatch = new SpriteBatch(stateManager.Game.GraphicsDevice);
        }

        /// <summary>
        /// Unload state content.
        /// </summary>
        public virtual void UnloadContent()
        {

        }

        /// <summary>
        /// Draw the state on screen.
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Draw(GameTime gameTime);

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