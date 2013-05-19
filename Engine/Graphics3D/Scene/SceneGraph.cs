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
    /// SceneGraph class
    /// </summary>
    public class SceneGraph : YnScene
    {
        protected SceneNode _rootNode;
        protected CameraManager _cameraManager;
        protected SceneLight _sceneLight;

        /// <summary>
        /// Gets the root node
        /// </summary>
        public SceneNode RootNode
        {
            get { return _rootNode; }
        }

        /// <summary>
        /// Gets the camera manager
        /// </summary>
        public CameraManager CameraManager
        {
            get { return _cameraManager; }
        }

        /// <summary>
        /// Gets the scene light
        /// </summary>
        public SceneLight SceneLight
        {
            get { return _sceneLight; }
        }

        /// <summary>
        /// Create a new SceneGraph with a fixed camera.
        /// </summary>
        public SceneGraph()
        {
            _cameraManager = new CameraManager(new FixedCamera());
            _sceneLight = new SceneLight();
        }

        /// <summary>
        /// Create a new SceneGraph with a custom camera
        /// </summary>
        /// <param name="camera">Custom camera</param>
        public SceneGraph(BaseCamera camera)
        {
            _cameraManager = new CameraManager(camera);
            _sceneLight = new SceneLight();
        }

        public override void Initialize()
        {
            
        }

        public override void LoadContent()
        {
            
        }

        /// <summary>
        /// Update root node and its children
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (_rootNode.Enabled)
            {
                _rootNode.Update(gameTime);
                _rootNode.UpdateChildren(gameTime);
            }

            _cameraManager.GetActiveCamera().Update(gameTime);
        }

        /// <summary>
        /// Draw root node and its children
        /// </summary>
        /// <param name="device"></param>
        public virtual void Draw(GraphicsDevice device)
        {
            if (_rootNode.Visible)
            {
                _rootNode.Draw(device, _cameraManager.GetActiveCamera(), _sceneLight);
                _rootNode.DrawChildren(device, _cameraManager.GetActiveCamera(), _sceneLight);
            }
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }
    }

    
}
