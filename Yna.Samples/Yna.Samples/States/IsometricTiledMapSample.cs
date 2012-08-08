using System;
using Yna.Display.TiledMap.Isometric;
using Yna.State;

namespace Yna.Samples.Windows.States
{
	/// <summary>
	/// Description of IsometricTiledMapSample.
	/// </summary>
	public class IsometricTiledMapSample : YnState
	{
		/// <summary>
		/// This is the TiledMap component
		/// </summary>
		private IsometricTiledMap _map;
		
		
		public IsometricTiledMapSample()
		{
			_map = new IsometricTiledMap()
		}
	}
}
