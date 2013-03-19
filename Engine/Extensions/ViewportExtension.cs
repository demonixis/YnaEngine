using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Graphics
{
    public static class ViewportExtension
    {
        public static Rectangle ToRectangle(this Viewport viewport)
        {
            return new Rectangle(viewport.X, viewport.Y, viewport.Width, viewport.Height);
        }

        public static Vector2 GetCenter(this Viewport viewport)
        {
            return new Vector2(viewport.Width / 2, viewport.Height / 2);
        }
    }
}
