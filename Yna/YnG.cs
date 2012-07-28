using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Yna.Display;
using Yna.Helpers;
using Yna.State;
using Yna.Input;

namespace Yna
{	
    public static class YnG
    {
        public static Game Game { get; set; }
        
        public static GraphicsDevice GraphicsDevice 
        { 
            get { return Game.GraphicsDevice; } 
        }

        public static GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        
        public static ContentManager Content 
        { 
            get { return Game.Content; } 
        }

        public static StateManager StateManager { get; set; }
		
        public static YnKeyboard Keys { get; set; }

        public static YnMouse Mouse { get; set; }
        
        public static Camera2D Camera { get; set; }

        /// <summary>
        /// Obtient la largeur de l'écran
        /// </summary>
		public static int Width 
        {
            get { return GraphicsDeviceManager.PreferredBackBufferWidth; }
            set { GraphicsDeviceManager.PreferredBackBufferWidth = value; }
        }

		/// <summary>
		/// Obtient la hauteur d'écran
		/// </summary>
		public static int Height
        {
            get { return GraphicsDeviceManager.PreferredBackBufferHeight; }
            set { GraphicsDeviceManager.PreferredBackBufferHeight = value; }
        }

        public static void SetScreenResolution(int width, int height)
        {
        	(Game as YnGame).SetScreenResolution(width, height);
        }

        public static void SwitchState(YnState state)
        {
            (Game as YnGame).SwitchState(state);
        }

        public static void Exit()
        {
            Game.Exit();
        }

        public static bool RectCollide (Sprite spriteA, Sprite spriteB)
        {
            return spriteA.Rectangle.Intersects(spriteB.Rectangle);
        }

        public static bool PerfectPixelCollide (Sprite spriteA, Sprite spriteB)
        {
            int top = Math.Max(spriteA.Rectangle.Top, spriteB.Rectangle.Top);
            int bottom = Math.Min(spriteA.Rectangle.Bottom, spriteB.Rectangle.Bottom);
            int left = Math.Max(spriteA.Rectangle.Left, spriteB.Rectangle.Left);
            int right = Math.Min(spriteA.Rectangle.Right, spriteB.Rectangle.Right);

            for (int y = top; y < bottom; y++)  // De haut en bas
            {
                for (int x = left; x < right; x++)  // de gauche à droite
                {
                    int index_A = (x - spriteA.Rectangle.Left) + (y - spriteA.Rectangle.Top) * spriteA.Rectangle.Width;
                    int index_B = (x - spriteB.Rectangle.Left) + (y - spriteB.Rectangle.Top) * spriteB.Rectangle.Width;

                    Color[] colorsSpriteA = GraphicsHelper.GetTextureData(spriteA);
                    Color[] colorsSpriteB = GraphicsHelper.GetTextureData(spriteB);

                    Color colorSpriteA = colorsSpriteA[index_A];
                    Color colorSpriteB = colorsSpriteB[index_B];

                    if (colorSpriteA.A != 0 && colorSpriteB.A != 0)
                        return true;
                }
            }
            return false;
        }
    }
}
