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

        public Effect Effect
        {
            get { return _effect; }
            set { _effect = value; }
        }

        public BasicLight Light
        {
            get { return _light; }
            set { _light = value; }
        }

        public abstract void LoadContent();

        public abstract void Update(ref Matrix world, ref Matrix view, ref Matrix projection, ref Vector3 position);
    }
}
