// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics.Animation
{
    /// <summary>
    /// Define a sprite animation
    /// </summary>
    public class SpriteAnimation
    {
        private int _index;
        private int _length;
        private long _elapsedTime;
        private int _frameRate;
        private double _frameRateValue;

        /// <summary>
        /// Gets or sets the rectangle that compose an animation.
        /// </summary>
        public Rectangle [] Rectangle { get; set; }
        
        /// <summary>
        /// Gets or sets the animation framerate
        /// </summary>
        public int FrameRate
        {
        	get	{ return _frameRate; }
        	set
        	{
        		_frameRate = value;
        		// This value is the amount of miliseconds between two animations
        		// 1 / _frameRate => amount of seconds per animation
        		// 1000 / _frameRate => amount of miliseconds per animation
				if (_frameRate > 0)
        			_frameRateValue = 1000 / _frameRate;
				else
					_frameRateValue = 0;
        	}
       	}

        /// <summary>
        /// Reverse the animation
        /// </summary>
        public bool Reversed { get; set; }

        /// <summary>
        /// Gets the current index of the animation.
        /// </summary>
        public int Index 
        {
            get { return _index; }
            protected set 
            {
                if (value < 0)
                {
                    _index = 0;
                }
                else if (value >= _length)
                {
                	OnAnimationJustComplete(EventArgs.Empty);
                    _index = 0;
                }
                else
                {
                    _index = value;
                }
            }
        }

        public int Count 
        { 
            get { return _length; }
        }

        /// <summary>
        /// Triggered when all frames are played.
        /// </summary>
        public event EventHandler<EventArgs> AnimationComplete = null;

        /// <summary>
        /// This event is thrown just before the animation index is reset.
        /// </summary>
        public event EventHandler<EventArgs> AnimationJustComplete = null;

        private void OnAnimationComplete(EventArgs e)
        {
            if (AnimationComplete != null)
                AnimationComplete(this, e);
        }

        private void OnAnimationJustComplete(EventArgs e)
        {
            if (AnimationJustComplete != null)
                AnimationJustComplete(this, e);
        }

        /// <summary>
        /// Create a sprite animation with a length, a framerate and an option that determine if this animation
        /// must reverse the texture.
        /// </summary>
        /// <param name="length">Size of animation.</param>
        /// <param name="frameRate">Desired framerate</param>
        /// <param name="reversed">Set to true to reverse the animation</param>
        public SpriteAnimation(int length, int frameRate, bool reversed)
        {
            Rectangle = new Rectangle[length];
            FrameRate = frameRate;
            Reversed = reversed;
            
            _index = 0;
            _elapsedTime = 0;
            _length = length;
        }

        /// <summary>
        /// Gets the next animation frame
        /// </summary>
        /// <param name="spriteEffect">SpriteEffects to use for reverse the sprite.</param>
        /// <returns></returns>
        public Rectangle Next(ref SpriteEffects spriteEffect)
        {
            if (Reversed)
                spriteEffect = SpriteEffects.FlipHorizontally;

            return Rectangle[Index];
        }

        /// <summary>
        /// Update frame animation index.
        /// </summary>
        /// <param name="gameTime">GameTime object</param>
        public void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (_elapsedTime > _frameRateValue)
            {
                Index++;
                if (Index == 0)
                    OnAnimationComplete(EventArgs.Empty);
                _elapsedTime = 0;
            }
        }
    }
}
