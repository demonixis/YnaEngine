// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;

namespace Yna.Engine.Graphics.Animation
{
    /// <summary>
    /// Event class used when an interpolator object done its job.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InterpolationEndEventArgs<T> : EventArgs
    {
        public T InterpolatedValue;
        public float ElapsedTime;

        public InterpolationEndEventArgs()
        {
            ElapsedTime = 0.0f;
            InterpolatedValue = default(T);
        }

        public InterpolationEndEventArgs(float elapsedTime, T value)
        {
            ElapsedTime = elapsedTime;
            InterpolatedValue = value;
        }
    }
}
