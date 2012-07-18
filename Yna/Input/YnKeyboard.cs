using System;
using Microsoft.Xna.Framework.Input;
using Yna.Helpers;

namespace Yna.Input
{
	public class YnKeyboard
	{
		public YnKeyboard () { }
		
		public bool Pressed(Keys key) 
		{
			return ServiceHelper.Get<IKeyboardService> ().Pressed (key);		
		}
		
		public bool Released (Keys key)
		{
			return ServiceHelper.Get<IKeyboardService> ().Released (key);
		}
		
		public bool JustPressed (Keys key)
		{
			return ServiceHelper.Get<IKeyboardService> ().JustPressed (key);
		}
		
		public bool JustReleased (Keys key)
		{
			return ServiceHelper.Get<IKeyboardService> ().JustReleased (key);
		}

        public bool Up { get { return this.Pressed(Keys.Up); } }

        public bool Down { get { return this.Pressed(Keys.Down); } }

        public bool Left { get { return this.Pressed(Keys.Left); } }

        public bool Right { get { return this.Pressed(Keys.Right); } }

		public bool Enter { get { return this.Pressed(Keys.Enter); } }

		public bool Space { get { return this.Pressed(Keys.Space); } }

		public bool Escape { get { return this.Pressed(Keys.Escape); } }

		public bool LeftControl { get { return this.Pressed(Keys.LeftControl); } }

		public bool LeftShift { get { return this.Pressed(Keys.LeftShift); } }

		public bool Tab { get { return this.Pressed(Keys.Tab); } }
	}
}

