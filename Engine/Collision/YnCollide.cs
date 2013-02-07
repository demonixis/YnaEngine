using System;
using System.Collections.Generic;
using Yna.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Collision
{
    /// <summary>
    /// Helper for collide detection
    /// </summary>
    public class YnCollide
    {
        /// <summary>
        /// Simple test collision with rectangles
        /// </summary>
        /// <param name="sceneObjectA">Sprite 1</param>
        /// <param name="sceneObjectB">Sprite 2</param>
        /// <returns></returns>
        public static bool Collide(YnEntity sceneObjectA, YnEntity sceneObjectB)
        {
            return sceneObjectA.Rectangle.Intersects(sceneObjectB.Rectangle);
        }

        public static bool CollideOneWithGroup(YnEntity sceneObject, List<YnEntity> group)
        {
            bool collide = false;
            int size = group.Count;
            int i = 0;

            while (i < size && !collide)
            {
                if (sceneObject.Rectangle.Intersects(group[i].Rectangle))
                    collide = true;

                i++;
            }

            return collide;
        }

        public static bool CollideGroupWithGroup(YnGroup groupA, YnGroup groupB)
        {
            bool collide = false;
            int i = 0;
            int j = 0;
            int groupASize = groupA.Count;
            int groupBSize = groupB.Count;

            while (i < groupASize && !collide)
            {
                while (j < groupBSize && !collide)
                {
                    if (groupA[i].Rectangle.Intersects(groupB[j].Rectangle))
                        collide = true;

                    j++;
                }
                i++;
            }

            return collide;
        }

        /// <summary>
        /// Perfect pixel test collision
        /// </summary>
        /// <param name="sceneObjectA">Sprite 1</param>
        /// <param name="sceneObjectB">Sprite 2</param>
        /// <returns></returns>
        public static bool PerfectPixelCollide(IColladable2PerfectPixel sceneObjectA, IColladable2PerfectPixel sceneObjectB)
        {
            int top = Math.Max(sceneObjectA.Rectangle.Top, sceneObjectB.Rectangle.Top);
            int bottom = Math.Min(sceneObjectA.Rectangle.Bottom, sceneObjectB.Rectangle.Bottom);
            int left = Math.Max(sceneObjectA.Rectangle.Left, sceneObjectB.Rectangle.Left);
            int right = Math.Min(sceneObjectA.Rectangle.Right, sceneObjectB.Rectangle.Right);

            for (int y = top; y < bottom; y++)  // De haut en bas
            {
                for (int x = left; x < right; x++)  // de gauche à droite
                {
                    int index_A = (x - sceneObjectA.Rectangle.Left) + (y - sceneObjectA.Rectangle.Top) * sceneObjectA.Rectangle.Width;
                    int index_B = (x - sceneObjectB.Rectangle.Left) + (y - sceneObjectB.Rectangle.Top) * sceneObjectB.Rectangle.Width;

                    Color[] colorsSpriteA = YnGraphics.GetTextureData(sceneObjectA.Texture);
                    Color[] colorsSpriteB = YnGraphics.GetTextureData(sceneObjectB.Texture);

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
