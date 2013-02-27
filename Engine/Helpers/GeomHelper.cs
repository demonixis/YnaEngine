using System;

namespace Yna.Engine.Helpers
{
	/// <summary>
	/// Helper for geometry
	/// </summary>
	public class GeomHelper
	{
		/// <summary>
		/// Ratio to convert a radian to a degree angle value : radAngle * RAD_TO_DEG = degAngle
		/// </summary>
		public const float RAD_TO_DEG = 57.2957795F;
		
		
		/// <summary>
		/// Ratio to convert a degree to a radian angle value : degAngle * DEG_TO_RAD = radAngle
		/// </summary>
		public const float DEG_TO_RAD = 0.0174532925F;
		
		/// <summary>
		/// Converts a degree angle into radian
		/// </summary>
		/// <param name="angle">The angle in degree</param>
		/// <returns>The angle in radians</returns>
		public static float degreeToRadian(float angle)
		{
			return angle * DEG_TO_RAD;
		}
		
		/// <summary>
		/// Converts a radian angle to degree
		/// </summary>
		/// <param name="angle">The angle in radians</param>
		/// <returns>The angle in degree</returns>
		public static float radianToDegree(float angle)
		{
			return angle * GeomHelper.RAD_TO_DEG;
		}
	}
}
