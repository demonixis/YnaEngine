using System;
using Microsoft.Xna.Framework;

namespace Yna.Framework.Display.Animation
{
    public class IntInterpolator : BaseInterpolator<int>
    {
        protected override void InterpolateValue(float step)
        {
            _interpolatedValue = (int)MathHelper.Lerp(_startValue, _endValue, step);
        }
    }

    public class FloatInterpolator : BaseInterpolator<float>
    {
        protected override void InterpolateValue(float step)
        {
            _interpolatedValue = MathHelper.Lerp(_startValue, _endValue, step);
        }
    }

    public class Vector2Interpolator : BaseInterpolator<Vector2>
    {
        protected override void InterpolateValue(float step)
        {
            _interpolatedValue = Vector2.Lerp(_startValue, _endValue, step);
        }
    }

    public class Vector3Interpolator : BaseInterpolator<Vector3>
    {
        protected override void InterpolateValue(float step)
        {
            _interpolatedValue = Vector3.Lerp(_startValue, _endValue, step);
        }
    }

    public class Vector4Interpolator : BaseInterpolator<Vector4>
    {
        protected override void InterpolateValue(float step)
        {
            _interpolatedValue = Vector4.Lerp(_startValue, _endValue, step);
        }
    }

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
        /// Triggered when started
        /// </summary>
        public event EventHandler<EventArgs> Started = null;

        /// <summary>
        /// Triggered when finished
        /// </summary>
        public event EventHandler<EventArgs> Finished = null;

        /// <summary>
        /// Triggered when restarted
        /// </summary>
        public event EventHandler<EventArgs> Restarted = null;

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

        public void StartInterpolation(T startValue, T endValue, float desiredDuration)
        {
            if (!_active)
            {
                _startValue = startValue;
                _endValue = endValue;
                _desiredDuration = desiredDuration;
                _interpolatedValue = startValue;
                _active = true;
            }
        }

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
                    }
                    else
                    {
                        _active = false;
                        _elapsedTime = _desiredDuration;
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
