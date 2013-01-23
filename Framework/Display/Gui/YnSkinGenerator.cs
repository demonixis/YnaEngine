using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Helpers;

namespace Yna.Framework.Display.Gui
{
    public class YnSkinGenerator
    {
        /// <summary>
        /// Generates a Windows 8 like skin base on the color
        /// </summary>
        /// <param name="baseColor">The base color</param>
        /// <param name="fontName">Font name</param>
        /// <returns></returns>
        public static YnSkin Generate(Color baseColor, string fontName)
        {
            Color tempColor;
            YnSkin skin = new YnSkin();

            skin.FontName = fontName; 
            if (fontName != null)
                skin.Font = YnG.Game.Content.Load<SpriteFont>(skin.FontName);
            skin.DefaultTextColor = Color.White;

            // The base color will be used for buttons
            skin.ButtonBackground = YnGraphics.CreateTexture(baseColor, 1, 1);

            // A darker color will be used for hovered buttons
            tempColor = Add(baseColor, 50, 50, 50);
            skin.HoveredButtonBackground = YnGraphics.CreateTexture(tempColor, 1, 1);

            // Invert text / background color for clicked buttons
            skin.ClickedButtonTextColor = baseColor;
            skin.ClickedButtonBackground = YnGraphics.CreateTexture(Color.White, 1, 1);

            // Panels background will be 
            tempColor = Sub(baseColor, 150, 150, 150);
            skin.PanelBackground = YnGraphics.CreateTexture(tempColor, 1, 1);

            return skin;
        }

        private static Color ApplyRatio(Color baseColor, double r, double g, double b)
        {
            Color newColor = baseColor;
            newColor.R = (byte)((double) newColor.R * r);
            newColor.G = (byte)((double) newColor.G * g);
            newColor.B = (byte)((double) newColor.B * b);
            
            return newColor;
        }

        private static Color Add(Color baseColor, float r, float g, float b)
        {
            Color newColor = baseColor;
            newColor.R = (byte) MathHelper.Clamp(newColor.R + r, 0, 255);
            newColor.G = (byte) MathHelper.Clamp(newColor.G + g, 0, 255);
            newColor.B = (byte) MathHelper.Clamp(newColor.B + b, 0, 255);

            return newColor;
        }

        private static Color Sub(Color baseColor, float r, float g, float b)
        {
            return Add(baseColor, -r, -g, -b);
        }
    }
}
