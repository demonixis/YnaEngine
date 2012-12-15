using System;
using System.Collections.Generic;
using System.Linq;
using Yna.Framework.Input.Service;
using Yna.Framework.Helpers;

namespace Yna.Framework.Input
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
