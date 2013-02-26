using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics.Animation
{
    /// <summary>
    /// Define a sprite animation
    /// </summary>
    public class SpriteAnimation
    {
        private int index;
        private int max;
        private int _length;
        private long _elapsedTime;
        private int _frameRate;
        private double _frameRateValue;

        public Rectangle [] Rectangle { get; set; }
        
        /// <summary>
        /// The animation framerate
        /// </summary>
        public int FrameRate
        {
        	get
        	{
        		return _frameRate;
        	}
        	set
        	{
        		_frameRate = value;
        		// This value is the amount of miliseconds between two animations
        		// 1 / _frameRate => amount of seconds per animation
        		// 1000 / _frameRate => amount of miliseconds per animation
        		_frameRateValue = 1000 / _frameRate;
        	}
       	}
        public bool Reversed { get; set; }

        public event EventHandler<EventArgs> AnimationComplete = null;
        
        /// <summary>
        /// This event is thrown just before the animation index is reset.
        /// </summary>
        public event EventHandler<EventArgs> AnimationJustComplete = null;

        public int Index 
        {
            get { return index; }
            set 
            {
                if (value < 0)
                {
                    index = 0;
                }
                else if (value >= max)
                {
                	// Call the AnimationJustComplete event before reseting the animation index
                	OnAnimationJustComplete(EventArgs.Empty);
                    index = 0;
                }
                else
                {
                    index = value;
                }
            }
        }

        public int Length 
        { 
            get { return _length; }
        }

        public SpriteAnimation(int length, int frameRate, bool reversed)
        {
            Rectangle = new Rectangle[length];
            FrameRate = frameRate;
            Reversed = reversed;
            
            index = 0;
            max = length;
            _elapsedTime = 0;
            _length = length;
        }

        public Rectangle Next(ref SpriteEffects spriteEffect, long elapsedTime)
        {
            if (Reversed)
                spriteEffect = SpriteEffects.FlipHorizontally;

            _elapsedTime += elapsedTime;

            if (_elapsedTime > _frameRateValue)
            {
                Index++;
                if (Index == 0)
                    OnAnimationComplete(EventArgs.Empty);
                
                _elapsedTime = 0;
            }

            return Rectangle[index];
        }

        private void OnAnimationComplete(EventArgs e)
        {
            if (AnimationComplete != null)
                AnimationComplete(this, e);
        }
        
        private void OnAnimationJustComplete(EventArgs e)
        {
        	if(AnimationJustComplete != null)
        		AnimationJustComplete(this, e);
        }
    }
}
