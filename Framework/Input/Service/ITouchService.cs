using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Framework.Input.Service
{
    public interface ITouchService
    {
        bool Pressed(int id);
        bool Released(int id);
        bool Moved(int id);
    }
}
