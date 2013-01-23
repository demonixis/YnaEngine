using System;
using Microsoft.Xna.Framework;

namespace Yna.Framework.Input.Service
{
    /// <summary>
    /// Interface for a touch service
    /// </summary>
    public interface ITouchService
    {
        /// <summary>
        /// Test if a finger has pressed the screen
        /// </summary>
        /// <param name="id">Finger id</param>
        /// <returns>True if pressed then false</returns>
        bool Pressed(int id);

        /// <summary>
        /// Test if a finger has just pressed the screen
        /// </summary>
        /// <param name="id">Finger id</param>
        /// <returns>True if just pressed then false</returns>
        bool JustPressed(int id);

        /// <summary>
        /// Test if a finger has released the screen
        /// </summary>
        /// <param name="id">Finger id</param>
        /// <returns>True if released then false</returns>
        bool Released(int id);

        /// <summary>
        /// Test if a finger has just released the screen
        /// </summary>
        /// <param name="id">Finger id</param>
        /// <returns>True if just released then false</returns>
        bool JustReleased(int id);

        /// <summary>
        /// Test if a finger is moving
        /// </summary>
        /// <param name="id">Finger id</param>
        /// <returns>True if moving then false</returns>
        bool Moving(int id);

        /// <summary>
        /// Test if a finger has moved
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Moved(int id);
        
        /// <summary>
        /// Get the pressure level of a touch
        /// </summary>
        /// <param name="id">Finger id</param>
        /// <returns>Value between 0.0f and 1.0f</returns>
        float GetPressureLevel(int id);
        
        /// <summary>
        /// Get the position of a finger
        /// </summary>
        /// <param name="id">Finger id</param>
        /// <returns>Position of the finger</returns>
        Vector2 GetPosition(int id);

        /// <summary>
        /// Get the last position of a finger
        /// </summary>
        /// <param name="id">Finger id</param>
        /// <returns>Last position of the finger</returns>
        Vector2 GetLastPosition(int id);

        /// <summary>
        /// Get the direction of a finger
        /// </summary>
        /// <param name="id">Finger id</param>
        /// <returns>Direction of the finger</returns>
        Vector2 GetDirection(int id);

        /// <summary>
        /// Get the last direction of a finger
        /// </summary>
        /// <param name="id">Finger id</param>
        /// <returns>Last direction of the finger</returns>
        Vector2 GetLastDirection(int id);
    }
}
