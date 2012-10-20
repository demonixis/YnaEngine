using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

        #endregion

        #region Properties

        public Color ClearColor { get; set; }

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

        #endregion

        #region GameState pattern

        public ScreenManager(Game game)
            : base(game)
        {
            ClearColor = Color.Black;

            _screens = new List<Screen>();
            _safeScreens = new List<Screen>();
            _initialized = false;
        }

        protected override void LoadContent()
        {
            if (!_initialized)
            {
                _spriteBatch = new SpriteBatch(GraphicsDevice);
                // La texture sera étirée
                _transitionTexture = GraphicsHelper.CreateTexture(Color.White, 16, 16);

                foreach (Screen screen in _screens)
                {
                    screen.LoadContent();
                    screen.Initialize();
                }

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
            GraphicsDevice.Clear(ClearColor);

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

        #region Collection methods

        /// <summary>
        /// Add a new screen to the Manager
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
