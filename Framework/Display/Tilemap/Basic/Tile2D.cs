using System;

namespace Yna.Framework.Display.TileMap.Basic
{
	/// <summary>
	/// A Tile is a section of a TileMap.
	/// </summary>
	public class Tile2D : AbstractTile
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="x">X position in the map</param>
		/// <param name="y">Y position in the map</param>
		/// <param name="textureID">Texture ID fore rendering</param>
		public Tile2D(int x, int y, int textureID) : base(x, y, textureID)
		{
		}
	}
}
