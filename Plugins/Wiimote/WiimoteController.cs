using System;
using System.Collections.Generic;
using System.Drawing;
using WiimoteLib;

namespace Yna.Input.Wiimote
{
    public class WiimoteController
    {
        private Wiimote wiimote;

        public byte BatteryLevel;

        private bool _isAvailable;

        public bool IsAvailable
        {
            get { return _isAvailable; }
            protected set { _isAvailable = value; }
        }

        #region Wiimote buttons
        public bool Up = false;
        public bool Down = false;
        public bool Left = false;
        public bool Right = false;

        public bool A = false;
        public bool B = false;
        public bool Minus = false;
        public bool Home = false;
        public bool Plus = false;
        public bool One = false;
        public bool Two = false;
        #endregion

        #region Nunchuck buttons / Joystick
        public bool C;
        public float NunchukX;
        public float NunchukY;
        #endregion

        public WiimoteController()
        {
            try
            {
                wiimote = new Wiimote();
                wiimote.WiimoteChanged += new EventHandler<WiimoteChangedEventArgs>(wiimote_WiimoteChanged);
                wiimote.WiimoteExtensionChanged += new EventHandler<WiimoteExtensionChangedEventArgs>(wiimote_WiimoteExtensionChanged);
                wiimote.Connect();
                wiimote.SetReportType(InputReport.IRAccel, true);
                wiimote.SetLEDs(true, false, false, true);
                _isAvailable = true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());

                _isAvailable = false;
            }
        }

        void wiimote_WiimoteExtensionChanged(object sender, WiimoteExtensionChangedEventArgs e)
        {
            if (e.Inserted)
                wiimote.SetReportType(InputReport.IRExtensionAccel, true);
            else
                wiimote.SetReportType(InputReport.IRAccel, true);
        }

        void wiimote_WiimoteChanged(object sender, WiimoteChangedEventArgs e)
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
            NunchukState nunchuck = state.NunchukState;
            C = nunchuck.C;
            NunchukX = nunchuck.Joystick.X;
            NunchukY = nunchuck.Joystick.Y;
        }  
    }
}