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
    public abstract class BaseState : YnBase
    {
        #region Private declarations

        private static int ScreenCounter = 0;

        private bool _visible;
        private bool _initialized;
        private bool _assetsLoaded;

        protected SpriteBatch spriteBatch;
        protected StateManager stateManager;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether the object is initialized
        /// </summary>
        public bool Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
        }

        /// <summary>
        /// Active or desactive this state. Enable & Visible
        /// </summary>
        public bool Active
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
        /// Get or set the visibility value
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

       
        /// <summary>
        /// Get or Set the Screen Manager
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

        /// <summary>
        /// Define the clear color before each draw calls
        /// </summary>
        public Color ClearColor
        {
            get { return stateManager.ClearColor; }
            set { stateManager.ClearColor = value; }
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

        protected virtual void OnActivated(EventArgs e)
        {
            if (Activated != null)
                Activated(this, e);
        }

        protected virtual void OnDesactivated(EventArgs e)
        {
            if (Desactivated != null)
                Desactivated(this, e);
        }

        #endregion

        #region Constructors

        public BaseState()
        {
            _name = "State_" + (ScreenCounter++);
            _enabled = true;
            _visible = true;
        }

        public BaseState(string name)
            : this()
        {
            _name = name;
        }

        #endregion

        public virtual void Initialize()
        {
            _initialized = true;
        }

        public virtual void LoadContent()
        {
            spriteBatch = new SpriteBatch(stateManager.Game.GraphicsDevice); 
        }

        public abstract void UnloadContent();


        public abstract void Draw(GameTime gameTime);

        /// <summary>
        /// Quit the screen and remove it from the ScreenManager
        /// </summary>
        public virtual void Exit()
        {
            UnloadContent();
            stateManager.Remove(this);
        }
    }
}