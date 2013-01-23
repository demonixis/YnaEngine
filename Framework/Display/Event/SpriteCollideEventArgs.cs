using System;

namespace Yna.Framework.Display.Event
{
	public enum SpriteScreenCollide
	{
		Top = 0, Bottom, Left, Right
	}
	
	/// <summary>
	/// Description of SpriteScreenCollideEventArgs.
	/// </summary>
	public class SpriteCollideEventArgs : EventArgs
	{
        public SpriteScreenCollide Collide;

		public SpriteCollideEventArgs(SpriteScreenCollide collide)
		{
			Collide = collide;
		}
	}
}
