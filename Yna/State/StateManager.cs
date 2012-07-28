using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Yna.Helpers;

namespace Yna.State
{
    public class StateManager 
    {
        public Game Game { get; protected set; }

        public GraphicsDevice GraphicsDevice 
        { 
            get { return Game.GraphicsDevice; }
        }

        public ContentManager Content 
        {
            get { return Game.Content; }
        }

        private List<GameState> _screens;
        private List<GameState> _safeScreens;

        private bool _initialized;

        private Texture2D _transitionTexture;

        private SpriteBatch _spriteBatch;

        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
            set { _spriteBatch = value; }
        }

        public StateManager(Game game)
        {
            Game = game;

            _screens = new List<GameState>();
            _safeScreens = new List<GameState>();
            _initialized = false;
            Initialize();
        }

        public virtual void Initialize()
        {

            foreach (GameState state in _screens)
                state.Initialize();
        }

        public virtual void LoadContent()
        {
            if (!_initialized)
            {
                _spriteBatch = new SpriteBatch(GraphicsDevice);
                // La texture sera étirée
                _transitionTexture = GraphicsHelper.CreateTexture(Color.White, 16, 16);

                foreach (GameState screen in _screens)
                    screen.LoadContent();

                _initialized = true;
            }
        }

        public virtual void UnloadContent()
        {
            if (_initialized && _screens.Count > 0)
            {
                foreach (GameState screen in _screens)
                    screen.UnloadContent();
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            // Copie sûr des states en cours
            foreach (GameState screen in _screens)
                _safeScreens.Add(screen);

            while (_safeScreens.Count > 0)
            {
                int index = _safeScreens.Count - 1;
                GameState screen = _safeScreens[index];
                _safeScreens.RemoveAt(index);

                if (!screen.Visible)
                    continue;

                screen.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkCyan);

            foreach (GameState screen in _screens) 
            {
                if (!screen.Visible)
                    continue;

                screen.Draw(gameTime);
            }
        }

        public void FadeBackBufferToBlack(float alpha)
        {
            FadeBackBuffer(Color.Black, alpha);
        }

        public void FadeBackBuffer(Color color, float alpha)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_transitionTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), color * alpha);
            _spriteBatch.End();
        }

        #region Ajout, Suppression et Récupération des Screen
        public void Add(GameState screen)
        {
            screen.ScreenManager = this;

            // Si le manager n'est pas encore prêt le Screen actuel sera chargé en même temps 
            // que les autres, dans LoadContent().
            if (_initialized)
            {
                screen.Initialize();
                screen.LoadContent();
            }

            _screens.Add(screen);
        }

        public void Remove(GameState screen)
        {
            _screens.Remove(screen);
            _safeScreens.Remove(screen);
            
            if (_initialized)
                screen.UnloadContent();
        }

        public void Clear()
        {
            if (_screens.Count > 0)
            {
                for (int i = 0; i < _screens.Count; i++)
                    _screens[i].Exit();
            }
        }

        public GameState GetScreenAt(int index)
        {
            return _screens[index];
        }

        public GameState[] GetScreens()
        {
            return _screens.ToArray();
        }
        #endregion
    }
}
