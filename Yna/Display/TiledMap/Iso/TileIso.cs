using System;

namespace Yna.Display.TiledMap.Isometric
{
	/// <summary>
	/// A 3DTile stores x and y coordinates and heights for each vertices
	/// </summary>
	public class TileIso : AbstractTile
	{
		private int[] _heights;
		
		/// <summary>
		/// Constructor with defined heights
		/// </summary>
		/// <param name="x">X position in the map</param>
		/// <param name="y">Y position in the map</param>
		/// <param name="textureID">The texture ID</param>
		/// <param name="heights">The tile's heights</param>
		public TileIso(int x, int y, int textureID, int[] heights) : base(x, y, textureID)
		{
			_heights = heights;
		}
		
		/// <summary>
		/// Constructor with default heights (0)
		/// </summary>
		/// <param name="x">X position in the map</param>
		/// <param name="y">Y position in the map</param>
		/// <param name="textureID">The texture ID</param>
		public TileIso(int x, int y, int textureID) : base(x, y, textureID)
		{
			// Heights are initialize at 0
			_heights = new int[4];
			_heights[0] = 0;
			_heights[1] = 0;
			_heights[2] = 0;
			_heights[3] = 0;
		}
	}
	
	/// <summary>
	/// Isométric tiles (Tile3D) store informations about their vertices
	/// heights. They're identified by those values
	/// </summary>
	public enum Vertex
	{
		/// <summary>
		/// Top left vertex (the top one when rendered)
		/// </summary>
		TopLeft = 0,
		
		/// <summary>
		/// Top right vertex (the right one when rendered)
		/// </summary>
		TopRight = 1,
		
		/// <summary>
		/// Bottom right vertex (the bottom one when rendered)
		/// </summary>
		BottomRight = 2,
		
		/// <summary>
		/// Bottom left vertex (the left one when rendered)
		/// </summary>
		BottomLeft = 3
	}
}
