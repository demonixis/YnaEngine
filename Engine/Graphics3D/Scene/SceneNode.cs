using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Scene;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Camera;

namespace Yna.Engine.Graphics3D.Scene
{
    /// <summary>
    /// A Node class used with a SceneGraph.
    /// </summary>
    public class SceneNode
    {
        protected YnTransform _tranform;
        protected List<SceneNode> _children;
        protected SceneNode _parentNode;
        protected bool _enabled;
        protected bool _visible;

        /// <summary>
        /// Gets or sets transforms.
        /// </summary>
        public YnTransform Transform
        {
            get { return _tranform; }
            set { _tranform = value; }
        }

        /// <summary>
        /// Enable or disable the node.
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
        }

        /// <summary>
        /// Gets or sets the node visible.
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
        }

        /// <summary>
        /// Create a new Node without parent
        /// </summary>
        public SceneNode()
        {
            _tranform = new YnTransform();
            _parentNode = null;
            Initialize();
        }

        /// <summary>
        /// Create a new Node with a parent node
        /// </summary>
        /// <param name="parent"></param>
        public SceneNode(SceneNode parent)
        {
            _tranform = new YnTransform(parent.Transform);
            Initialize();
        }

        /// <summary>
        /// Initialize the node
        /// </summary>
        public virtual void Initialize()
        {
            _enabled = true;
            _visible = true;
        }

        /// <summary>
        /// Update children logic.
        /// </summary>
        /// <param name="gameTime">Time elapsed</param>
        public virtual void UpdateChildren(GameTime gameTime)
        {
            int nbChildren = _children.Count;

            if (nbChildren > 0)
            {
                for (int i = 0; i < nbChildren; i++)
                {
                    if (_children[i].Enabled)
                        _children[i].Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Draw children
        /// </summary>
        /// <param name="device">GraphicsDevice to use for drawing</param>
        /// <param name="camera">Camera to use</param>
        /// <param name="light">Light</param>
        public virtual void DrawChildren(GraphicsDevice device, BaseCamera camera, SceneLight light)
        {
            int nbChildren = _children.Count;

            if (nbChildren > 0)
            {
                for (int i = 0; i < nbChildren; i++)
                {
                    if (_children[i].Visible)
                        _children[i].Draw(device, camera, light);
                }
            }
        }

        /// <summary>
        /// Update node logic.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            UpdateChildren(gameTime);
        }

        /// <summary>
        /// Draw node
        /// </summary>
        /// <param name="device"></param>
        /// <param name="camera"></param>
        /// <param name="light"></param>
        public virtual void Draw(GraphicsDevice device, BaseCamera camera, SceneLight light)
        {
            DrawChildren(device, camera, light);
        }
    }

}
