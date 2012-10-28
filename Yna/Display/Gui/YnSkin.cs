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
        public YnBorder PanelBorder { get; set; }

        /// <summary>
        /// Border definitions for buttons
        /// </summary>
        public YnBorder ButtonBorder { get; set; }

        /// <summary>
        /// Border definition for hovered bos widgets
        /// </summary>
        public YnBorder HoveredButtonBorder { get; set; }

        /// <summary>
        /// Background image for box widgets
        /// </summary>
        public Texture2D PanelBackground { get; set; }

        /// <summary>
        /// Background image for buttons
        /// </summary>
        public Texture2D ButtonBackground { get; set; }

        /// <summary>
        /// Hovered background image for buttons
        /// </summary>
        public Texture2D HoveredButtonBackground { get; set; }

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

        /// <summary>
        /// The text color for clicked buttons
        /// </summary>
        public Color ClickedButtonTextColor { get; set; }

        /// <summary>
        /// Clicked background image for buttons
        /// </summary>
        public Texture2D ClickedButtonBackground { get; set; }
    }
}
