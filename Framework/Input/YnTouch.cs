using System;
using System.Collections.Generic;
using System.Linq;
using Yna.Input.Service;
using Yna.Helpers;

namespace Yna.Input
{
    public class YnTouch
    {
        private TouchService service;

        public bool Moved;
        public bool Pressed;
        public bool Released;
        public bool Invalid;

        public YnTouch()
        {
            service = ServiceHelper.Get<ITouchService>() as TouchService;
        }
    }
}
