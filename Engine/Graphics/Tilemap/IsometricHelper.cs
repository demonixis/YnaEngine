// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics.TileMap.Isometric
{
	/// <summary>
	/// Utility class for isometric transformations and ease
	/// </summary>
	public class IsometricHelper
	{
		/// <summary>
		/// Transform isometric coordinates into orthogonal coordinates
		/// </summary>
		/// <param name="point">The point to transform</param>
		/// <returns></returns>
		public static Vector2 ToOrtho(Vector2 p)
		{
			return ToOrtho(p.X, p.Y);
		}
		
		/// <summary>
		/// Transform isometric coordinates into orthogonal coordinates
		/// </summary>
		/// <param name="x">X isometric coordinate</param>
		/// <param name="y">Y isometric cooridnate</param>
		/// <returns></returns>
		public static Vector2 ToOrtho(float x, float y)
		{
			Vector2 p = new Vector2();
			p.X = x - y;
			p.Y = (x + y) / 2;
			return p;
		}
		
		/// <summary>
		/// Transform orthogonal coordinates into isometric coordinates
		/// </summary>
		/// <param name="point">The point to transform</param>
		/// <returns></returns>
		public static Vector2 ToIso(Vector2 p)
		{
			return ToIso(p.X, p.Y);
		}
		
		/// <summary>
		/// Transform orthogonal coordinates into isometric coordinates
		/// </summary>
		/// <param name="x">X orthogonal coordinate</param>
		/// <param name="y">Y orthogonal cooridnate</param>
		/// <returns></returns>
		public static Vector2 ToIso(float x, float y)
		{
			Vector2 p = new Vector2();
			p.Y = (2 * y - x) / 2;
			p.X = (x + p.Y);
			return p;
		}
		
		/// <summary>
		/// Get the rectangle containing the proper texture for the tile, 
		/// according to it's texture ID and heights
		/// </summary>
		/// <param name="tile">The tile</param>
		/// <returns></returns>
		public static Rectangle TextureRect(TileIso tile)
		{
			return new Rectangle();
		}
	}
}
