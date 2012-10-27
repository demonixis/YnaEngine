using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Yna.Content;
using Yna.Helpers;

namespace Yna.State
{
    public class ScreenManager : DrawableGameComponent
    {
        #region Private declarations

        private List<Screen> _screens;
        private List<Screen> _safeScreens;
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
        public Screen this[int index]
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
        public Screen FirstActiveScreen
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
        public Screen LastActiveScreen
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

        public ScreenManager(Game game)
            : base(game)
        {
            _clearColor = Color.Black;

            _screens = new List<Screen>();
            _safeScreens = new List<Screen>();
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
                _transitionTexture = GraphicsHelper.CreateTexture(Color.White, 16, 16);

                foreach (Screen screen in _screens)
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
                foreach (Screen screen in _screens)
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
                    if (_safeScreens[i].Visible)
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
        public void SetScreenActive(int index, bool desactiveOtherScreens = true)
        {
            int size = _screens.Count;

            if (index < 0 || index > size - 1)
                throw new IndexOutOfRangeException("[ScreenManager] The screen doesn't exist at this index");

            _screens[index].Active = true;

            if (desactiveOtherScreens)
            {
                for (int i = 0; i < size; i++)
                {
                    if (i != index)
                        _screens[i].Hide();
                }
            }
        }

        /// <summary>
        /// Hide all popup
        /// </summary>
        public void HideAllPopup()
        {
            foreach (Screen screen in _screens)
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

        #endregion

        #region Collection methods

        /// <summary>
        /// Add a new screen to the Manager. The screen is not activate or desactivate, you must manage this yourself
        /// </summary>
        /// <param name="screen">Screen to add</param>
        public void Add(Screen screen)
        {
            screen.ScreenManager = this;

            // If the manager is not yet ready we don't need to initialize and load its content
            // Because it's donne in the init. process
            if (_initialized)
            {
                screen.LoadContent();
                screen.Initialize();
            }

            _screens.Add(screen);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="active"></param>
        public void Add(Screen screen, bool active)
        {
            screen.Active = active;
            Add(screen);
        }

        /// <summary>
        /// Remove a screen to the Manager
        /// </summary>
        /// <param name="screen">Screen to remove</param>
        public void Remove(Screen screen)
        {
            _screens.Remove(screen);

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
        public Screen GetScreenAt(int index)
        {
            return _screens[index];
        }

        /// <summary>
        /// Get alls screens
        /// </summary>
        /// <returns></returns>
        public Screen[] GetScreens()
        {
            return _screens.ToArray();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (Screen screen in _screens)
                yield return screen;
        }

        #endregion
    }
}
