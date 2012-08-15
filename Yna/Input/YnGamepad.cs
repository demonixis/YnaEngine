using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Helpers;
using Yna.Input;
using Yna.Input.Service;

namespace Yna.Input
{
    public class YnGamepad
    {
        private IGamepadService service;
        
        public YnGamepad() 
        {
            service = ServiceHelper.Get<IGamepadService>();
        }
    }
}
