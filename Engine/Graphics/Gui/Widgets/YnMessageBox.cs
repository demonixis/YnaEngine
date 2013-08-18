// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Engine.Graphics.Gui.Widgets
{
    /// <summary>
    /// Work in progress
    /// </summary>
    public class YnMessageBox : YnWindow
    {
        public YnMessageBox(string title, string message)
            : base(title)
        {
            _contentPanel.Add(new YnLabel(){Text = message});
        }
    }
}
