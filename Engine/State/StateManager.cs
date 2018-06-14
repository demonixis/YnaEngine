// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Yna.Engine.State
{
    /// <summary>
    /// The StateManager is responsible off managing the various screens that composes the game.
    /// A state represents a game screen as a menu, a scene or a score screen. 
    /// The state manager can add, delete, and work with registered states.
    /// </summary>
    public class StateManager : DrawableGameComponent
    {
        #region Private declarations

        private List<YnState> _scenes;
        private List<YnState> _toAdd;
        private List<YnState> _toRemove;
        private string _nextActive;
        private bool _nextDisableOther;
        private Dictionary<string, int> _statesDictionary;

        private bool _initialized;
        private bool _assetLoaded;
        private SpriteBatch _spriteBatch;
        private Color _clearColor;

        #endregion

        #region Properties

        /// <summary>
        /// Get or Set the color used to clear the screen before each frame
        /// </summary>
        public Color ClearColor
        {
            get { return _clearColor; }
            set { _clearColor = value; }
        }

        /// <summary>
        /// Get the SpriteBatch
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        /// <summary>
        /// Get the screen at index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public YnState this[int index]
        {
            get
            {
                if (index < 0 || index > _scenes.Count - 1)
                    return null;
                else
                    return _scenes[index] as YnState;
            }
            set
            {
                if (index < 0 || index > _scenes.Count - 1)
                    throw new IndexOutOfRangeException();
                else
                    _scenes[index] = value;
            }
        }

        #endregion

        #region Constructor

        public StateManager(Game game)
            : base(game)
        {
            _clearColor = Color.Black;

            _scenes = new List<YnState>();
            _toAdd = new List<YnState>();
            _toRemove = new List<YnState>();
            _statesDictionary = new Dictionary<string, int>();

            _initialized = false;
            _assetLoaded = false;

            game.Components.Add(this);
        }

        #endregion

        #region GameState pattern

        public override void Initialize()
        {
            base.Initialize();

            if (_initialized)
                return;

            foreach (var screen in _scenes)
                screen.Initialize();

            _initialized = true;
        }

        protected override void LoadContent()
        {
            if (_assetLoaded)
                return;

            var nbScreens = _scenes.Count;

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (var screen in _scenes)
                screen.LoadContent();

            _assetLoaded = true;
        }

        protected override void UnloadContent()
        {
            if (!_assetLoaded)
                return;

            foreach (var screen in _scenes)
                screen.UnloadContent();

            _spriteBatch.Dispose();
            _assetLoaded = false;
        }

        /// <summary>
        /// Update logic of enabled states.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (!Enabled)
                return;

            if (_toRemove.Count > 0)
            {
                foreach (var state in _toRemove)
                {
                    _scenes.Remove(state);
                    _statesDictionary.Remove(state.Name);
                }

                _toRemove.Clear();
            }

            if (_toAdd.Count > 0)
            {
                foreach (var state in _toAdd)
                {
                    _scenes.Add(state);
                    _statesDictionary.Add(state.Name, _scenes.IndexOf(state));
                }

                _toAdd.Clear();
            }

            if (!string.IsNullOrEmpty(_nextActive))
            {
                var activableState = _scenes[_statesDictionary[_nextActive]];
                activableState.Active = true;

                if (_nextDisableOther)
                {
                    foreach (var screen in _scenes)
                        if (activableState != screen)
                            screen.Active = false;
                }

                _nextActive = string.Empty;
            }

            foreach (var scene in _scenes)
                if (scene.Enabled)
                    scene.Update(gameTime);
        }

        /// <summary>
        /// Draw visible states.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (!Visible)
                return;

            GraphicsDevice.Clear(_clearColor);

            foreach (var scene in _scenes)
                if (scene.Enabled)
                    scene.Draw(gameTime);
        }

        #endregion

        #region State management methods

        /// <summary>
        /// Get the index of the screen
        /// </summary>
        /// <param name="name">State name</param>
        /// <returns>State index</returns>
        public int IndexOf(string name)
        {
            var state = Get(name);

            if (state != null)
                return _scenes.IndexOf(state);

            return -1;
        }

        /// <summary>
        /// Get the index of the screen
        /// </summary>
        /// <param name="scene">State</param>
        /// <returns>State index</returns>
        public int IndexOf(YnState scene) => _scenes.IndexOf(scene);

        /// <summary>
        /// Replace a state by another state
        /// </summary>
        /// <param name="oldState">Old state in the collection</param>
        /// <param name="newState">New state</param>
        /// <returns>True if for success then false</returns>
        public bool Replace(YnState oldState, YnState newState)
        {
            var index = _scenes.IndexOf(oldState);

            if (index < 0)
                return false;

            newState.StateManager = this;
            _scenes[index] = newState;

            if (_assetLoaded && !newState.AssetLoaded)
                newState.LoadContent();

            if (_initialized && !newState.Initialized)
                newState.Initialize();

            return true;
        }

        public void SetActive(string name, bool desactiveOtherScreens)
        {
            _nextActive = name;
            _nextDisableOther = desactiveOtherScreens;
        }

        /// <summary>
        /// Gets a state by its name
        /// </summary>
        /// <param name="name">The name used by the state</param>
        /// <returns>The state if exists otherwise return null</returns>
        public YnState Get(string name)
        {
            if (_statesDictionary.ContainsKey(name))
                return _scenes[_statesDictionary[name]];

            return null;
        }

        /// <summary>
        /// Update internal mapping with de Dictionary and the State collection
        /// </summary>
        protected void UpdateDictionaryStates()
        {
            _statesDictionary.Clear();

            foreach (var screen in _scenes)
            {
                if (_statesDictionary.ContainsKey(screen.Name))
                    throw new Exception("Two screens can't have the same name, it's forbiden and it's bad :(");

                _statesDictionary.Add(screen.Name, _scenes.IndexOf(screen));
            }
        }

        /// <summary>
        /// Sets all state in pause by desabling them.
        /// </summary>
        public void PauseAllStates()
        {
            foreach (var state in _scenes)
                state.Active = false;
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Add a new state to the manager. The screen is not activated or desactivated, you must manage it yourself
        /// </summary>
        /// <param name="state">Screen to add</param>
        public void Add(YnState state)
        {
            state.StateManager = this;

            state.LoadContent();
            state.Initialize();

            _toAdd.Add(state);
        }

        /// <summary>
        /// Add a state to the manager and active or desactive it.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="isActive"></param>
        public void Add(YnState state, bool isActive)
        {
            if (state.Active != isActive)
            {
                state.Enabled = isActive;
                state.Visible = isActive;
            }

            Add(state);
        }

        /// <summary>
        /// Remove a screen to the Manager
        /// </summary>
        /// <param name="state">Screen to remove</param>
        public void Remove(YnState state)
        {
            _toRemove.Add(state);
        }

        /// <summary>
        /// Clear all the Screens in the Manager
        /// </summary>
        public void Clear()
        {
            if (_scenes.Count > 0)
            {
                for (int i = _scenes.Count - 1; i >= 0; i--)
                    _scenes[i].Active = false;

                _scenes.Clear();
                _statesDictionary.Clear();
            }
        }

        public IEnumerator GetEnumerator()
        {
            foreach (YnState screen in _scenes)
                yield return screen;
        }

        #endregion
    }
}
