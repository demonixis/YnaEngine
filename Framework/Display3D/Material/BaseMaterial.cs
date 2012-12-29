using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D.Light;
using Yna.Framework.Display3D.Camera;

namespace Yna.Framework.Display3D.Material
{
    public abstract class BaseMaterial
    {
        protected Effect _effect;
        protected BasicLight _light;
        protected BaseCamera _camera;
        protected YnObject3D _object3D;

        public abstract void SetParameters(Dictionary<string, object> parameters);

        public abstract void LoadContent();

        public abstract void Update();
    }
}
