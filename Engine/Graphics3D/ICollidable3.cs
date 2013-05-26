using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics3D
{
    /// <summary>
    /// An interface to collide with 3D object
    /// </summary>
    public interface ICollidable3
    {
        BoundingBox BoundingBox { get; set; }
        BoundingSphere BoundingSphere { get; set; }
    }
}
