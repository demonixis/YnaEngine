using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Yna.Framework.Input.Service
{
    /// <summary>
    /// Interface for a mouse service
    /// </summary>
    public interface IMouseService
    {
        /// <summary>
        /// Gets the X position of the mouse cursor
        /// </summary>
        int X { get; }
        
        /// <summary>
        /// Gets the Y position of the mouse cursor
        /// </summary>
        int Y { get; }
       
        /// <summary>
        /// Gets the value of the middle button of the mouse
        /// </summary>
        int Wheel { get; }
        
        /// <summary>
        /// Gets the state of the mouse, true if moving then false
        /// </summary>
        bool Moving { get; }

        /// <summary>
        /// Gets the state of the left button
        /// </summary>
        /// <param name="state">Pressed or released</param>
        /// <returns>True if the current state is equal to the button state then false</returns>
        bool ClickLeft(ButtonState state);

        /// <summary>
        /// Gets the state of the right button
        /// </summary>
        /// <param name="state">Pressed or released</param>
        /// <returns>True if the current state is equal to the button state then false</returns>
        bool ClickRight(ButtonState state);

        /// <summary>
        /// Gets the state of the middle button
        /// </summary>
        /// <param name="state">Pressed or released</param>
        /// <returns>True if the current state is equal to the button state then false</returns>
        bool ClickMiddle(ButtonState state);

        /// <summary>
        /// Indicates if the mouse button has been just clicked
        /// </summary>
        /// <param name="state">Pressed or released</param>
        /// <returns>True if the current state is equal to the button state then false</returns>
        bool JustClicked(MouseButton button);

        /// <summary>
        /// Indicates if the mouse button has been just released
        /// </summary>
        /// <param name="state">Pressed or released</param>
        /// <returns>True if the current state is equal to the button state then false</returns>
        bool JustReleased(MouseButton button);

        /// <summary>
        /// Get the delta of displacement
        /// </summary>
        /// <returns>Delta of displacement</returns>
        Vector2 GetDelta();
    }
}
