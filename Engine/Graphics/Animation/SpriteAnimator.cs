﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics.Animation
{
    /// <summary>
    /// Sprite animator class used in Sprite class for animating a sprite with a spritesheet
    /// </summary>
    public class SpriteAnimator
    {
        public Dictionary<string, SpriteAnimation> Animations { get; set; }
        public int SpriteWidth { get; set; }
        public int SpriteHeight { get; set; }
        public int TextureWidth { get; set; }
        public int TextureHeight { get; set; }

        private int nbSpriteX;
        private int nbSpriteY;
        private int spritesheetLenght;

        public SpriteAnimator()
        {
            SpriteWidth = 0;
            SpriteHeight = 0;
            TextureWidth = 0;
            TextureHeight = 0;

            nbSpriteX = 0;
            nbSpriteY = 0;
            spritesheetLenght = 0;

            Animations = new Dictionary<string, SpriteAnimation>();
        }

        public void Initialize(int animationWidth, int animationHeight, int textureWidth, int textureHeight)
        {
            Animations.Clear();

            SpriteWidth = animationWidth;
            SpriteHeight = animationHeight;
            TextureWidth = textureWidth;
            TextureHeight = textureHeight;

            nbSpriteX = TextureWidth / SpriteWidth;
            nbSpriteY = TextureHeight / SpriteHeight;
            spritesheetLenght = nbSpriteX * nbSpriteY;
        }

        public void Add(string name, int[] indexes, int frameRate, bool reversed)
        {
            int animationLength = indexes.Length;

            SpriteAnimation animation = new SpriteAnimation(animationLength, frameRate, reversed);

            for (int i = 0; i < animationLength; i++)
            {
                int x = indexes[i] % nbSpriteX;
                int y = (int)Math.Floor((double)(indexes[i] / nbSpriteX));

                // Adapter la valeur X suivant le numéro de ligne
                // Exemple : Pour Y = 2, on doit faire repartir X à [0... tailleX]
                if (y > 0)
                    x = x % (nbSpriteX * y);
                else
                    x = x % nbSpriteX;

                // Définition du rectangle source du sprite
                animation.Rectangle[i] = new Rectangle(x * SpriteWidth, y * SpriteHeight, SpriteWidth, SpriteHeight);
            }

            Animations.Add(name, animation);
        }

        public void Add(string name, Rectangle[] rectangles, int frameRate, bool reversed)
        {
            int animationLength = rectangles.Length;

            SpriteAnimation animation = new SpriteAnimation(animationLength, frameRate, reversed);

            animation.Rectangle = rectangles;

            Animations.Add(name, animation);
        }

        public void Update(GameTime gameTime, Vector2 currentPosition, Vector2 lastPosition)
        {

        }

        public void Update(GameTime gameTime, Vector2 lastDistance)
        {

        }

        public static int[] GetIndexXY(int searchedIndex, int nbSpriteX)
        {
            int[] coordsXY = new int[2];

            coordsXY[0] = searchedIndex % nbSpriteX;
            coordsXY[1] = (int)Math.Floor((double)(searchedIndex / nbSpriteX));

            return coordsXY;
        }
    }
}