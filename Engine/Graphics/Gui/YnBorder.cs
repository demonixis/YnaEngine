// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics.Gui
{
    /// <summary>
    /// Border skin definition. Contains textures for all borders.
    /// </summary>
    public class YnBorder
    {
        #region Attributes

        protected Texture2D _topLeft;
        protected Texture2D _top;
        protected Texture2D _topRight;
        protected Texture2D _right;
        protected Texture2D _bottomRight;
        protected Texture2D _bottom;
        protected Texture2D _bottomLeft;
        protected Texture2D _left;

        #endregion

        #region Properties

        /// <summary>
        /// Top left border texture
        /// </summary>
        public Texture2D TopLeft
        {
            get { return _topLeft; }
            set { _topLeft = value; }
        }

        /// <summary>
        /// Top border texture
        /// </summary>
        public Texture2D Top
        {
            get { return _top; }
            set { _top = value; }
        }

        /// <summary>
        /// Top right border texture
        /// </summary>
        public Texture2D TopRight
        {
            get { return _topRight; }
            set { _topRight = value; }
        }

        /// <summary>
        /// Right border texture
        /// </summary>
        public Texture2D Right 
        {
            get { return _right; }
            set { _right = value; }
        }

        /// <summary>
        /// Bottom right border texture
        /// </summary>
        public Texture2D BottomRight 
        {
            get { return _bottomRight; }
            set { _bottomRight = value; }
        }

        /// <summary>
        /// Bottom border texture
        /// </summary>
        public Texture2D Bottom 
        {
            get { return _bottom; }
            set { _bottom = value; }
        }

        /// <summary>
        /// Bottom left border texture
        /// </summary>
        public Texture2D BottomLeft 
        {
            get { return _bottomLeft; }
            set { _bottomLeft = value; }
        }

        /// <summary>
        /// Left border texture
        /// </summary>
        public Texture2D Left
        {
            get { return _left; }
            set { _left = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public YnBorder()
        {
            // Nothing here
        }

        /// <summary>
        /// Create a flat border definition based on the given color and
        /// border size.
        /// </summary>
        /// <param name="borderColor">The border color</param>
        /// <param name="thickness">The border thickness</param>
        public YnBorder(Color borderColor, int thickness)
        {
            // Corners
            Texture2D cornerTexture = YnGraphics.CreateTexture(borderColor, thickness, thickness);
            TopLeft = cornerTexture;
            TopRight = cornerTexture;
            BottomRight = cornerTexture;
            BottomLeft = cornerTexture;

            // Horizontal
            Texture2D hTexture = YnGraphics.CreateTexture(borderColor, 1, thickness);
            Top = hTexture;
            Bottom = hTexture;

            // Vertical
            Texture2D vTexture = YnGraphics.CreateTexture(borderColor, thickness, 1);
            Left = vTexture;
            Right = vTexture;

        }

        #endregion
    }
}
