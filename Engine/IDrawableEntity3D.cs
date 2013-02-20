using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine
{
    /// <summary>
    /// An interface for Entity 3D
    /// </summary>
    interface IDrawableEntity3D
    {
        void Draw(GameTime gameTime, GraphicsDevice device);
    }
}
