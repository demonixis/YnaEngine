using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Yna.Framework.Input.Service
{
    /// <summary>
    /// Interface for a gamepad service
    /// </summary>
    public interface IGamepadService
    {
        /// <summary>
        /// Define if the controller is connected
        /// </summary>
        /// <param name="index">Id of gamepad</param>
        /// <returns>True if connected then false</returns>
        bool Connected(PlayerIndex index);

        /// <summary>
        /// Test if a button is pressed
        /// </summary>
        /// <param name="index">Id of gamepad</param>
        /// <param name="button">Button to test</param>
        /// <returns>True if pressed then false</returns>
        bool Pressed(PlayerIndex index, Buttons button);

        /// <summary>
        /// Test if a button is released
        /// </summary>
        /// <param name="index">Id of gamepad</param>
        /// <param name="button">Button to test</param>
        /// <returns>True if released then false</returns>
        bool Released(PlayerIndex index, Buttons button);

        /// <summary>
        /// Test if a button is just pressed
        /// </summary>
        /// <param name="index">Id of gamepad</param>
        /// <param name="button">Button to test</param>
        /// <returns>True if just pressed then false</returns>
        bool JustPressed(PlayerIndex index, Buttons button);

        /// <summary>
        /// Test if a button is just released
        /// </summary>
        /// <param name="index">Id of gamepad</param>
        /// <param name="button">Button to test</param>
        /// <returns>True if just released then false</returns>
        bool JustReleased(PlayerIndex index, Buttons button);

        /// <summary>
        /// Gets the pressure of a trigger
        /// </summary>
        /// <param name="index">Id of gamepad</param>
        /// <param name="left">true for left trigger, false for right trigger</param>
        /// <returns>Value of pressure on trigger button</returns>
        float Triggers(PlayerIndex index, bool left);

        /// <summary>
        /// Gets the position of the thumbstick
        /// </summary>
        /// <param name="index">Id of the gamepad</param>
        /// <param name="left">true for left thumbstick, false for right thumbstick</param>
        /// <returns></returns>
        Vector2 ThumbSticks(PlayerIndex index, bool left);
    }
}
