using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Engine.Graphics.Gui
{
    public class YnMessageBox : YnWindow
    {
        public YnMessageBox(string title, string message)
            : base(title)
        {
            _contentPanel.Add(new YnLabel(){Text = message});
        }
    }
}
