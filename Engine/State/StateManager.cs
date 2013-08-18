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

        private YnGameEntityCollection _states;
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
                if (index < 0 || index > _states.Count - 1)
                    return null;
                else
                    return _states[index] as YnState;
            }
            set
            {
                if (index < 0 || index > _states.Count - 1)
                    throw new IndexOutOfRangeException();
                else
                    _states[index] = value;
            }
        }

        #endregion

        #region Constructor

        public StateManager(Game game)
            : base(game)
        {
            _clearColor = Color.Black;

            _states = new YnGameEntityCollection();
            _statesDictionary = new Dictionary<string, int>();

            _initialized = false;
            _assetLoaded = false;
        }

        #endregion

        #region GameState pattern

        public override void Initialize()
        {
            base.Initialize();

            if (!_initialized)
            {
                foreach (YnState screen in _states)
                    screen.Initialize();

                _initialized = true;
            }
        }

        protected override void LoadContent()
        {
            if (!_assetLoaded)
            {
                int nbScreens = _states.Count;

                _spriteBatch = new SpriteBatch(GraphicsDevice);

                foreach (YnState screen in _states)
                    screen.LoadContent();

                _assetLoaded = true;
            }
        }

        protected override void UnloadContent()
        {
            if (_assetLoaded && _states.Count > 0)
            {
                foreach (YnState screen in _states)
                    screen.UnloadContent();

                _assetLoaded = false;
            }
        }

        /// <summary>
        /// Update logic of enabled states.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (Enabled)
                _states.Update(gameTime);
        }

        /// <summary>
        /// Draw visible states.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                GraphicsDevice.Clear(_clearColor);
                _states.Draw(gameTime, _spriteBatch);
            }
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
            YnState state = Get(name);

            if (state != null)
                return _states.IndexOf(state);

            return -1;
        }

        /// <summary>
        /// Get the index of the screen
        /// </summary>
        /// <param name="name">State</param>
        /// <returns>State index</returns>
        public int IndexOf(YnState state)
        {
            return _states.IndexOf(state);
        }

        /// <summary>
        /// Replace a state by another state
        /// </summary>
        /// <param name="oldState">Old state in the collection</param>
        /// <param name="newState">New state</param>
        /// <returns>True if for success then false</returns>
        public bool Replace(YnState oldState, YnState newState)
        {
            int index = _states.IndexOf(oldState);

            if (index > -1)
            {
                newState.StateManager = this;
                _states[index] = newState;

                if (_initialized && !newState.Initialized)
                    newState.Initialize();

                if (_assetLoaded && !newState.AssetLoaded)
                    newState.LoadContent();

                return true;
            }

            return false;
        }


        /// <summary>
        /// Active a screen and desactive other screens if needed.
        /// </summary>
        /// <param name="index">Index of the screen in the collection</param>
        /// <param name="desactiveOtherStates">Desactive or not others screens</param>
        public void SetActive(int index, bool desactiveOtherStates)
        {
            int size = _states.Count;

            if (index < 0 || index > size - 1)
                throw new IndexOutOfRangeException("[ScreenManager] The screen doesn't exist at this index");

            _states[index].Active = true;

            if (desactiveOtherStates)
            {
                for (int i = 0; i < size; i++)
                {
                    if (i != index)
                        _states[i].Active = false;
                }
            }
        }

        public void SetActive(string name, bool desactiveOtherScreens)
        {
            if (_statesDictionary.ContainsKey(name))
            {
                YnState activableState = _states[_statesDictionary[name]] as YnState;
                activableState.Active = true;

                if (desactiveOtherScreens)
                {
                    foreach (YnState screen in _states)
                    {
                        if (activableState != screen)
                            screen.Active = false;
                    }
                }
            }
            else
                throw new StateManagerException("This screen name doesn't exists");
        }

        /// <summary>
        /// Gets a state by its name
        /// </summary>
        /// <param name="name">The name used by the state</param>
        /// <returns>The state if exists otherwise return null</returns>
        public YnState Get(string name)
        {
            if (_statesDictionary.ContainsKey(name))
                return _states[_statesDictionary[name]] as YnState;

            return null;
        }

        /// <summary>
        /// Update internal mapping with de Dictionary and the State collection
        /// </summary>
        protected void UpdateDictionaryStates()
        {
            _statesDictionary.Clear();

            foreach (YnState screen in _states)
            {
                if (_statesDictionary.ContainsKey(screen.Name))
                    throw new StateManagerException("Two screens can't have the same name, it's forbiden and it's bad :(");

                _statesDictionary.Add(screen.Name, _states.IndexOf(screen));
            }
        }

        /// <summary>
        /// Sets all state in pause by desabling them.
        /// </summary>
        public void PauseAllStates()
        {
            foreach (YnState state in _states)
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

            if (_initialized)
                state.Initialize();

            if (_assetLoaded)
                state.LoadContent();

            _states.Add(state);
            _statesDictionary.Add(state.Name, _states.IndexOf(state));
        }

        /// <summary>
        /// Add a state to the manager and active or desactive it.
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="active"></param>
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
        /// <param name="screen">Screen to remove</param>
        public void Remove(YnState state)
        {
            _states.Remove(state);
            _statesDictionary.Remove(state.Name);
        }

        /// <summary>
        /// Clear all the Screens in the Manager
        /// </summary>
        public void Clear()
        {
            if (_states.Count > 0)
            {
                for (int i = _states.Count - 1; i >= 0; i--)
                    _states[i].Active = false;

                _states.Clear();
                _statesDictionary.Clear();
            }
        }

        public IEnumerator GetEnumerator()
        {
            foreach (YnState screen in _states)
                yield return screen;
        }

        #endregion
    }
}
