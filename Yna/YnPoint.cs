using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna
{
    public class YnPoint
    {
        /// <summary>
        /// X value of the point in a 2D space
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y value of the point in a 2D space
        /// </summary>
        public int Y { get; set; }

        public YnPoint()
        {
            X = 0;
            Y = 0;
        }

        public YnPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        #region Operators overload

        public static YnPoint operator ++(YnPoint point)
        {
            return new YnPoint(point.X + 1, point.Y + 1);
        }

        public static YnPoint operator --(YnPoint point)
        {
            return new YnPoint(point.X - 1, point.Y - 1);
        }

        public static YnPoint operator +(YnPoint pointA, YnPoint pointB)
        {
            return new YnPoint(pointA.X + pointB.X, pointA.Y + pointB.Y);
        }

        public static YnPoint operator -(YnPoint pointA, YnPoint pointB)
        {
            return new YnPoint(pointA.X - pointB.X, pointA.Y - pointB.Y);
        }

        public static bool operator==(YnPoint pointA, YnPoint pointB)
        {
            return (pointA.X == pointB.X) && (pointA.Y == pointB.Y);
        }

        public static bool operator !=(YnPoint pointA, YnPoint pointB)
        {
            return (pointA.X != pointB.X) || (pointA.Y != pointB.Y);
        }

        #endregion

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        } 
    }
}
