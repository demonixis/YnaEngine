// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics.Animation
{
    public enum TransitionState
    {
        On = 0, Off = 1, FadeIn, FadeOut
    }

    /// <summary>
    /// Simple transition effect
    /// </summary>
    public class YnTransitionEffect : YnBasicEntity, IEffectAnimation
    {
        private TransitionState _transitionState;
        private float _timeTransitionOn;
        private float _timeTransitionOff;
        private float _timeTransitionCounter;
        private float _transitionAlpha;
        private float _alphaMin;
        private float _alphaMax;

        /// <summary>
        /// Get alpha value (0.0f .. 1.0f)
        /// </summary>
        public float Alpha
        {
            get { return _transitionAlpha; }
        }

        /// <summary>
        /// Get the current status of the transition
        /// </summary>
        public TransitionState TransitionState
        {
            get { return _transitionState; }
        }

        public float AlphaMax
        {
            get { return _alphaMax; }
            set { _alphaMax = value; }
        }

        public float AlphaMin
        {
            get { return _alphaMin; }
            set { _alphaMin = value; }
        }


        #region Events

        /// <summary>
        /// Triggered when the transition on is active
        /// </summary>
        public event EventHandler<EventArgs> TransitionOnActive = null;

        /// <summary>
        /// Triggered when the transition on is starting
        /// </summary>
        public event EventHandler<EventArgs> TransitionOnStarted = null;

        /// <summary>
        /// Triggered when the transition on is terminated
        /// </summary>
        public event EventHandler<EventArgs> TransitionOnFinished = null;

        /// <summary>
        /// Triggered when the transition off is active
        /// </summary>
        public event EventHandler<EventArgs> TransitionOffActive = null;

        /// <summary>
        /// Triggered when the transition of is starting
        /// </summary>
        public event EventHandler<EventArgs> TransitionOffStarted = null;
        
        /// <summary>
        /// Triggered when the transition off is terminated
        /// </summary>
        public event EventHandler<EventArgs> TransitionOffFinished = null;

        private void OnTransitionActive(EventArgs e, bool isTransitionOn)
        {
            if (isTransitionOn && TransitionOnActive != null)
                TransitionOnActive(this, e);
            else if (!isTransitionOn && TransitionOffActive != null)
                TransitionOffActive(this, e);
        }

        private void OnTransitionStarted(EventArgs e, bool isTransitionOn)
        {
            if (isTransitionOn && TransitionOnStarted != null)
                TransitionOnStarted(this, e);
            else if (!isTransitionOn && TransitionOffStarted != null)
                TransitionOffStarted(this, e);
        }

        private void OnTransitionFinished(EventArgs e, bool isTransitionOn)
        {
            if (isTransitionOn && TransitionOnFinished != null)
                TransitionOnFinished(this, e);
            else if (!isTransitionOn && TransitionOffFinished != null)
                TransitionOffFinished(this, e);
        }

        #endregion


        /// <summary>
        /// Create a transition alpha effect
        /// </summary>
        /// <param name="fadeInTime">Duration of the start transition</param>
        /// <param name="fadeOutTime">Duration of the end transition</param>
        public YnTransitionEffect(float fadeInTime, float fadeOutTime)
        {
            _transitionState = TransitionState.Off;
            _timeTransitionOn = fadeInTime;
            _timeTransitionOff = fadeOutTime;
            _alphaMin = 0.0f;
            _alphaMax = 1.0f;
        }

        public YnTransitionEffect(float fadeTime)
            : this(fadeTime, fadeTime)
        {

        }

        /// <summary>
        /// Update transitions
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Update(GameTime gameTime)
        {
            if (_transitionState == TransitionState.FadeIn)
            {
                if (UpdateTransition(gameTime, _timeTransitionOn, -1))
                {
                    _transitionState = TransitionState.On;
                    OnTransitionFinished(EventArgs.Empty, true);
                }
                else
                {
                    OnTransitionActive(EventArgs.Empty, true);
                }
            }

            else if (_transitionState == TransitionState.FadeOut)
            {
                if (UpdateTransition(gameTime, _timeTransitionOff, 1))
                {
                    _transitionState = TransitionState.Off;
                    OnTransitionFinished(EventArgs.Empty, false);
                }
                else
                {
                    OnTransitionActive(EventArgs.Empty, true);
                }
            }
        }

        /// <summary>
        /// Start transition on
        /// </summary>
        public void StartFadeIn()
        {
            ChangeTransitionState(TransitionState.FadeIn);
        }

        /// <summary>
        /// Start transition off
        /// </summary>
        public void StartFadeOut()
        {
            ChangeTransitionState(TransitionState.FadeOut);
        }
        
        /// <summary>
        /// Change the current translation State
        /// Values are set to default
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeTransitionState(TransitionState newState)
        {
            if (newState == TransitionState.FadeIn)
            {
                _timeTransitionCounter = 0;
                _transitionAlpha = 0.0f;
                OnTransitionStarted(EventArgs.Empty, true);
            }
            else if (newState == TransitionState.FadeOut)
            {
                _timeTransitionCounter = _timeTransitionOff;
                _transitionAlpha = 1.0f;
                OnTransitionStarted(EventArgs.Empty, false);
            }

            _transitionState = newState;
        }

        /// <summary>
        /// Update the current transition
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        /// <param name="timer">Duration (on or off)</param>
        /// <param name="direction">Fadein: 1 and Fadeout: -1</param>
        /// <returns></returns>
        private bool UpdateTransition(GameTime gameTime, float timer, int direction)
        {
            _timeTransitionCounter -= gameTime.ElapsedGameTime.Milliseconds * direction;
            _transitionAlpha = _timeTransitionCounter / timer;
            _transitionAlpha = MathHelper.Clamp(_transitionAlpha, 0, 1);
            return _transitionAlpha >= 1 || _transitionAlpha <= 0;
        }
    }
}
