using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.State
{
    public enum ScreenState
    {
        Active, Hidden, TransitionOn, TransitionOff
    }

    public enum ScreenType
    {
        GameState, Popup
    }

    public abstract class GameState
    {
        private bool _active;
        private bool _exiting;
        private bool _visible;
        private bool _isPopup;

        protected ScreenState _screenState;
        protected float _timeTransitionOn;
        protected float _timeTransitionOff;
        protected float _timeTransitionCounter;
        protected float _transitionAlpha;

        protected SpriteBatch spriteBatch;
        protected StateManager screenManager;

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public bool Exiting
        {
            get { return _exiting; }
            set { _exiting = value; }
        }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public bool IsPopup
        {
            get { return _isPopup; }
            set { _isPopup = value; }
        }

        public float TransitionOn
        {
            get { return _timeTransitionOn; }
            set { _timeTransitionOn = value; }
        }

        public float TransitionOff
        {
            get 
            {
                if (_timeTransitionOff <= 0)
                    _timeTransitionOff = 1;
                return _timeTransitionOff;
            }
            set { _timeTransitionOff = value; }
        }

        public float TransitionAlpha
        {
            get { return _transitionAlpha; }
            set { _transitionAlpha = value; }
        }

        public StateManager ScreenManager
        {
            get { return screenManager; }
            set 
            { 
                screenManager = value;
                spriteBatch = value.SpriteBatch;
            }
        }

        public ScreenState ScreenState 
        {
            get { return _screenState; }
            set { _screenState = value; }
        }

        public GameState(ScreenType type, float timeTransitionOn = 1500f, float timeTransitionOff = 0f)
        {
            _active = true;
            _exiting = false;
            _visible = true;
            _isPopup = (type == ScreenType.Popup) ? true : false;
            _screenState = ScreenState.TransitionOn;

            _timeTransitionOn = timeTransitionOn;
            _timeTransitionOff = timeTransitionOff;
            _timeTransitionCounter = _timeTransitionOn;
            _transitionAlpha = 1.0f;
        }

        public virtual void Initialize()
        {

        }

        public virtual void LoadContent()
        {
            spriteBatch = new SpriteBatch(screenManager.Game.GraphicsDevice);
        }

        public virtual void UnloadContent()
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            if (_screenState == ScreenState.TransitionOn)
            {
                if (_timeTransitionCounter <= 0)
                {
                    _screenState = ScreenState.Active;
                }
                else
                {
                    UpdateTransition(gameTime, _timeTransitionOn, 1);
                }
            }
            else if (_screenState == ScreenState.TransitionOff)
            {
                if (_timeTransitionCounter <= 0)
                    screenManager.Remove(this);
                else
                {
                    UpdateTransition(gameTime, _timeTransitionOff, -1);
                }
            }
        }

        private bool UpdateTransition(GameTime gameTime, float timer, int direction)
        {
            _timeTransitionCounter -= gameTime.ElapsedGameTime.Milliseconds;
            _transitionAlpha = _timeTransitionCounter / timer;
            _transitionAlpha = MathHelper.Clamp(_transitionAlpha, 0, 1);
            return _transitionAlpha >= 1 || _transitionAlpha <= 0;
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (_screenState == ScreenState.TransitionOn)
            {
                screenManager.FadeBackBufferToBlack(TransitionAlpha);
            }
            else if (_screenState == ScreenState.TransitionOff)
            {
                screenManager.FadeBackBufferToBlack(TransitionAlpha);
            }
        }

        public void Exit()
        {
            if (_timeTransitionOff <= 0)
                screenManager.Remove(this);
            else
            {
                _timeTransitionCounter = _timeTransitionOff;
                _screenState = ScreenState.TransitionOff;
                _exiting = true;
            }
        }
    }
}
