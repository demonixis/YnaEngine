using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using Yna.Engine.Content;
using Yna.Engine.Graphics;

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

        private List<BaseState> _states;
        private List<BaseState> _safeScreens;
        private Dictionary<string, int> _statesDictionary;
        
        private bool _initialized;
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
        public BaseState this[int index]
        {
            get
            {
                if (index < 0 || index > _states.Count - 1)
                    return null;
                else
                    return _states[index];
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

            _states = new List<BaseState>();
            _safeScreens = new List<BaseState>();
            _statesDictionary = new Dictionary<string, int>();

            _initialized = false;
        }

        #endregion

        #region GameState pattern

        protected override void LoadContent()
        {
            if (!_initialized)
            {
                int nbScreens = _states.Count;

                _spriteBatch = new SpriteBatch(GraphicsDevice);

                foreach (BaseState screen in _states)
                {
                    screen.LoadContent();
                    screen.Initialize();
                }

                _initialized = true;
            }
        }

        protected override void UnloadContent()
        {
            if (_initialized && _states.Count > 0)
            {
                foreach (BaseState screen in _states)
                    screen.UnloadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            // We make a copy of all screens to provide any error
            // if a screen is removed during the update opreation
            int nbScreens = _states.Count;

            if (nbScreens > 0)
            {
                _safeScreens.Clear();
                _safeScreens.AddRange(_states);

                int nbSafeScreen = _safeScreens.Count;

                for (int i = 0; i < nbSafeScreen; i++)
                {
                    if (_safeScreens[i].Active)
                        _safeScreens[i].Update(gameTime);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_clearColor);

            // We make a copy of all screens to provide any error
            // if a screen is removed during the update opreation
            int nbScreens = _safeScreens.Count;

            if (nbScreens > 0)
            {
                for (int i = 0; i < nbScreens; i++)
                {
                    if (_safeScreens[i].Active)
                        _safeScreens[i].Draw(gameTime);
                }
            }
        }

        #endregion

        #region Screens methods

        /// <summary>
        /// Get the index of the screen
        /// </summary>
        /// <param name="name">State name</param>
        /// <returns>State index</returns>
        public int IndexOf(string name)
        {
            BaseState state = Get(name);

            if (state != null)
                return _states.IndexOf(state);

            return -1;
        }

        /// <summary>
        /// Get the index of the screen
        /// </summary>
        /// <param name="name">State</param>
        /// <returns>State index</returns>
        public int IndexOf(BaseState state)
        {
            return _states.IndexOf(state);
        }

        /// <summary>
        /// Replace a state by another state
        /// </summary>
        /// <param name="oldState">Old state in the collection</param>
        /// <param name="newState">New state</param>
        /// <returns>True if for success then false</returns>
        public bool Replace(BaseState oldState, BaseState newState)
        {
            int index = _states.IndexOf(oldState);

            if (index > -1)
            {
                newState.StateManager = this;
                _states[index] = newState;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Active a screen and desactive other screens on demand
        /// </summary>
        /// <param name="index">Index of the screen in the collection</param>
        /// <param name="desactiveOtherScreens">Desactive or not others screens</param>
        public void SetStateActive(int index, bool desactiveOtherScreens)
        {
            int size = _states.Count;

            if (index < 0 || index > size - 1)
                throw new IndexOutOfRangeException("[ScreenManager] The screen doesn't exist at this index");

            _states[index].Active = true;

            if (!_states[index].Initialized)
                _states[index].Initialize();

            if (desactiveOtherScreens)
            {
                for (int i = 0; i < size; i++)
                {
                    if (i != index)
                        _states[i].Active = false; // TODO : Replace by hide when it's ok
                }
            }
        }

        public void SetStateActive(string name, bool desactiveOtherScreens)
        {
            if (_statesDictionary.ContainsKey(name))
            {
                BaseState activableState = _states[_statesDictionary[name]];
                activableState.Active = true;

                if (!activableState.Initialized)
                    activableState.Initialize();

                if (desactiveOtherScreens)
                {
                    foreach (BaseState screen in _states)
                    {
                        if (activableState != screen)
                            screen.Active = false;
                    }
                }
            }
            else
                throw new Exception("[ScreenManager] This screen name doesn't exists");
        }

        /// <summary>
        /// Get desactivated states
        /// </summary>
        /// <returns>An array of index</returns>
        public int[] GetDisabledStatesIndex()
        {
            List<int> indexs = new List<int>();

            int size = _states.Count;

            for (int i = 0; i < size; i++)
            {
                if (!_states[i].Active)
                    indexs.Add(i);
            }

            return indexs.ToArray();
        }

        /// <summary>
        /// Gets a state by its name
        /// </summary>
        /// <param name="name">The name used by the state</param>
        /// <returns>The state if exists otherwise return null</returns>
        public BaseState Get(string name)
        {
            if (_statesDictionary.ContainsKey(name))
                return _states[_statesDictionary[name]];

            return null;
        }

        /// <summary>
        /// Update internal mapping with de Dictionary and the State collection
        /// </summary>
        protected void UpdateDictionaryStates()
        {
            _statesDictionary.Clear();

            foreach (BaseState screen in _states)
            {
                if (_statesDictionary.ContainsKey(screen.Name))
                    throw new Exception("[ScreenManager] Two screens can't have the same name, it's forbiden and it's bad :(");

                _statesDictionary.Add(screen.Name, _states.IndexOf(screen));
            }
        }

        /// <summary>
        /// Do all state unactive
        /// </summary>
        public void PauseAllStates()
        {
            foreach (BaseState state in _states)
                state.Active = false;
        }

        /// <summary>
        /// Switch to a new state, just pass a new instance of a state and 
        /// the StateManager will clear all other states and use the new state
        /// </summary>
        /// <param name="state">New state</param>
        /// <returns>True if the state manager has done the swith, false if it disabled</returns>
        public void SwitchState(BaseState nextState)
        {
            Clear();
            Add(nextState);
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Add a new screen to the Manager. The screen is not activate or desactivate, you must manage this yourself
        /// </summary>
        /// <param name="state">Screen to add</param>
        public void Add(BaseState state)
        {
            state.StateManager = this;

            // If the manager is not yet ready we don't need to initialize and load its content
            // Because it's donne in the init. process
            if (_initialized)
            {
                state.LoadContent();
                state.Initialize();
            }

            _states.Add(state);

            _statesDictionary.Add(state.Name, _states.IndexOf(state));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="active"></param>
        public void Add(BaseState screen, bool active)
        {
            screen.Active = active;
            Add(screen);
        }

        public void Add(BaseState[] screens)
        {
            foreach (BaseState screen in screens)
                Add(screen);
        }

        public void Add(BaseState[] screens, bool active)
        {
            foreach (BaseState screen in screens)
            {
                screen.Active = active;
                Add(screen);
            }
        }

        /// <summary>
        /// Remove a screen to the Manager
        /// </summary>
        /// <param name="screen">Screen to remove</param>
        public void Remove(BaseState screen)
        {
            _states.Remove(screen);

            _statesDictionary.Remove(screen.Name);
            // TODO : Not necessary
            if (_initialized)
                screen.UnloadContent();
        }

        /// <summary>
        /// Clear all the Screens in the Manager
        /// </summary>
        public void Clear()
        {
            if (_states.Count > 0)
            {
                for (int i = 0; i < _states.Count; i++)
                    _states[i].Exit();
            }
        }

        public void Clear(bool force)
        {
            Clear();
            _states.Clear();
            _safeScreens.Clear();
            _statesDictionary.Clear();
        }

        /// <summary>
        /// Get Screen at a position
        /// </summary>
        /// <param name="index">position</param>
        /// <returns>The screen at the position</returns>
        public BaseState GetAt(int index)
        {
            return _states[index];
        }

        /// <summary>
        /// Get alls screens
        /// </summary>
        /// <returns></returns>
        public BaseState[] GetScreens()
        {
            return _states.ToArray();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (BaseState screen in _states)
                yield return screen;
        }

        #endregion
    }
}
