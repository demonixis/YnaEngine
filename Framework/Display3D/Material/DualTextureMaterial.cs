using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D.Lighting;
using Yna.Framework.Display3D.Camera;

namespace Yna.Framework.Display3D.Material
{
    public class DualTextureMaterial : StockMaterial
    {
        protected Texture2D _secondTexture;
        protected string _secondTextureName;
        protected bool _secondTextureLoaded;

        public override void LoadContent()
        {
            base.LoadContent();

            if (!_secondTextureLoaded)
            {
                _secondTexture = YnG.Content.Load<Texture2D>(_secondTextureName);
                _secondTextureLoaded = true;
            }

            if (!_effectLoaded)
            {
                _effect = new DualTextureEffect(YnG.GraphicsDevice);
                _effectLoaded = true;
            }
        }

        public override void Update(ref Matrix world, ref Matrix view, ref Matrix projection, ref Vector3 position)
        {
            DualTextureEffect dualTextureEffect = (DualTextureEffect)_effect;

            // Matrices
            dualTextureEffect.World = world;
            dualTextureEffect.View = view;
            dualTextureEffect.Projection = projection;

            // Textures
            dualTextureEffect.Texture = _mainTexture;
            dualTextureEffect.Texture2 = _secondTexture;

            // Fog
            if (_enabledFog)
            {
                dualTextureEffect.FogEnabled = _enabledFog;
                dualTextureEffect.FogColor = _fogColor;
                dualTextureEffect.FogStart = _fogStart;
                dualTextureEffect.FogEnd = _fogEnd;
            }

            // Lights
            dualTextureEffect.DiffuseColor = new Vector3(_diffuseColor.X, _diffuseColor.Y, _diffuseColor.Z) * _diffuseIntensity;
            dualTextureEffect.Alpha = _alphaColor;
        }
    }
}
