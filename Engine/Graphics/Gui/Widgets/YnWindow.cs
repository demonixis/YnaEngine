// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics.Gui.Widgets
{
    /// <summary>
    /// Work in progress
    /// </summary>
    public class YnWindow : YnWidget
    {
        private string _title;

        private YnPanel _titlePanel;
        protected YnPanel _contentPanel;

        public YnWindow(string title)
            : base()
        {
            _title = title;
            _titlePanel = Add(new YnPanel());
            _titlePanel.Padding = 2;
            _titlePanel.Add(new YnLabel() { Text = title});
            _contentPanel = Add(new YnPanel());
            _contentPanel.Padding = 10;
        }

        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            // TODO
        }


    }
}
