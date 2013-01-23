using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Framework.Display.Gui
{
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
            Orientation = YnOrientation.Vertical;
        }

        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // TODO
        }


    }
}
