using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Yna.Display.Gui
{
    /// <summary>
    /// Container of all definitions for a given GUI theme
    /// </summary>
    public class YnSkin
    {
        /// <summary>
        /// Border definitions for box widgets
        /// </summary>
        public YnBorder BoxBorder { get; set; }

        /// <summary>
        /// Background image for box widgets
        /// </summary>
        public Texture2D BoxBackground { get; set; }

        /// <summary>
        /// The skin font
        /// </summary>
        public SpriteFont Font { get; set; }

        /// <summary>
        /// The font name to use
        /// </summary>
        public string FontName { get; set; }

        /// <summary>
        /// The default text color
        /// </summary>
        public Color DefaultTextColor { get; set; }
    }
}
