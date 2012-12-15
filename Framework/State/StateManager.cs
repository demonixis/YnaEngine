using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using Yna.Framework.Event;
using Yna.Framework.Display;

namespace Yna.Framework.State
{
    public class StateManager : DrawableGameComponent
    {
        #region Private declarations

        private List<BaseState> _screens;
        private Dictionary<string, int> _namedScreens;
        private List<BaseState> _safeScreens;
        private bool _initialized;

        private Texture2D _transitionTexture;
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
                if (index < 0 || index > _screens.Count - 1)
                    return null;
                else
                    return _screens[index];
            }
            set
            {
                if (index < 0 || index > _screens.Count - 1)
                    throw new IndexOutOfRangeException();
                else
                    _screens[index] = value;
            }
        }

        /// <summary>
        /// Get the first active screen
        /// </summary>
        public BaseState FirstActiveScreen
        {
            get
            {
                int firstIndex = GetFirstActiveScreenIndex();
                return firstIndex > -1 ? _screens[firstIndex] : null;
            }
        }

        /// <summary>
        /// Get the last active screen
        /// </summary>
        public BaseState LastActiveScreen
        {
            get
            {
                int lastIndex = GetLastActiveScreenIndex();
                return lastIndex > -1 ? _screens[lastIndex] : null;
            }
        }

        #endregion

        #region events

        public event EventHandler<ContentLoadStartedEventArgs> LoadContentStarted = null;
        public event EventHandler<ContentLoadDoneEventArgs> LoadContentDone = null;

        protected void OnContentLoadStarted(ContentLoadStartedEventArgs e)
        {
            if (LoadContentStarted != null)
                LoadContentStarted(this, e);
        }

        protected void OnContentLoadDone(ContentLoadDoneEventArgs e)
        {
            if (LoadContentDone != null)
                LoadContentDone(this, e);
        }

        #endregion

        #region Constructor

        public StateManager(Game game)
            : base(game)
        {
            _clearColor = Color.Black;

            _screens = new List<BaseState>();
            _safeScreens = new List<BaseState>();
            _namedScreens = new Dictionary<string, int>();

            _initialized = false;
        }

        #endregion

        #region GameState pattern

        protected override void LoadContent()
        {
            if (!_initialized)
            {
                int nbScreens = _screens.Count;
                TimeSpan start = new TimeSpan();

                OnContentLoadStarted(new ContentLoadStartedEventArgs(nbScreens));

                _spriteBatch = new SpriteBatch(GraphicsDevice);
                // La texture sera étirée
                _transitionTexture = YnGraphics.CreateTexture(Color.White, 16, 16);

                foreach (BaseState screen in _screens)
                {
                    screen.LoadContent();
                    screen.Initialize();
                }

                OnContentLoadDone(new ContentLoadDoneEventArgs(new TimeSpan() - start, nbScreens));

                _initialized = true;
            }
        }

        protected override void UnloadContent()
        {
            if (_initialized && _screens.Count > 0)
            {
                foreach (BaseState screen in _screens)
                    screen.UnloadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            // We make a copy of all screens to provide any error
            // if a screen is removed during the update opreation
            int nbScreens = _screens.Count;

            if (nbScreens > 0)
            {
                _safeScreens.Clear();
                _safeScreens.AddRange(_screens);

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

        #region Helpers for fadein/out the screen

        public void FadeBackBufferToBlack(float alpha)
        {
            FadeBackBuffer(Color.Black, alpha);
        }

        /// <summary>
        /// Fade the screen to the specified color
        /// </summary>
        /// <param name="color">Transition color</param>
        /// <param name="alpha">Alpha color</param>
        public void FadeBackBuffer(Color color, float alpha)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_transitionTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), color * alpha);
            _spriteBatch.End();
        }

        #endregion

        #region Screens methods

        /// <summary>
        /// Get index of actived screens
        /// </summary>
        public int[] GetActiveScreensIndex()
        {
            int size = _screens.Count;

            if (size == 0)
            {
                int[] index = new int[0];
                index[0] = -1;
                return index;
            }

            List<int> indexs = new List<int>();

            for (int i = 0; i < size; i++)
            {
                if (_screens[i].Active)
                    indexs.Add(i);
            }

            return indexs.ToArray();
        }

        /// <summary>
        /// Active a screen and desactive other screens on demand
        /// </summary>
        /// <param name="index">Index of the screen in the collection</param>
        /// <param name="desactiveOtherScreens">Desactive or not others screens</param>
        public void SetScreenActive(int index, bool desactiveOtherScreens)
        {
            int size = _screens.Count;

            if (index < 0 || index > size - 1)
                throw new IndexOutOfRangeException("[ScreenManager] The screen doesn't exist at this index");

            _screens[index].Active = true;

            if (!_screens[index].Initialized)
                _screens[index].Initialize();

            if (desactiveOtherScreens)
            {
                for (int i = 0; i < size; i++)
                {
                    if (i != index)
                        _screens[i].Active = false; // TODO : Replace by hide when it's ok
                }
            }
        }

        public void SetScreenActive(string name, bool desactiveOtherScreens)
        {
            if (_namedScreens.ContainsKey(name))
            {
                BaseState activableScreen = _screens[_namedScreens[name]];
                activableScreen.Active = true;

                if (!activableScreen.Initialized)
                    activableScreen.Initialize();

                // Rebuild the screen GUI
                activableScreen.BuildGui();

                if (desactiveOtherScreens)
                {
                    foreach (BaseState screen in _screens)
                    {
                        if (activableScreen != screen)
                            screen.Hide();  // TODO : Replace by hide when it's ok
                    }
                }
            }
            else
                throw new Exception("[ScreenManager] This screen name doesn't exists");
        }

        /// <summary>
        /// Hide all popup
        /// </summary>
        public void HideAllPopup()
        {
            foreach (BaseState screen in _screens)
            {
                if (screen.IsPopup)
                    screen.Hide();
            }
        }

        /// <summary>
        /// Get desactivated screens
        /// </summary>
        /// <returns></returns>
        public int[] GetDesactivedScreenIndex()
        {
            List<int> indexs = new List<int>();

            int size = _screens.Count;

            for (int i = 0; i < size; i++)
            {
                if (!_screens[i].Active)
                    indexs.Add(i);
            }

            return indexs.ToArray();
        }

        /// <summary>
        /// Get index of the first activated screen
        /// </summary>
        /// <returns>Index of the first activated screen, -1 if no screen is actived</returns>
        public int GetFirstActiveScreenIndex()
        {
            int[] activeIndex = GetActiveScreensIndex();

            if (activeIndex.Length > 0)
                return GetActiveScreensIndex()[0];
            else
                return -1;
        }

        /// <summary>
        /// Get index of the last activated screen
        /// </summary>
        /// <returns>Index of the last activated screen, -1 if no screen actived</returns>
        public int GetLastActiveScreenIndex()
        {
            int[] activeIndex = GetActiveScreensIndex();
            int size = activeIndex.Length;

            if (size > 0 && size < 1)
                return activeIndex[size - 1];
            else if (size > 0)
                return activeIndex[0];
            else
                return -1;
        }

        public BaseState GetScreenByName(string name)
        {
            if (_namedScreens.ContainsKey(name))
                return _screens[_namedScreens[name]];

            return null;
        }

        protected void UpdateScreenNamedScreens()
        {
            _namedScreens.Clear();

            foreach (BaseState screen in _screens)
            {
                if (_namedScreens.ContainsKey(screen.Name))
                    throw new Exception("[ScreenManager] Two screens can't have the same name, it's forbiden and it's bad :(");

                _namedScreens.Add(screen.Name, _screens.IndexOf(screen));
            }
        }

        /// <summary>
        /// Switch to a new state, just pass a new instance of a state and 
        /// the StateManager will clear all other states and use the new state
        /// </summary>
        /// <param name="state">New state</param>
        /// <returns>True if the state manager has done the swith, false if it disabled</returns>
        public void SwitchState(BaseState nextState)
        {
            if (!nextState.IsPopup)
                Clear();

            Add(nextState);
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Add a new screen to the Manager. The screen is not activate or desactivate, you must manage this yourself
        /// </summary>
        /// <param name="screen">Screen to add</param>
        public void Add(BaseState screen)
        {
            screen.ScreenManager = this;

            // If the manager is not yet ready we don't need to initialize and load its content
            // Because it's donne in the init. process
            if (_initialized)
            {
                screen.LoadContent();

                if (screen.Active)
                    screen.Initialize();
            }

            _screens.Add(screen);

            _namedScreens.Add(screen.Name, _screens.IndexOf(screen));
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
            _screens.Remove(screen);

            _namedScreens.Remove(screen.Name);

            if (_initialized)
                screen.UnloadContent();
        }

        /// <summary>
        /// Clear all the Screens in the Manager
        /// </summary>
        public void Clear()
        {
            if (_screens.Count > 0)
            {
                for (int i = 0; i < _screens.Count; i++)
                    _screens[i].Exit();
            }
        }

        /// <summary>
        /// Get Screen at a position
        /// </summary>
        /// <param name="index">position</param>
        /// <returns>The screen at the position</returns>
        public BaseState GetScreenAt(int index)
        {
            return _screens[index];
        }

        /// <summary>
        /// Get alls screens
        /// </summary>
        /// <returns></returns>
        public BaseState[] GetScreens()
        {
            return _screens.ToArray();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (BaseState screen in _screens)
                yield return screen;
        }

        #endregion
    }
}
