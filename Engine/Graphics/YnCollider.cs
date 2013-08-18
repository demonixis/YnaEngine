// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics
{
    /// <summary>
    /// Helper for collide detection
    /// </summary>
    public class YnCollider
    {
        /// <summary>
        /// Test if two entities collinding
        /// </summary>
        /// <param name="entityA">Sprite 1</param>
        /// <param name="entityB">Sprite 2</param>
        /// <returns></returns>
        public static bool Collide(YnEntity entityA, YnEntity entityB)
        {
            Rectangle r1 = new Rectangle((int)(entityA.X - entityA.Origin.X), (int)(entityA.Y - entityA.Origin.Y), (int)entityA.ScaledWidth, (int)entityA.ScaledHeight);
            Rectangle r2 = new Rectangle((int)(entityB.X - entityB.Origin.X), (int)(entityB.Y - entityB.Origin.Y), (int)entityB.ScaledWidth, (int)entityB.ScaledHeight);
            return r1.Intersects(r2);
        }

        /// <summary>
        /// Test if an entity and a group of entities are colliding.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static bool CollideOneWithGroup(YnEntity entity, List<YnEntity> group)
        {
            bool collide = false;
            int size = group.Count;
            int i = 0;

            while (i < size && !collide)
            {
                collide = Collide(entity, group[i]);
                i++;
            }

            return collide;
        }

        /// <summary>
        /// Test if a group of entities is collinding another group of entities.
        /// </summary>
        /// <param name="groupA"></param>
        /// <param name="groupB"></param>
        /// <returns></returns>
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
                    collide = Collide(groupA[i], groupB[j]);
                    j++;
                }
                i++;
            }

            return collide;
        }

        /// <summary>
        /// Perfect pixel test collision
        /// </summary>
        /// <param name="entityA">Sprite 1</param>
        /// <param name="entityB">Sprite 2</param>
        /// <returns></returns>
        public static bool PerfectPixelCollide(IColladable2PerfectPixel entityA, IColladable2PerfectPixel entityB)
        {
            int top = Math.Max(entityA.Rectangle.Top, entityB.Rectangle.Top);
            int bottom = Math.Min(entityA.Rectangle.Bottom, entityB.Rectangle.Bottom);
            int left = Math.Max(entityA.Rectangle.Left, entityB.Rectangle.Left);
            int right = Math.Min(entityA.Rectangle.Right, entityB.Rectangle.Right);

            for (int y = top; y < bottom; y++)  // De haut en bas
            {
                for (int x = left; x < right; x++)  // de gauche à droite
                {
                    int index_A = (x - entityA.Rectangle.Left) + (y - entityA.Rectangle.Top) * entityA.Rectangle.Width;
                    int index_B = (x - entityB.Rectangle.Left) + (y - entityB.Rectangle.Top) * entityB.Rectangle.Width;

                    Color[] colorsSpriteA = YnGraphics.GetTextureColors(entityA.Texture);
                    Color[] colorsSpriteB = YnGraphics.GetTextureColors(entityB.Texture);

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
