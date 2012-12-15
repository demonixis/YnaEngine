using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Yna.Framework.Display.Gui
{
    /// <summary>
    /// Container of all definitions for a given GUI theme
    /// </summary>
    public class YnSkin
    {
        #region Attributes

        protected YnBorder _panelBorder;
        protected YnBorder _buttonBorder;
        protected YnBorder _hoveredButtonBorder;
        protected Texture2D _panelBackground;
        protected Texture2D _buttonBackground;
        protected Texture2D _hoveredButtonBackground;
        protected SpriteFont _font;
        protected string _fontName;
        protected Color _defaultTextColor;
        protected Color _clickedButtonTextColor;
        protected Texture2D _clickedButtonBackground;

        #endregion

        #region Properties

        /// <summary>
        /// Border definitions for box widgets
        /// </summary>
        public YnBorder PanelBorder
        {
            get { return _panelBorder; }
            set { _panelBorder = value; }
        }

        /// <summary>
        /// Border definitions for buttons
        /// </summary>
        public YnBorder ButtonBorder
        {
            get { return _buttonBorder; }
            set { _buttonBorder = value; }
        }

        /// <summary>
        /// Border definition for hovered bos widgets
        /// </summary>
        public YnBorder HoveredButtonBorder
        {
            get { return _hoveredButtonBorder; }
            set { _hoveredButtonBorder = value; }
        }

        /// <summary>
        /// Background image for box widgets
        /// </summary>
        public Texture2D PanelBackground
        {
            get { return _panelBackground; }
            set { _panelBackground = value; }
        }

        /// <summary>
        /// Background image for buttons
        /// </summary>
        public Texture2D ButtonBackground
        {
            get { return _buttonBackground; }
            set { _buttonBackground = value; }
        }

        /// <summary>
        /// Hovered background image for buttons
        /// </summary>
        public Texture2D HoveredButtonBackground
        {
            get { return _hoveredButtonBackground; }
            set { _hoveredButtonBackground = value; }
        }

        /// <summary>
        /// The skin font
        /// </summary>
        public SpriteFont Font
        {
            get { return _font; }
            set { _font = value; }
        }

        /// <summary>
        /// The font name to use
        /// </summary>
        public string FontName
        {
            get { return _fontName; }
            set { _fontName = value; }
        }

        /// <summary>
        /// The default text color
        /// </summary>
        public Color DefaultTextColor
        {
            get { return _defaultTextColor; }
            set { _defaultTextColor = value; }
        }

        /// <summary>
        /// The text color for clicked buttons
        /// </summary>
        public Color ClickedButtonTextColor
        {
            get { return _clickedButtonTextColor; }
            set { _clickedButtonTextColor = value; }
        }

        /// <summary>
        /// Clicked background image for buttons
        /// </summary>
        public Texture2D ClickedButtonBackground
        {
            get { return _clickedButtonBackground; }
            set { _clickedButtonBackground = value; }
        }

        #endregion
    }
}
