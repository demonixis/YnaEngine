using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Display
{
    public class YnScene : YnGroup
    {
        public YnObject GetChildByName(string name)
        {
            YnObject result = null;
            int i = 0;
            while (i < Count && result == null)
            {
                if (Members[i].Name == name)
                    result = Members[i];

                i++;
            }
            return result;
        }
    }
}
