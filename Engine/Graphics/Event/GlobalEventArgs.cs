// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;

namespace Yna.Engine.Graphics.Event
{
    public class ValueChangedEventArgs<T> : EventArgs
    {
        public T Value { get; protected set; }

        public ValueChangedEventArgs()
        {
            Value = default(T);
        }

        public ValueChangedEventArgs(T t)
        {
            Value = t;
        }
    }
}
