using Microsoft.Xna.Framework;

namespace Yna.Engine.Collision
{
    /// <summary>
    /// An interface to collide with 2D object
    /// </summary>
    public interface ICollidable2
    {
        Rectangle Rectangle { get; set; }
    }

    /// <summary>
    /// An interface to collide with 3D object
    /// </summary>
    public interface ICollidable3
    {
        BoundingBox BoundingBox { get; set; }
        BoundingSphere BoundingSphere { get; set; }
    }
}
