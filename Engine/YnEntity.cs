// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine
{
    /// <summary>
    /// Base class for all object on the Framework. A basic object is updateable
    /// </summary>
    public abstract class YnEntity
    {
        #region private declarations

        private static uint counterId = 0;
        protected uint _id = counterId++;
        protected string _name = "YnBase";
        protected bool _enabled = true;
        protected bool _assetLoaded;
        protected bool _initialized;
        protected bool _visible = true;

        #endregion

        #region Properties

        /// <summary>
        /// Get the unique identification code of this object
        /// </summary>
        public uint Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Get or Set the name of this object
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Pause or resume updates
        /// </summary>
        public virtual bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public bool Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
        }

        /// <summary>
        /// Gets or sets asset loaded. Sets to true to force LoadContent to reload an asset.
        /// </summary>
        public bool AssetLoaded
        {
            get { return _assetLoaded; }
            set { _assetLoaded = value; }
        }

        /// <summary>
        /// Active or desactive the Entity. An active Entity will be updated and drawn
        /// </summary>
        public virtual bool Active
        {
            get => _visible && _enabled;
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

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        #endregion

        #region Events declaration

        /// <summary>
        /// Triggered when the entity is set to Active = true
        /// </summary>
        public event EventHandler<EventArgs> Activated = null;

        /// <summary>
        /// Triggered when the entity is set to Active = false
        /// </summary>
        public event EventHandler<EventArgs> Desactivated = null;

        /// <summary>
        /// Method called when the entity is activated.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnActivated(EventArgs e) => Activated?.Invoke(this, e);

        /// <summary>
        /// Method called when the entity is desactivated.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDesactivated(EventArgs e) => Desactivated?.Invoke(this, e);

        #endregion

        /// <summary>
        /// Initialize the logic.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Load assets
        /// </summary>
        public virtual void LoadContent() { }

        /// <summary>
        /// Unload assets and free memory.
        /// </summary>
        public virtual void UnloadContent() { }

        /// <summary>
        /// Update method called on each engine update
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draw entity on screen.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch">A SpriteBatch to draw this entity.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }
    }
}
