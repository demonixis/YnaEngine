using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Yna.Engine.Input.Service;
using Yna.Engine.Helpers;

namespace Yna.Engine.Input
{
    /// <summary>
    /// The touch manager
    /// </summary>
    public class YnTouch
    {
        private TouchComponent _touchComponent;

        public bool Moved
        {
            get { return _touchComponent.Moved(0); }
        }

        public bool Pressed
        {
            get { return _touchComponent.Pressed(0); }
        }

        public bool Released
        {
            get { return _touchComponent.Released(0); }
        }

        public Vector2 Position
        {
            get { return _touchComponent.GetPosition(0); }
        }

        public Vector2 LastPosition
        {
            get { return _touchComponent.GetLastPosition(0); }
        }

        public Vector2 Delta
        {
            get 
            { 
                Vector2 delta = _touchComponent.GetPosition(0) - _touchComponent.GetLastPosition(0);
                return delta;
            }
        }

        public YnTouch(TouchComponent component)
        {
            _touchComponent = component;
        }
    }
}
