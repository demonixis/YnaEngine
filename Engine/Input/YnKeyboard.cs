using Microsoft.Xna.Framework.Input;
using Yna.Engine.Input.Service;

namespace Yna.Engine.Input
{
    /// <summary>
    /// The keyboard manager
    /// </summary>
    public class YnKeyboard
    {
        private KeyboardComponent _keyboardComponent;

        public YnKeyboard(KeyboardComponent keyboardComponent)
        {
            _keyboardComponent = keyboardComponent;
        }

        public bool Pressed(Keys key)
        {
            return _keyboardComponent.Pressed(key);
        }

        public bool Released(Keys key)
        {
            return _keyboardComponent.Released(key);
        }

        public bool JustPressed(Keys key)
        {
            return _keyboardComponent.JustPressed(key);
        }

        public bool JustReleased(Keys key)
        {
            return _keyboardComponent.JustReleased(key);
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

