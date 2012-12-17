using System;
using Microsoft.Xna.Framework.Input;
using Yna.Framework.Helpers;
using Yna.Framework.Input.Service;

namespace Yna.Framework.Input
{
    public class YnKeyboard
    {
        private IKeyboardService service;

        public YnKeyboard()
        {
            service = ServiceHelper.Get<IKeyboardService>();
        }

        public bool Pressed(Keys key)
        {
            return service.Pressed(key);
        }

        public bool Released(Keys key)
        {
            return service.Released(key);
        }

        public bool JustPressed(Keys key)
        {
            return service.JustPressed(key);
        }

        public bool JustReleased(Keys key)
        {
            return service.JustReleased(key);
        }

        public bool Up
        {
            get { return this.Pressed(Keys.Up); }
        }

        public bool Down
        {
            get { return this.Pressed(Keys.Down); }
        }

        public bool Left
        {
            get { return this.Pressed(Keys.Left); }
        }

        public bool Right
        {
            get { return this.Pressed(Keys.Right); }
        }

        public bool Enter
        {
            get { return this.Pressed(Keys.Enter); }
        }

        public bool Space
        {
            get { return this.Pressed(Keys.Space); }
        }

        public bool Escape
        {
            get { return this.Pressed(Keys.Escape); }
        }

        public bool LeftControl
        {
            get { return this.Pressed(Keys.LeftControl); }
        }

        public bool LeftShift
        {
            get { return this.Pressed(Keys.LeftShift); }
        }

        public bool Tab
        {
            get { return this.Pressed(Keys.Tab); }
        }
    }
}

