using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Yna.Framework.Input.Service;
using Yna.Framework.Helpers;

namespace Yna.Framework.Input
{
    public class YnTouch
    {
        private ITouchService service;

        public bool Moved
        {
            get { return service.Moved(0); }
        }

        public bool Pressed
        {
            get { return service.Pressed(0); }
        }

        public bool Released
        {
            get { return service.Released(0); }
        }

        public Vector2 Position
        {
            get { return service.GetPosition(0); }
        }

        public Vector2 LastPosition
        {
            get { return service.GetLastPosition(0); }
        }

        public Vector2 Delta
        {
            get
            {
                return Vector2.Zero; // Fixme
            }
        }

        public bool Invalid;

        public YnTouch()
        {
            service = ServiceHelper.Get<ITouchService>();
        }
    }
}
