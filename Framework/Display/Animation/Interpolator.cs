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

        public BaseInterpolator()
        {
            _active = false;
            _elapsedTime = 0;
            _desiredDuration = 0;
            _startValue = default(T);
            _endValue = default(T);
            _interpolatedValue = default(T);
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
                    _active = false;
                    _elapsedTime = _desiredDuration;
                }

                float step = (float)(_elapsedTime / _desiredDuration);
                InterpolateValue(step);
            }

            return _interpolatedValue;
        }

        protected abstract void InterpolateValue(float step);
    }
}
