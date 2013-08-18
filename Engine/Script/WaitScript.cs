// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Yna.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Script
{
    /// <summary>
    /// Wait the given amount of miliseconds. Use this script to create time gaps in
    /// your animations. Note that the time waited may not be accurate. As updates are made
    /// at a certain framerate by the game container, the wait time may go beyond it's defined
    /// value (about a few miliseconds)
    /// </summary>
    public class WaitScript : BaseScriptNode
    {
        #region Attributes

        protected int _waitTime;
        protected int _elaspedTime;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="time">The time to wait in miliseconds</param>
        public WaitScript(int time)
            : base()
        {
            _waitTime = time;
            _elaspedTime = 0;
        }

        /// <summary>
        /// Wait
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="o">Unused here</param>
        public override void Update(GameTime gameTime, YnEntity o)
        {
            _elaspedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (_elaspedTime >= _waitTime)
            {
                _scriptDone = true;
            }
        }

        /// <summary>
        /// Reset the timer
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            _elaspedTime = 0;
        }
    }
}
