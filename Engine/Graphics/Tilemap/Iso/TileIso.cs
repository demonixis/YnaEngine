// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;

namespace Yna.Engine.Graphics.TileMap.Isometric
{
	/// <summary>
	/// A 3DTile stores x and y coordinates and heights for each vertices
	/// </summary>
	public class TileIso : BaseTile
	{
		const int TOP_LEFT = 0;
		const int TOP_RIGHT = 1;
		const int BOTTOM_RIGHT = 2;
		const int BOTTOM_LEFT = 3;
			
		private int[] _heights;
		
		#region Properties
		public int TopLeft
		{
			get{return _heights[TOP_LEFT];}
			set{_heights[TOP_LEFT] = value;}
		}
		public int TopRight
		{
			get{return _heights[TOP_RIGHT];}
			set{_heights[TOP_RIGHT] = value;}
		}
		public int BottomRight
		{
			get{return _heights[BOTTOM_RIGHT];}
			set{_heights[BOTTOM_RIGHT] = value;}
		}
		public int BottomLeft
		{
			get{return _heights[BOTTOM_LEFT];}
			set{_heights[BOTTOM_LEFT] = value;}
		}
		#endregion

		#region Constructors
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
			// Heights are initialized at 0
			_heights = new int[4];
			FlattenTo(0);
		}
		#endregion
		
		/// <summary>
		/// Flattens the tile to the given height. All vertex are set
		/// with the height
		/// </summary>
		/// <param name="height">The height</param>
		public void FlattenTo(int height)
		{
			TopLeft = height;
			TopRight = height;
			BottomRight = height;
			BottomLeft = height;
		}
		
		/// <summary>
		/// Check tile's heights and return the type. Check documentation 
		/// for details
		/// </summary>
		/// <returns></returns>
		public int TileType()
		{
			int type = 0;
			if(IsFlat())
			{
				type = 0;
			}
			else if(IsSlope())
			{
				if(TopLeft == TopRight 
				   && TopLeft > BottomLeft)
				{
					type = 5;
				}
				else if(TopRight == BottomRight
				       && TopRight > TopLeft)
				{
					type = 6;
				}
				else if(BottomRight == BottomLeft
				       && BottomRight > TopLeft)
				{
					type = 7;
				}
				else if(TopLeft == BottomLeft
				       && TopLeft > TopRight)
				{
					type = 8;
				}
			}
			
			return type;
		}
		
		/// <summary>
		/// Calculate the max height of the tile
		/// </summary>
		/// <returns>The max height of the tile's vertices</returns>
		public int MaxHeight()
		{
			int max = Math.Max(TopLeft, TopRight);
			max = Math.Max(max, BottomRight);
			max = Math.Max(max, BottomLeft);
			return max;
		}
		
		/// <summary>
		/// Calculate the min height of the tile
		/// </summary>
		/// <returns>The min height of the tile's vertices</returns>
		public int MinHeight()
		{
			int min = Math.Min(TopLeft, TopRight);
			min = Math.Min(min, BottomRight);
			min = Math.Min(min, BottomLeft);
			return min;
		}
		
		/// <summary>
		/// Tests the tile's heights to define if it is flat
		/// </summary>
		/// <returns></returns>
		public bool IsFlat()
		{
			return TopLeft == TopRight
				&& TopLeft == BottomRight
				&& TopLeft == BottomLeft;
		}
		
		/// <summary>
		/// Tests if the tile is "craggy". A craggy tile has a height
		/// difference of 2 between it's lower and higher vertex. That's the 
		/// maximum allowed
		/// </summary>
		/// <returns></returns>
		public bool IsCraggy()
		{
			return MaxHeight() - MinHeight() == 2;
		}
		
		/// <summary>
		/// Tests if the tile is a simple slope : heights are equals 2 by 2
		/// and equals vertices are neighbours.
		/// </summary>
		/// <returns></returns>
		public bool IsSlope()
		{
			return (TopLeft == TopRight && BottomLeft == BottomRight
			       || TopLeft == BottomLeft && TopRight == BottomRight)
				&& MaxHeight() != MinHeight();
		}
	}
}
