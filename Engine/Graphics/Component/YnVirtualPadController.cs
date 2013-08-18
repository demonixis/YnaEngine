// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Yna.Engine.Graphics.Event;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics.Component
{
    /// <summary>
    /// A Virtual pad controller that create a virtual pad and manage events
    /// </summary>
    public class YnVirtualPadController
    {
        protected bool[] _buttons;

        private YnVirtualPad _virtualPad;

        /// <summary>
        /// Gets or sets the virtual pad used with this controller
        /// </summary>
        public YnVirtualPad VirtualPad
        {
            get { return _virtualPad; }
            set { _virtualPad = value; }
        }

        /// <summary>
        /// Create a new controller for a virtual pad
        /// </summary>
        public YnVirtualPadController()
        {
            _virtualPad = new YnVirtualPad();
            _virtualPad.VirtualPadPressed += _virtualPad_Pressed;
            _virtualPad.VirtualPadReleased += _virtualPad_Released;
            _buttons = new bool[10];

            for (int i = 0; i < 10; i++)
                _buttons[i] = false;
        }

		public YnVirtualPadController(YnVirtualPad virtualPad)
        {
            _virtualPad = virtualPad;
            _virtualPad.VirtualPadPressed += _virtualPad_Pressed;
            _virtualPad.VirtualPadReleased += _virtualPad_Released;
            _buttons = new bool[10];

            for (int i = 0; i < 10; i++)
                _buttons[i] = false;
        }
		
        private void _virtualPad_Released(object sender, VirtualPadPressedEventArgs e)
        {
            _buttons[(int)e.Direction] = false;
        }

        private void _virtualPad_Pressed(object sender, VirtualPadPressedEventArgs e)
        {
            _buttons[(int)e.Direction] = true;
        }

        public bool Pressed(PadButtons button)
        {
            return _buttons[(int)button];
        }
		
		public bool JustPressed(PadButtons button)
		{
            return false;
		}
		
		public bool Released(PadButtons button)
		{
			return !_buttons[(int)button];
		}

        public bool hasPressedButton()
        {
            bool result = false;

            foreach (bool button in _buttons)
            {
                if (button)
                    result = true;
            }

            return result;
        }

        public void LoadContent()
        {
            _virtualPad.LoadContent();
            _virtualPad.Initialize();
        }

        public void Update(GameTime gameTime)
        {
            _virtualPad.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _virtualPad.Draw(gameTime, spriteBatch);
        }

        public void DrawOnSingleBatch(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _virtualPad.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}
