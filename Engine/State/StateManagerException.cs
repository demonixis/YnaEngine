// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;

namespace Yna.Engine.State
{
    public class StateManagerException : Exception
    {
        public StateManagerException(string message)
            : base(message)
        {
        }
    }
}
