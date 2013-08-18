// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Yna.Engine.Graphics;

namespace Yna.Engine.Script
{
    /// <summary>
    /// This class manage scripts to animate a YnObject. Scripts are executed in the order 
    /// they were added in the manager. When a script is done, the next one is lunched, and
    /// so on, until the end of the script list.
    /// 
    /// Animation process can be controlled at any time to stop, restart, pause or resume the
    /// script processing.
    /// </summary>
    public class ScriptAnimator : YnBasicEntity
    {
        #region Attributes

        private List<BaseScriptNode> _scriptNodes;
        protected bool _repeatAnimation;
        protected YnEntity _target;
        protected int _nodeIndex;
        protected bool _started;
        protected bool _running;

        #endregion

        #region Properties

        /// <summary>
        /// Set to true in order to repeat the animations : when the last script is done, the
        /// manager goes back to the first one and repeat, forever.
        /// </summary>
        public bool RepeatAnimation { set { _repeatAnimation = value; } }

        #endregion

        #region Events

        /// <summary>
        /// This event is triggered when the animation is started
        /// </summary>
        public event EventHandler<EventArgs> OnStart = null;

        /// <summary>
        /// This event is triggered when the animation is stopped
        /// </summary>
        public event EventHandler<EventArgs> OnStop = null;

        /// <summary>
        /// This event is triggered when the animation is paused
        /// </summary>
        public event EventHandler<EventArgs> OnPause = null;

        /// <summary>
        /// This event is triggered when the animation is resumed
        /// </summary>
        public event EventHandler<EventArgs> OnResume = null;

        /// <summary>
        /// This event is triggered when the whole animation is done
        /// </summary>
        public event EventHandler<EventArgs> OnEnd = null;

        #endregion

        /// <summary>
        /// Create an script animator for the given YnObject
        /// </summary>
        /// <param name="target"></param>
        public ScriptAnimator(YnEntity target)
        {
            _target = target;
            _scriptNodes = new List<BaseScriptNode>();
            _started = false;
            _running = false;
            _nodeIndex = 0;
            _repeatAnimation = false;
        }

        #region Nodes management

        /// <summary>
        /// Add a script node to the animator
        /// </summary>
        /// <param name="node"></param>
        public void Add(BaseScriptNode node)
        {
            _scriptNodes.Add(node);
        }

        /// <summary>
        /// Stops the animation and removes all nodes previously added and
        /// </summary>
        public void ClearNodes()
        {
            Stop();
            _scriptNodes.Clear();
        }
        #endregion

        #region Animation controls

        /// <summary>
        /// Starts the animation at the current script
        /// </summary>
        public void Start()
        {
            if (!_started)
            {
                _started = true;
                _running = true;
            }

            if (OnStart != null)
                OnStart(this, EventArgs.Empty);
        }

        /// <summary>
        /// Pause the animation. Current animation progress is stored and may be resumed later
        /// with the Resume method.
        /// </summary>
        public void Pause()
        {
            if (_running)
            {
                _running = false;

                if (OnPause != null)
                    OnPause(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Resume the animation if it was previously paused. The animation restarts at it's
        /// last state
        /// </summary>
        public void Resume()
        {
            if (!_running)
            {
                _running = true;

                if (OnResume != null)
                    OnResume(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Totally stops the animation. If you want to continue it later, use 
        /// Pause/Resume methods instead. Stopping the animation sets the current script at 
        /// 0 (the first one)
        /// </summary>
        public void Stop()
        {
            _started = false;
            _running = false;
            _nodeIndex = 0;

            // Reset all scripts
            foreach (BaseScriptNode script in _scriptNodes)
            {
                script.Reset();
            }

            if (OnStop != null)
                OnStop(this, EventArgs.Empty);
        }

        #endregion

        /// <summary>
        /// Runs the animation scripts
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Update(GameTime gameTime)
        {
            // Execute scripts only if the animation is started and is running
            if (_started && _running)
            {
                if (_scriptNodes.Count == _nodeIndex)
                {
                    // The last path node has been reached
                    Stop();

                    if (_repeatAnimation)
                    {
                        // Here we go again!
                        Start();
                    }

                    if (OnEnd != null)
                        OnEnd(this, EventArgs.Empty);
                }
                else
                {
                    // Get the current script node
                    BaseScriptNode currentNode = _scriptNodes[_nodeIndex];

                    // Perform the scripted animation
                    currentNode.Update(gameTime, _target);

                    if (currentNode.ScriptDone)
                    {
                        // The script is done, jump to the next one
                        _nodeIndex++;
                    }
                }
            }
        }
    }
}
