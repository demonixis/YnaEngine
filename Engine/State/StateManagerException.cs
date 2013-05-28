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
