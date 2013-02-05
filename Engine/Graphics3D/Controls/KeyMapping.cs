using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Yna.Engine.Graphics3D.Controls
{
    /// <summary>
    /// A key mapper used with camera control.
    /// </summary>
    public class KeysMapper
    {
        private Dictionary<string, Keys[]> _keysMap;

        /// <summary>
        /// Gets mapped keys
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Keys[] this[string key]
        {
            get { return _keysMap[key]; }
        }

        /// <summary>
        /// Gets or sets mapped keys for Up
        /// </summary>
        public Keys[] Up
        {
            get { return _keysMap["Up"]; }
            set
            {
                if (value.Length > 0)
                    _keysMap["Up"] = value;
            }
        }

        /// <summary>
        /// Gets or sets mapped keys for Down
        /// </summary>
        public Keys[] Down
        {
            get { return _keysMap["Down"]; }
            set
            {
                if (value.Length > 0)
                    _keysMap["Down"] = value;
            }
        }

        /// <summary>
        /// Gets or sets mapped keys for Left
        /// </summary>
        public Keys[] Left
        {
            get { return _keysMap["Left"]; }
            set
            {
                if (value.Length > 0)
                    _keysMap["Left"] = value;
            }
        }

        /// <summary>
        /// Gets or sets mapped keys for Right
        /// </summary>
        public Keys[] Right
        {
            get { return _keysMap["Right"]; }
            set
            {
                if (value.Length > 0)
                    _keysMap["Right"] = value;
            }
        }

        /// <summary>
        /// Gets or sets mapped keys for Strafe left
        /// </summary>
        public Keys[] StrafeLeft
        {
            get { return _keysMap["StrafeLeft"]; }
            set
            {
                if (value.Length > 0)
                    _keysMap["StrafeLeft"] = value;
            }
        }

        /// <summary>
        /// Gets or sets mapped keys for Strafe right
        /// </summary>
        public Keys[] StrafeRight
        {
            get { return _keysMap["StrafeRight"]; }
            set
            {
                if (value.Length > 0)
                    _keysMap["StrafeRight"] = value;
            }
        }

        /// <summary>
        /// Gets or sets mapped keys for Move up
        /// </summary>
        public Keys[] MoveUp
        {
            get { return _keysMap["MoveUp"]; }
            set
            {
                if (value.Length > 0)
                    _keysMap["MoveUp"] = value;
            }
        }

        /// <summary>
        /// Gets or sets mapped keys for Move down
        /// </summary>
        public Keys[] MoveDown
        {
            get { return _keysMap["MoveDown"]; }
            set
            {
                if (value.Length > 0)
                    _keysMap["MoveDown"] = value;
            }
        }

        /// <summary>
        /// Gets or sets mapped keys for Pitch up
        /// </summary>
        public Keys[] PitchUp
        {
            get { return _keysMap["PitchUp"]; }
            set
            {
                if (value.Length > 0)
                    _keysMap["PitchUp"] = value;
            }
        }

        /// <summary>
        /// Gets or sets mapped keys for Pitch down
        /// </summary>
        public Keys[] PitchDown
        {
            get { return _keysMap["PitchDown"]; }
            set
            {
                if (value.Length > 0)
                    _keysMap["PitchDown"] = value;
            }
        }

        /// <summary>
        /// Gets or sets mapped keys for Roll up
        /// </summary>
        public Keys[] RollUp
        {
            get { return _keysMap["RollUp"]; }
            set
            {
                if (value.Length > 0)
                    _keysMap["RollUp"] = value;
            }
        }

        /// <summary>
        /// Gets or sets mapped keys for Roll down
        /// </summary>
        public Keys[] RollDown
        {
            get { return _keysMap["RollDown"]; }
            set
            {
                if (value.Length > 0)
                    _keysMap["RollDown"] = value;
            }
        }

        /// <summary>
        /// Gets or sets mapped keys for Jump
        /// </summary>
        public Keys[] Jump
        {
            get { return _keysMap["Jump"]; }
            set
            {
                if (value.Length > 0)
                    _keysMap["Jump"] = value;
            }
        }

        /// <summary>
        /// Gets or sets mapped keys for Escape
        /// </summary>
        public Keys[] Escape
        {
            get { return _keysMap["Escape"]; }
            set
            {
                if (value.Length > 0)
                    _keysMap["Escape"] = value;
            }
        }

        public KeysMapper()
        {
            string culture = System.Globalization.CultureInfo.CurrentCulture.Name;

            _keysMap = new Dictionary<string, Keys[]>(14);
            _keysMap.Add("Up", new Keys[] { Keys.Up, Keys.Z });
            _keysMap.Add("Down", new Keys[] { Keys.Down, Keys.S });
            _keysMap.Add("Left", new Keys[] { Keys.Left });
            _keysMap.Add("Right", new Keys[] { Keys.Right });
            _keysMap.Add("StrafeLeft", new Keys[] { Keys.Q });
            _keysMap.Add("StrafeRight", new Keys[] { Keys.D });
            _keysMap.Add("MoveUp", new Keys[] { Keys.A });
            _keysMap.Add("MoveDown", new Keys[] { Keys.E });
            _keysMap.Add("PitchUp", new Keys[] { Keys.PageUp });
            _keysMap.Add("PitchDown", new Keys[] { Keys.PageDown });
            _keysMap.Add("RollUp", new Keys[] { Keys.W });
            _keysMap.Add("RollDown", new Keys[] { Keys.X });
            _keysMap.Add("Jump", new Keys[] { Keys.Space });
            _keysMap.Add("Escape", new Keys[] { Keys.Escape });

            // TODO : Find a better solution for detecting Qwerty keyboard ;)
            if (culture.Contains("en"))
            {
                _keysMap["Up"][1] = Keys.W;
                _keysMap["StrafeLeft"][0] = Keys.A;
                _keysMap["MoveUp"][0] = Keys.Q;
                _keysMap["RollUp"][0] = Keys.Z;

            }
        }
    }
}
