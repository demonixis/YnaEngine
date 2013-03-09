using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Graphics
{
    public static class Texture2DExtension
    {
        public static Rectangle GetSourceRectangle(this Texture2D texture)
        {
            return new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public static Vector2 GetSize(this Texture2D texture)
        {
            return new Vector2(texture.Width, texture.Height);
        }
    }
}
