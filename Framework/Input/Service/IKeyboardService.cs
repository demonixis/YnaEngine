using System;
using Microsoft.Xna.Framework.Input;

namespace Yna.Framework.Input.Service
{
    /// <summary>
    /// Interface for keyboard service
    /// </summary>
    public interface IKeyboardService
    {
        /// <summary>
        /// Test if a key is pressed
        /// </summary>
        /// <param name="key">Key to test</param>
        /// <returns>True if pressed then false</returns>
        bool Pressed(Keys key);
        
        /// <summary>
        /// Test if a key is released
        /// </summary>
        /// <param name="key">Key to test</param>
        /// <returns>True if released then false</returns>
        bool Released(Keys key);
        
        /// <summary>
        /// Test if a key is just pressed
        /// </summary>
        /// <param name="key">Key to test</param>
        /// <returns>True if just pressed then false</returns>
        bool JustPressed(Keys key);
        
        /// <summary>
        /// Test if a key is just released
        /// </summary>
        /// <param name="key">Key to test</param>
        /// <returns>True if just released then false</returns>
        bool JustReleased(Keys key);
    }
}
