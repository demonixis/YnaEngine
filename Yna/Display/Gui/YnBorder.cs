using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display.Gui
{
    /// <summary>
    /// Border skin definition. Contains textures for all borders
    /// </summary>
    public class YnBorder
    {
        #region Properties

        /// <summary>
        /// Top left border texture
        /// </summary>
        public Texture2D TopLeft { get; set; }

        /// <summary>
        /// Top border texture
        /// </summary>
        public Texture2D Top { get; set; }

        /// <summary>
        /// Top right border texture
        /// </summary>
        public Texture2D TopRight { get; set; }

        /// <summary>
        /// Right border texture
        /// </summary>
        public Texture2D Right { get; set; }

        /// <summary>
        /// Bottom right border texture
        /// </summary>
        public Texture2D BottomRight { get; set; }

        /// <summary>
        /// Bottom border texture
        /// </summary>
        public Texture2D Bottom { get; set; }

        /// <summary>
        /// Bottom left border texture
        /// </summary>
        public Texture2D BottomLeft { get; set; }

        /// <summary>
        /// Left border texture
        /// </summary>
        public Texture2D Left { get; set; }

        #endregion
    }
}
