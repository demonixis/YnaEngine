// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Yna.Engine.State;
using Yna.Engine.Graphics.Scene;
using System;

namespace Yna.Engine.Graphics
{
    /// <summary>
    /// A configuration structure used when rendering scene with sprite batch.
    /// </summary>
    public struct SpriteBatchConfiguration
    {
        public SpriteSortMode SpriteSortMode;
        public BlendState BlendState;
        public SamplerState SamplerState;
        public DepthStencilState DepthStencilState;
        public RasterizerState RasterizerState;
        public Effect Effect;
    }

    /// <summary>
    /// This is a State object that contains the scene.
    /// That allows you to add different types of objects.
    /// Timers, basic objects (which have an update method) and entities
    /// </summary>
    public class YnState2D : YnState
    {
        protected YnScene _scene;
        protected SpriteBatchConfiguration _spriteBatchConfig;
        protected YnCamera2D _camera;

        #region Properties

        /// <summary>
        /// Gets basic objects
        /// </summary>
        public List<YnBasicEntity> BasicEntities
        {
            get { return _scene.BaseObjects; }
        }

        /// <summary>
        /// Gets members attached to the scene
        /// </summary>
        public List<YnGameEntity> GameEntities
        {
            get { return _scene.Entities; }
        }

        /// <summary>
        /// Gets or sets the configuration used for sprite batch.
        /// </summary>
        public SpriteBatchConfiguration SpriteBatchConfiguration
        {
            get { return _spriteBatchConfig; }
            set { _spriteBatchConfig = value; }
        }

        /// <summary>
        /// Gets or sets the spriteBatchCamera used for add effect on the scene like 
        /// displacement, rotation and zoom
        /// </summary>
        public YnCamera2D Camera
        {
            get { return _camera; }
            set { _camera = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a 2D state.
        /// </summary>
        /// <param name="name">The state name</param>
        /// <param name="active">Set to true to activate the state</param>
        /// <param name="enableGui">Set to true tu enable GUI on this state</param>
        public YnState2D(string name, bool active, bool enableGui)
            : base(name)
        {
            _enabled = active;
            _visible = active;

            InitializeDefaultState();

            _scene = new YnScene();
        }

        /// <summary>
        /// Create a 2D state without GUI.
        /// </summary>
        /// <param name="name">The state name</param>
        /// <param name="active">Set to true to activate the state</param>
        public YnState2D(string name, bool active)
            : this(name, active, false)
        {
        }

        /// <summary>
        ///  Create a 2D state without GUI.
        /// </summary>
        /// <param name="name">The state name</param>
        public YnState2D(string name)
            : this(name, true, false)
        {
        }

        #endregion

        /// <summary>
        /// Initialize defaut states
        /// </summary>
        private void InitializeDefaultState()
        {
            _spriteBatchConfig = new SpriteBatchConfiguration()
            {
                SpriteSortMode = SpriteSortMode.Deferred,
                BlendState = BlendState.AlphaBlend,
                SamplerState = SamplerState.LinearClamp,
                DepthStencilState = DepthStencilState.Default,
                RasterizerState = RasterizerState.CullNone,
                Effect = null
            };

            _camera = new YnCamera2D();
        }

        #region GameState pattern

        /// <summary>
        /// Initialize the state
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            if (!_initialized)
                _scene.Initialize();
        }

        /// <summary>
        /// Load content
        /// </summary>
        public override void LoadContent()
        {
            if (!_assetLoaded)
            {
                OnContentLoadingStarted(EventArgs.Empty);

                base.LoadContent();
                _scene.LoadContent();

                OnContentLoadingFinished(EventArgs.Empty);

                _assetLoaded = true;
            }
        }

        /// <summary>
        /// Unload content
        /// </summary>
        public override void UnloadContent()
        {
            if (_assetLoaded)
            {
                _scene.UnloadContent();
                _scene.Clear();
                _assetLoaded = false;
            }
        }

        /// <summary>
        /// Update the camera and the scene who will update BasicObjects, Entities and Gui
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            _camera.Update(gameTime);
            _scene.Update(gameTime);
        }

        /// <summary>
        /// Draw all entities and the gui
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            int nbMembers = _scene.Entities.Count;

            if (nbMembers > 0)
            {
                spriteBatch.Begin(
                    _spriteBatchConfig.SpriteSortMode,
                    _spriteBatchConfig.BlendState,
                    _spriteBatchConfig.SamplerState,
                    _spriteBatchConfig.DepthStencilState,
                    _spriteBatchConfig.RasterizerState,
                    _spriteBatchConfig.Effect,
                    _camera.GetTransformMatrix());
                _scene.Draw(gameTime, spriteBatch);
                spriteBatch.End();
            }
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Add a basic object to the scene
        /// </summary>
        /// <param name="basicObject">A basic object</param>
        public void Add(YnBasicEntity basicObject)
        {
            _scene.Add(basicObject);
        }

        /// <summary>
        /// Add an entity to the scene
        /// </summary>
        /// <param name="entity">An entitiy</param>
        public void Add(YnEntity entity)
        {
            if (Initialized && !entity.Initialized)
                entity.Initialize();

            if (AssetLoaded && !entity.AssetLoaded)
                    entity.LoadContent();
       
            _scene.Add(entity);
        }

        /// <summary>
        /// Remove a basic object to the scene
        /// </summary>
        /// <param name="basicObject">A basic object</param>
        public void Remove(YnBasicEntity basicObject)
        {
            _scene.Remove(basicObject);
        }

        /// <summary>
        /// Remove an entity to the scene
        /// </summary>
        /// <param name="entity">An entitiy</param>
        public void Remove(YnEntity entity)
        {
            _scene.Remove(entity);
        }

        public YnBasicEntity GetMemberByName(string name)
        {
            return _scene.GetMemberByName(name);
        }

        #endregion
    }
}
