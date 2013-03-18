using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine
{
    /// <summary>
    /// An interface for Entity 3D
    /// </summary>
    interface IDrawableEntity3D
    {
        /// <summary>
        /// Initialize logic.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Load content.
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Unload content.
        /// </summary>
        void UnloadContent();

        /// <summary>
        /// Draw on screen.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="device"></param>
        void Draw(GameTime gameTime, GraphicsDevice device);
    }
}
