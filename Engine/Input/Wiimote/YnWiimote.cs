// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using WiimoteLib;

namespace Yna.Engine.Input
{
    /// <summary>
    /// A Wiimote/Nunchuk manager
    /// </summary>
    public class YnWiimote : IDisposable
    {
        private Wiimote _wiimote;

        /// <summary>
        /// Gets the battery level
        /// </summary>
        public byte BatteryLevel { get; private set; }

        /// <summary>
        /// Gets the status of the wiimote
        /// </summary>
        public bool IsAvailable { get; private set; }

        /// <summary>
        /// Gets the state of up button
        /// </summary>
        public bool Up { get; private set; }

        /// <summary>
        /// Gets the state of down button
        /// </summary>
        public bool Down { get; private set; }

        /// <summary>
        /// Gets the state of left button
        /// </summary>
        public bool Left { get; private set; }

        /// <summary>
        /// Gets the state of right button
        /// </summary>
        public bool Right { get; private set; }

        /// <summary>
        /// Gets the state of A button
        /// </summary>
        public bool A { get; private set; }

        /// <summary>
        /// Gets the state of B button
        /// </summary>
        public bool B { get; private set; }

        /// <summary>
        /// Gets the state of Minus button
        /// </summary>
        public bool Minus { get; private set; }

        /// <summary>
        /// Gets the state of Home button
        /// </summary>
        public bool Home { get; private set; }

        /// <summary>
        /// Gets the state of Plus button
        /// </summary>
        public bool Plus { get; private set; }

        /// <summary>
        /// Gets the state of One button
        /// </summary>
        public bool One { get; private set; }

        /// <summary>
        /// Gets the state of Two button
        /// </summary>
        public bool Two { get; private set; }

        /// <summary>
        /// Gets the position on X axis of nunchuck's joystick
        /// </summary>
        public float NunchukX { get; private set; }

        /// <summary>
        /// Gets the position on Y axis of nunchuck's joystick
        /// </summary>
        public float NunchukY { get; private set; }

        /// <summary>
        /// Gets the state of C button of nunchuk
        /// </summary>
        public bool C { get; private set; }

        /// <summary>
        /// Gets the state of Z button of nunchuk
        /// </summary>
        public bool Z { get; private set; }

        /// <summary>
        /// Construct an YnWiimote input
        /// </summary>
        public YnWiimote()
        {
     
        }

        /// <summary>
        /// Initialize the wiimote and nunchuk if it plugged
        /// </summary>
        public bool Initialize()
        {
            if (_wiimote == null)
            {
                try
                {
                    _wiimote = new Wiimote();
                    _wiimote.WiimoteChanged += wiimote_WiimoteChanged;
                    _wiimote.WiimoteExtensionChanged += wiimote_WiimoteExtensionChanged;
                    _wiimote.Connect();
                    _wiimote.SetReportType(InputReport.IRAccel, true);
                    _wiimote.SetLEDs(true, false, false, true);
                    IsAvailable = true;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    _wiimote = null;
                    IsAvailable = false;
                }
            }

            return IsAvailable;
        }

        /// <summary>
        /// Dispose and destroy the Wiimote manager
        /// </summary>
        public void Dispose()
        {
            if (_wiimote != null)
            {
                try
                {
                    _wiimote.WiimoteChanged -= wiimote_WiimoteChanged;
                    _wiimote.WiimoteExtensionChanged -= wiimote_WiimoteExtensionChanged;
                    _wiimote.Disconnect();
                    _wiimote.Dispose();
                    _wiimote = null;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    IsAvailable = false;
                }
            }
        }

        /// <summary>
        /// Enable or disable rumble.
        /// </summary>
        /// <param name="enabled">Sets to true for enable rumble.</param>
        public void SetRumble(bool enabled)
        {
            if (IsAvailable)
                _wiimote.SetRumble(enabled);
        }

        private void wiimote_WiimoteExtensionChanged(object sender, WiimoteExtensionChangedEventArgs e)
        {
            if (e.Inserted)
                _wiimote.SetReportType(InputReport.IRExtensionAccel, true);
            else
                _wiimote.SetReportType(InputReport.IRAccel, true);
        }

        private void wiimote_WiimoteChanged(object sender, WiimoteChangedEventArgs e)
        {
            WiimoteState state = e.WiimoteState;

            BatteryLevel = state.Battery;
            
            A = state.ButtonState.A;
            B = state.ButtonState.B;
            Home = state.ButtonState.Home;
            Minus = state.ButtonState.Minus;
            Plus = state.ButtonState.Plus;
            One = state.ButtonState.One;
            Two = state.ButtonState.Two;

            Up = state.ButtonState.Up;
            Down = state.ButtonState.Down;
            Left = state.ButtonState.Left;
            Right = state.ButtonState.Right;

            // Nunchuck
            NunchukState nunchuk = state.NunchukState;
            C = nunchuk.C;
            Z = nunchuk.Z;
            NunchukX = nunchuk.Joystick.X;
            NunchukY = nunchuk.Joystick.Y;
        }
    }
}