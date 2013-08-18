// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Yna.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Script
{
    /// <summary>
    /// Base class for all script nodes.
    /// </summary>
    public abstract class BaseScriptNode
    {
        protected bool _scriptDone;

        /// <summary>
        /// Return true if the animation script is done
        /// </summary>
        public bool ScriptDone { get { return _scriptDone; } }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseScriptNode()
        {
            _scriptDone = false;
        }

        /// <summary>
        /// Perform an iteration of the script
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="o">The object to perform the script on</param>
        public abstract void Update(GameTime gameTime, YnEntity o);

        /// <summary>
        /// Reset the script to it's initial state.
        /// </summary>
        public virtual void Reset()
        {
            _scriptDone = false;
        }
    }
}
