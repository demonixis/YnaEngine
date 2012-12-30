using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D.Light;
using Yna.Framework.Display3D.Camera;

namespace Yna.Framework.Display3D.Material
{
    public class NormalMapMaterial : BaseMaterial
    {
        protected Texture2D _diffuseMap;
        protected Texture2D _normalMap;
        protected string _diffuseMapName;
        protected string _normalMapName;
        protected string _effectName;

        public NormalMapMaterial(string diffuseMapName, string normalMapName)
        {
            // Textures
            _diffuseMapName = diffuseMapName;
            _normalMapName = normalMapName;
	        _effectName = "NormalMapEffect";
        }

        public override void LoadContent()
        {
            _diffuseMap = YnG.Content.Load<Texture2D>(_diffuseMapName);
            _normalMap = YnG.Content.Load<Texture2D>(_normalMapName);
            _effect = YnG.Content.Load<Effect>(_effectName);
        }

        public override void Update(ref Matrix world, ref Matrix view, ref Matrix projection, ref Vector3 position)
        {
            // Matrices
            _effect.Parameters["World"].SetValue(world);
            _effect.Parameters["View"].SetValue(view);
            _effect.Parameters["Projection"].SetValue(projection);

            // Lights
            _effect.Parameters["AmbientColor"].SetValue(_light.Ambient);
            _effect.Parameters["AmbientIntensity"].SetValue(1.0f);
            _effect.Parameters["DiffuseColor"].SetValue(_light.Diffuse);
            _effect.Parameters["DiffuseIntensity"].SetValue(1.5f);
            _effect.Parameters["SpecularColor"].SetValue(_light.Specular);
            _effect.Parameters["LightDirection"].SetValue(_light.Direction);
            
            // Textures
            _effect.Parameters["ColorMapSampler"].SetValue(_diffuseMap);
            _effect.Parameters["NormalMapSampler"].SetValue(_normalMap);   

            // Position
            _effect.Parameters["EyePosition"].SetValue(position);
        }
    }
}
