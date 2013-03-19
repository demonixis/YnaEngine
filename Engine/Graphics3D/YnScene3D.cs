using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Lighting;

namespace Yna.Engine.Graphics3D
{
    public class YnScene3D : YnGroup3D
    {
        private YnBaseList _basicObjects;
        protected SceneLight _sceneLight;

        /// <summary>
        /// Gets or sets the basic light of the scene.
        /// </summary>
        public SceneLight SceneLight
        {
            get { return _sceneLight; }
            set { _sceneLight = value; }
        }

        /// <summary>
        /// Gets base members like Camera, Timers, etc...
        /// </summary>
        public List<YnBase> BaseMembers
        {
            get { return _basicObjects.Members; }
            protected set { _basicObjects.Members = value; }
        }

        public YnScene3D()
            : base(null)
        {
            _sceneLight = new SceneLight();
            _sceneLight.AmbientIntensity = 1f;
            _basicObjects = new YnBaseList();
        }

        /// <summary>
        /// Update logic of all members.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            _basicObjects.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw members.
        /// </summary>
        /// <param name="device"></param>
        public override void Draw(GameTime gameTime, GraphicsDevice device, BaseCamera camera)
        {
            _members.Draw(gameTime, device, camera, _sceneLight);
        }

        #region Collection methods

        /// <summary>
        /// Add a basic object to the scene.
        /// </summary>
        /// <param name="basicObject">A basic object like Timer, Camera, etc...</param>
        /// <returns>Return true if the object has been added, otherwise return false.</returns>
        public bool Add(YnBase basicObject)
        {
            return _basicObjects.Add(basicObject);
        }

        /// <summary>
        /// Gets a basic member with its index id.
        /// </summary>
        /// <param name="index">Index of object.</param>
        /// <returns>Return desired object if exists, otherwise return null.</returns>
        public YnBase GetBasicMember(int index)
        {
            return _basicObjects[index];
        }

        /// <summary>
        /// Remove a basic object to the scene.
        /// </summary>
        /// <param name="basicObject">Basic object to remove.</param>
        /// <returns>Return true if the object has been succefully removed, otherwise return false.</returns>
        public bool Remove(YnBase basicObject)
        {
            return _basicObjects.Remove(basicObject);
        }

        #endregion
    }
}
