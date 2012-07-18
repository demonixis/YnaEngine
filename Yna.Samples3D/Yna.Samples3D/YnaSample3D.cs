using System;
using System.Collections.Generic;
using System.Linq;
using Yna;
using Yna.Samples3D.States;

namespace Yna.Samples3D
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class YnaSample3D : YnGame
    {
        public YnaSample3D()
            : base(1280, 800, "YNA 3D Samples")
        {
            SwitchState(new SpaceGame());
        }
    }
}
