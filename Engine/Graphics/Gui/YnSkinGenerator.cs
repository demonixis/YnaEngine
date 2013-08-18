// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Helpers;

namespace Yna.Engine.Graphics.Gui
{
    /// <summary>
    /// This helper can generate flat skins based on a single color.
    /// </summary>
    public class YnSkinGenerator
    {

        /// <summary>
        /// Generates a Windows 8 like skin based on a start color.
        /// </summary>
        /// <param name="baseColor">The base color</param>
        /// <param name="fontName">Font name</param>
        /// <returns>The skin</returns>
        public static YnSkin Generate(Color baseColor, string fontName)
        {
            return Generate(baseColor, fontName, true);
        }

        /// <summary>
        /// Generates a Windows 8 like skin based on a start color.
        /// </summary>
        /// <param name="baseColor">The base color</param>
        /// <param name="fontName">Font name</param>
        /// <param name="animated">set to true to generate "Hover" and "Clicked" skins</param>
        /// <returns>The skin</returns>
        public static YnSkin Generate(Color baseColor, string fontName, bool animated)
        {
            Color tempColor;
            YnSkin skin = new YnSkin();

            // Default state
            skin.FontNameDefault = fontName;
            skin.FontDefault = YnG.Game.Content.Load<SpriteFont>(fontName);
            skin.TextColorDefault = Color.White;
            skin.BackgroundDefault = YnGraphics.CreateTexture(baseColor, 1, 1);
            skin.BorderDefault = null; // Borders are left blank

            // If the skin is "animated", generate data for "Hover" and "Clicked" states
            if (animated)
            {
                // Hover state : change only background color
                tempColor = Add(baseColor, 50, 50, 50);
                skin.BackgroundHover = YnGraphics.CreateTexture(tempColor, 1, 1);

                // Clicked state : invert background & text color
                skin.TextColorClicked = baseColor;
                skin.BackgroundClicked = YnGraphics.CreateTexture(Color.White, 1, 1);
            }

            return skin;
        }

        /// <summary>
        /// Apply color multipliers on the color RGB values. Usefull to create shades.
        /// </summary>
        /// <param name="baseColor">The base color to work with</param>
        /// <param name="r">Red multiplier</param>
        /// <param name="g">Green multiplier</param>
        /// <param name="b">Blue multiplier</param>
        /// <returns>The new color</returns>
        private static Color ApplyRatio(Color baseColor, double r, double g, double b)
        {
            Color newColor = baseColor;
            newColor.R = (byte)((double) newColor.R * r);
            newColor.G = (byte)((double) newColor.G * g);
            newColor.B = (byte)((double) newColor.B * b);
            
            return newColor;
        }
        
        /// <summary>
        /// Add RGB values to the given color. Usefull to create shades.
        /// </summary>
        /// <param name="baseColor">The base color to work with</param>
        /// <param name="r">Red value</param>
        /// <param name="g">Green value</param>
        /// <param name="b">Blue value</param>
        /// <returns>The new color</returns>
        private static Color Add(Color baseColor, float r, float g, float b)
        {
            Color newColor = baseColor;
            newColor.R = (byte) MathHelper.Clamp(newColor.R + r, 0, 255);
            newColor.G = (byte) MathHelper.Clamp(newColor.G + g, 0, 255);
            newColor.B = (byte) MathHelper.Clamp(newColor.B + b, 0, 255);

            return newColor;
        }

        /// <summary>
        /// Remove RGB values to the given color. Usefull to create shades.
        /// </summary>
        /// <param name="baseColor">The base color to work with</param>
        /// <param name="r">Red value</param>
        /// <param name="g">Green value</param>
        /// <param name="b">Blue value</param>
        /// <returns>The new color</returns>
        private static Color Sub(Color baseColor, float r, float g, float b)
        {
            return Add(baseColor, -r, -g, -b);
        }
    }
}
