﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.State
{
    /// <summary>
    /// Define the state of a state
    /// </summary>
    public enum ScreenState
    {
        Active, Hidden, TransitionOn, TransitionOff
    }

    /// <summary>
    /// Define the type of a state
    /// </summary>
    public enum ScreenType
    {
        GameState, Popup
    }

    public abstract class Screen
    {
        #region Private declarations

        private static int ScreenCounter = 0;

        private bool _active;
        private bool _exiting;
        private bool _visible;
        private bool _isPopup;

        protected string _name;

        private ScreenState _screenState;
        private float _timeTransitionOn;
        private float _timeTransitionOff;
        private float _timeTransitionCounter;
        private float _transitionAlpha;

        protected SpriteBatch spriteBatch;
        protected ScreenManager screenManager;

        #endregion

        #region Properties

        /// <summary>
        /// Get or set a name for this screen state
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Active or desactive this screen
        /// </summary>
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        /// <summary>
        /// Get the exiting state
        /// </summary>
        public bool Exiting
        {
            get { return _exiting; }
            protected set { _exiting = value; }
        }

        /// <summary>
        /// Get or set the visibility value
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        /// <summary>
        /// Get the type of screen (PopUp or GameState)
        /// </summary>
        public bool IsPopup
        {
            get { return _isPopup; }
            protected set { _isPopup = value; }
        }

        /// <summary>
        /// Get or set the "time transition on" value
        /// </summary>
        public float TransitionOn
        {
            get { return _timeTransitionOn; }
            set { _timeTransitionOn = value; }
        }

        /// <summary>
        /// Get or set the "time transition off" value
        /// </summary>
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

        /// <summary>
        /// Get or set the "transition alpha" value
        /// </summary>
        public float TransitionAlpha
        {
            get { return _transitionAlpha; }
            set { _transitionAlpha = value; }
        }

        /// <summary>
        /// Get or Set the Screen Manager
        /// </summary>
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            set
            {
                screenManager = value;
                spriteBatch = value.SpriteBatch;
            }
        }

        /// <summary>
        /// Get or Set the state of the screen
        /// </summary>
        public ScreenState ScreenState
        {
            get { return _screenState; }
            set { _screenState = value; }
        }

        /// <summary>
        /// Define the clear color before each draw calls
        /// </summary>
        public Color ClearColor
        {
            get { return screenManager.ClearColor; }
            set { screenManager.ClearColor = value; }
        }

        #endregion

        public Screen(string name, ScreenType type, float timeTransitionOn = 1500f, float timeTransitionOff = 0.0f)
            : this(type, timeTransitionOn, timeTransitionOff)
        {
            _name = name;
        }

        public Screen(ScreenType type, float timeTransitionOn = 1500f, float timeTransitionOff = 0.0f)
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

            _name = "Screen_" + (ScreenCounter++);
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
                    _screenState = ScreenState.Active;
                else
                    UpdateTransition(gameTime, _timeTransitionOn, 1);

            }
            else if (_screenState == ScreenState.TransitionOff)
            {
                if (_timeTransitionCounter <= 0)
                    screenManager.Remove(this);
                else
                    UpdateTransition(gameTime, _timeTransitionOff, 1);
            }
        }

        /// <summary>
        /// Update Transition effect
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="timer">Fade timer</param>
        /// <param name="direction">FadeIn or FadeOut</param>
        /// <returns></returns>
        private bool UpdateTransition(GameTime gameTime, float timer, int direction)
        {
            _timeTransitionCounter -= gameTime.ElapsedGameTime.Milliseconds * direction;
            _transitionAlpha = _timeTransitionCounter / timer;
            _transitionAlpha = MathHelper.Clamp(_transitionAlpha, 0, 1);
            return _transitionAlpha >= 1 || _transitionAlpha <= 0;
        }

        /// <summary>
        /// Draw the fade effect
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Draw(GameTime gameTime)
        {
            if (_screenState == ScreenState.TransitionOn)
                screenManager.FadeBackBufferToBlack(TransitionAlpha);

            else if (_screenState == ScreenState.TransitionOff)
                screenManager.FadeBackBufferToBlack(TransitionAlpha);
        }

        /// <summary>
        /// Quit the screen and remove it from the ScreenManager
        /// </summary>
        public void Exit()
        {
            if (_timeTransitionOff <= 0)
            {
                UnloadContent();
                screenManager.Remove(this);
            }
            else
            {
                _timeTransitionCounter = _timeTransitionOff;
                _screenState = ScreenState.TransitionOff;
                _exiting = true;
            }
        }

        /// <summary>
        /// Start fadeout effect and desactive it. It's not removed from the ScreenManager so you can
        /// reactive it with Active property of with Show method.
        /// </summary>
        public void Hide()
        {
            if (_timeTransitionOff <= 0)
                Active = false;
            else
            {
                _timeTransitionCounter = _timeTransitionOff;
                _screenState = ScreenState.TransitionOff;
            }
        }

        /// <summary>
        /// Start the Fadin effect and reactive it. You can hide it with Hide method.
        /// </summary>
        public void Show()
        {
            if (_timeTransitionOn <= 0) 
                Active = true;
            else
            {
                _timeTransitionCounter = _timeTransitionOn;
                _screenState = ScreenState.TransitionOn;
            }
        }
    }
}