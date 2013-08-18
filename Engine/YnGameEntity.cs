// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Yna.Engine
{
    /// <summary>
    /// Represent an updateable and drawable entity
    /// </summary>
    public abstract class YnGameEntity : YnBasicEntity
    {
        protected bool _assetLoaded;
        protected bool _initialized;
        protected bool _visible;

        #region Properties

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
            set { _assetLoaded = value ;}
        }

        /// <summary>
        /// Active or desactive the Entity. An active Entity will be updated and drawn
        /// </summary>
        public new bool Active
        {
            get { return _visible && _enabled; }
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
        protected virtual void OnActivated(EventArgs e)
        {
            if (Activated != null)
                Activated(this, e);
        }

        /// <summary>
        /// Method called when the entity is desactivated.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDesactivated(EventArgs e)
        {
            if (Desactivated != null)
                Desactivated(this, e);
        }

        #endregion

        /// <summary>
        /// Create game entity that is updateable and drawable.
        /// </summary>
        public YnGameEntity()
            : base()
        {
            _assetLoaded = false;
            _initialized = false;
            _enabled = true;
            _visible = true;
        }

        /// <summary>
        /// Initialize the logic.
        /// </summary>
        public virtual void Initialize()
        {
            _initialized = true;
        }

        /// <summary>
        /// Load assets
        /// </summary>
        /// <param name="content">The content manager.</param>
        public virtual void LoadContent()
        {
            _assetLoaded = true;
        }

        /// <summary>
        /// Unload assets and free memory.
        /// </summary>
        public virtual void UnloadContent()
        {
            _assetLoaded = false;
        }

        /// <summary>
        /// Draw entity on screen.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch">A SpriteBatch to draw this entity.</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }
    }
}
