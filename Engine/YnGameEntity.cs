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
        protected bool _visible;

        #region Properties

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

        public YnGameEntity()
            : base()
        {
            _assetLoaded = false;
            _enabled = true;
            _visible = true;
        }

        /// <summary>
        /// Initialize the logic.
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Load assets
        /// </summary>
        /// <param name="content">The content manager.</param>
        public virtual void LoadContent()
        {

        }

        /// <summary>
        /// Unload assets and free memory.
        /// </summary>
        public virtual void UnloadContent()
        {

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
