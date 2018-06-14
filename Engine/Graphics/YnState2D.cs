// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Yna.Engine.Graphics.Gui;
using Yna.Engine.Graphics.Gui.Widgets;
using Yna.Engine.State;

namespace Yna.Engine.Graphics
{
    /// <summary>
    /// This is a State object that contains the scene.
    /// That allows you to add different types of objects.
    /// Timers, basic objects (which have an update method) and entities
    /// </summary>
    public class YnState2D : YnState
    {
        protected List<YnEntity> _entities;
        protected YnGui _guiManager;
        protected YnCamera2D _camera;
        protected bool _GUIEnabled = false;

        /// <summary>
        /// Gets or sets the gui
        /// </summary>
        public YnGui Gui
        {
            get { return _guiManager; }
            protected set { _guiManager = value; }
        }

        #region Properties

        /// <summary>
        /// Gets members attached to the scene
        /// </summary>
        public List<YnEntity> GameEntities => _entities;

        /// <summary>
        /// Gets or sets the spriteBatchCamera used for add effect on the scene like 
        /// displacement, rotation and zoom
        /// </summary>
        public YnCamera2D Camera
        {
            get { return _camera; }
            set { _camera = value; }
        }

        public bool GUIEnabled
        {
            get => _GUIEnabled;
            set
            {
                _GUIEnabled = value;

                if (_guiManager == null)
                {
                    _guiManager = new YnGui();

                    if (_assetLoaded)
                        _guiManager.LoadContent();

                    if (_initialized)
                        _guiManager.Initialize();
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a 2D state.
        /// </summary>
        /// <param name="name">The state name</param>
        /// <param name="active">Set to true to activate the state</param>
        /// <param name="enableGui">Set to true tu enable GUI on this state</param>
        public YnState2D(string name, bool active = true, bool enableGui = false)
            : base(name)
        {
            _enabled = active;
            _visible = active;
            _camera = new YnCamera2D();
            _entities = new List<YnEntity>();
            _GUIEnabled = enableGui;
        }

        #endregion

        #region GameState pattern

        /// <summary>
        /// Initialize the state
        /// </summary>
        public override void Initialize()
        {
            if (_initialized)
                return;

            foreach (var entity in _entities)
                entity.Initialize();

            if (_GUIEnabled)
                _guiManager.Initialize();
        }

        /// <summary>
        /// Load content
        /// </summary>
        public override void LoadContent()
        {
            if (_assetLoaded)
                return;

            base.LoadContent();

            foreach (var entity in _entities)
                entity.LoadContent();

            if (_GUIEnabled)
                _guiManager.LoadContent();

            _assetLoaded = true;
        }

        /// <summary>
        /// Unload content
        /// </summary>
        public override void UnloadContent()
        {
            if (!_assetLoaded)
                return;

            foreach (var entity in _entities)
                entity.UnloadContent();

            if (_GUIEnabled)
                _guiManager.UnloadContent();

            _entities.Clear();
        }

        /// <summary>
        /// Update the camera and the scene who will update BasicObjects, Entities and Gui
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            _camera.Update(gameTime);

            foreach (var entity in _entities)
                entity.Update(gameTime);

            if (_GUIEnabled)
                _guiManager.Update(gameTime);
        }

        /// <summary>
        /// Draw all entities and the gui
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            foreach (var entity in _entities)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _camera.GetTransformMatrix());
                entity.Draw(gameTime, spriteBatch);
                spriteBatch.End();
            }

            if (_GUIEnabled)
                _guiManager.Draw(gameTime, spriteBatch);
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Add a basic object to the scene
        /// </summary>
        /// <param name="entity">A basic object</param>
        public void Add(YnEntity entity)
        {
            if (_entities.Contains(entity))
                return;

            if (Initialized && !entity.Initialized)
                entity.Initialize();

            if (AssetLoaded && !entity.AssetLoaded)
                entity.LoadContent();

            _entities.Add(entity);
        }

        public void Add(YnWidget widget) => _guiManager.Add(widget);

        public void Remove(YnWidget widget) => _guiManager.Remove(widget);

        /// <summary>
        /// Remove a basic object to the scene
        /// </summary>
        /// <param name="basicObject">A basic object</param>
        public void Remove(YnEntity basicObject) => _entities.Remove(basicObject);

        public YnEntity2D GetMemberByName(string name)
        {
            foreach (var entity in _entities)
                if (entity is YnEntity2D && entity.Name == name)
                    return (YnEntity2D)entity;

            return null;
        }

        #endregion
    }
}
