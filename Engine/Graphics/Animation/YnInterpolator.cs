// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics.Animation
{
    /// <summary>
    /// An integer interpolator
    /// </summary>
    public class IntInterpolator : BaseInterpolator<int>
    {
        protected override void InterpolateValue(float step)
        {
            _interpolatedValue = (int)MathHelper.Lerp(_startValue, _endValue, step);
        }
    }

    /// <summary>
    /// A float interpolator
    /// </summary>
    public class FloatInterpolator : BaseInterpolator<float>
    {
        protected override void InterpolateValue(float step)
        {
            _interpolatedValue = MathHelper.Lerp(_startValue, _endValue, step);
        }
    }

    /// <summary>
    /// A Vector2 interpolator
    /// </summary>
    public class Vector2Interpolator : BaseInterpolator<Vector2>
    {
        protected override void InterpolateValue(float step)
        {
            _interpolatedValue = Vector2.Lerp(_startValue, _endValue, step);
        }
    }

    /// <summary>
    /// A Vector3 interpolator
    /// </summary>
    public class Vector3Interpolator : BaseInterpolator<Vector3>
    {
        protected override void InterpolateValue(float step)
        {
            _interpolatedValue = Vector3.Lerp(_startValue, _endValue, step);
        }
    }

    /// <summary>
    /// A Vector4 interpolator
    /// </summary>
    public class Vector4Interpolator : BaseInterpolator<Vector4>
    {
        protected override void InterpolateValue(float step)
        {
            _interpolatedValue = Vector4.Lerp(_startValue, _endValue, step);
        }
    }

    /// <summary>
    /// Base class used to construct an interpolator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseInterpolator<T>
    {
        protected bool _active;
        protected float _desiredDuration;
        protected float _elapsedTime;
        protected T _startValue;
        protected T _endValue;
        protected T _interpolatedValue;
        protected bool _repeat;

        #region Properties

        /// <summary>
        /// Enable or disable looped interpolation
        /// </summary>
        public bool Repeat
        {
            get { return _repeat; }
            set { _repeat = value; }
        }

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Triggered when current interpolation is started
        /// </summary>
        public event EventHandler<EventArgs> Started = null;

        /// <summary>
        /// Triggered when current interpolation is finished
        /// </summary>
        public event EventHandler<EventArgs> Finished = null;

        /// <summary>
        /// Triggered when current interpolation is restarted
        /// </summary>
        public event EventHandler<EventArgs> Restarted = null;

        private void OnStarted(EventArgs e)
        {
            if (Started != null)
                Started(this, e);
        }

        private void OnFinished(InterpolationEndEventArgs<T> e)
        {
            if (Finished != null)
                Finished(this, e);
        }


        private void OnRestarted(InterpolationEndEventArgs<T> e)
        {
            if (Restarted != null)
                Restarted(this, e);
        }

        #endregion

        public BaseInterpolator()
        {
            _active = false;
            _elapsedTime = 0;
            _desiredDuration = 0;
            _startValue = default(T);
            _endValue = default(T);
            _interpolatedValue = default(T);
            _repeat = false;
        }

        /// <summary>
        /// Start a new interpolation
        /// </summary>
        /// <param name="startValue">Start value</param>
        /// <param name="endValue">End value</param>
        /// <param name="desiredDuration">Desired duration</param>
        public void StartInterpolation(T startValue, T endValue, float desiredDuration)
        {
            if (!_active)
            {
                _startValue = startValue;
                _endValue = endValue;
                _desiredDuration = desiredDuration;
                _elapsedTime = 0.0f;
                _interpolatedValue = startValue;
                _active = true;
                OnStarted(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Start a new interpolation
        /// </summary>
        /// <param name="startValue">Start value</param>
        /// <param name="endValue">End value</param>
        /// <param name="desiredDuration">Desired duration</param>
        public void StartInterpolation(T startValue, T endValue, float desiredDuration, bool force)
        {
            if (!_active || force)
            {
                _startValue = startValue;
                _endValue = endValue;
                _desiredDuration = desiredDuration;
                _elapsedTime = 0.0f;
                _interpolatedValue = startValue;
                _active = true;
                OnStarted(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Update and get the interpolated value
        /// </summary>
        /// <param name="elaspedMilliseconds">The time elapsed since last call</param>
        /// <returns>The interpolated value</returns>
        public T GetInterpolatedValue(float elaspedMilliseconds)
        {
            if (_active)
            {
                _elapsedTime += elaspedMilliseconds;

                if (_elapsedTime >= _desiredDuration)
                {
                    if (_repeat)
                    {
                        _active = true;
                        _elapsedTime = 0;
                        OnRestarted(new InterpolationEndEventArgs<T>(_elapsedTime, _interpolatedValue));
                    }
                    else
                    {
                        _active = false;
                        _elapsedTime = _desiredDuration;
                        OnFinished(new InterpolationEndEventArgs<T>(_elapsedTime, _interpolatedValue));
                    }
                }

                float step = (float)(_elapsedTime / _desiredDuration);
                InterpolateValue(step);
            }

            return _interpolatedValue;
        }

        protected abstract void InterpolateValue(float step);
    }
}
