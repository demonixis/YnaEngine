using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics.Gui
{
    /// <summary>
    /// Container widget. Useful for multiple widget placement
    /// </summary>
    public class YnPanel : YnWidget
    {
        public YnPanel()
            : base()
        {
            _pack = true;
            _withBackground = true;
        }

        /// <summary>
        /// Does nothing. YnPanels are juste containers
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The sprite batch</param>
        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Nothing here, just draw the children in super class
        }
    }
}
