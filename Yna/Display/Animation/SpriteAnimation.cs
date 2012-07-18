﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display.Animation
{
    public class SpriteAnimation
    {
        public Rectangle [] Rectangle { get; set; }
        public int FrameRate { get; set; }
        public bool Reversed { get; set; }
        public int Index 
        {
            get { return index; }
            set 
            {
                if (value < 0)
                    index = 0;
                else if (value >= max)
                    index = 0;
                else
                    index = value;
            }
        }
        
        private int index;
        private int max;
        private long _elapsedTime;

        public SpriteAnimation(int length, int frameRate, bool reversed)
        {
            Rectangle = new Rectangle[length];
            FrameRate = frameRate;
            Reversed = reversed;
            
            index = 0;
            max = length;
            _elapsedTime = 0;
        }

        public Rectangle Next(ref SpriteEffects spriteEffect, long elapsedTime)
        {
            if (Reversed)
                spriteEffect = SpriteEffects.FlipHorizontally;

            _elapsedTime += elapsedTime;

            if (_elapsedTime > FrameRate)
            {
                Index++;
                _elapsedTime = 0;
            }

            return Rectangle[index];
        }
    }
}
