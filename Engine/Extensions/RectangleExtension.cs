using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework
{
    public static class RectangleExtension
    {
        public static Vector2 GetTopCenter(this Rectangle rectangle)
        {
            return new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y);
        }

        public static Vector2 GetBottomCenter(this Rectangle rectangle)
        {
            return new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height);
        }

        public static Vector2 GetLeftCenter(this Rectangle rectangle)
        {
            return new Vector2(rectangle.X, rectangle.Y + rectangle.Height / 2);
        }

        public static Vector2 GetRightCenter(this Rectangle rectangle)
        {
            return new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height / 2);
        }
    }
}
