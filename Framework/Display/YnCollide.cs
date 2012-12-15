using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Yna.Framework.Display
{
    public class YnCollide
    {
        /// <summary>
        /// Simple test collision with rectangles
        /// </summary>
        /// <param name="sceneObjectA">Sprite 1</param>
        /// <param name="sceneObjectB">Sprite 2</param>
        /// <returns></returns>
        public static bool Collide(YnObject sceneObjectA, YnObject sceneObjectB)
        {
            return sceneObjectA.Rectangle.Intersects(sceneObjectB.Rectangle);
        }

        public static bool CollideOneWithGroup(YnObject sceneObject, List<YnObject> group)
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
        public static bool PerfectPixelCollide(YnObject sceneObjectA, YnObject sceneObjectB)
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

                    Color[] colorsSpriteA = Yna.Framework.Helpers.GraphicsHelper.GetTextureData(sceneObjectA);
                    Color[] colorsSpriteB = Yna.Framework.Helpers.GraphicsHelper.GetTextureData(sceneObjectB);

                    Color colorSpriteA = colorsSpriteA[index_A];
                    Color colorSpriteB = colorsSpriteB[index_B];

                    if (colorSpriteA.A != 0 && colorSpriteB.A != 0)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Optimised perfect collide test
        /// </summary>
        /// <param name="sceneObjectA">Sprite 1</param>
        /// <param name="sceneObjectB">Sprite 2</param>
        /// <returns></returns>
        public static bool PerfectCollide(YnObject sceneObjectA, YnObject sceneObjectB)
        {
            return Collide(sceneObjectA, sceneObjectB) && PerfectPixelCollide(sceneObjectA, sceneObjectB);
        }
    }
}
