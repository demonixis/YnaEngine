using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Yna.Framework.Display3D.Controls
{
    public class KeysMapping
    {
        private Dictionary<string, Keys> _keysMap;

        public Keys this[string identifier]
        {
            get { return _keysMap[identifier]; }
            set
            {
                if (_keysMap.ContainsKey(identifier))
                    _keysMap[identifier] = value;
                else
                    _keysMap.Add(identifier, value);
            }
        }

        public KeysMapping()
        {
            _keysMap = new Dictionary<string, Keys>(12);
            _keysMap.Add("Up", Keys.Up);
            _keysMap.Add("Down", Keys.Down);
            _keysMap.Add("Left", Keys.Left);
            _keysMap.Add("Right", Keys.Right);
            _keysMap.Add("A", Keys.A);
            _keysMap.Add("Z", Keys.Z);
            _keysMap.Add("E", Keys.E);
            _keysMap.Add("Q", Keys.Q);
            _keysMap.Add("S", Keys.S);
            _keysMap.Add("W", Keys.D);
            _keysMap.Add("PageUp", Keys.PageUp);
            _keysMap.Add("PageDown", Keys.PageDown);
        }
    }
}
