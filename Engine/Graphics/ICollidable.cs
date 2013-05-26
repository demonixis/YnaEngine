using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics
{
    /// <summary>
    /// An interface to collide with 2D object
    /// </summary>
    public interface ICollidable2
    {
        Rectangle Rectangle { get; set; }
    }

    public interface IColladable2PerfectPixel
    {
        Vector2 Scale { get; set; }
        Texture2D Texture { get; set; }
        Rectangle Rectangle { get; set; }
    }
}
