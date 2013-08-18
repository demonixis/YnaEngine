// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Lighting;

namespace Yna.Engine.Graphics3D
{
    public class YnScene3D : YnGroup3D
    {
        protected SceneLight _sceneLight;

        /// <summary>
        /// Gets or sets the basic light of the scene.
        /// </summary>
        public SceneLight SceneLight
        {
            get { return _sceneLight; }
            set { _sceneLight = value; }
        }

        public YnScene3D()
            : base(null)
        {
            _sceneLight = new SceneLight();
            _sceneLight.AmbientIntensity = 1f;
        }

        /// <summary>
        /// Draw members.
        /// </summary>
        /// <param name="device"></param>
        public override void Draw(GameTime gameTime, GraphicsDevice device, BaseCamera camera)
        {
            _members.Draw(gameTime, device, camera, _sceneLight);
        }
    }
}
