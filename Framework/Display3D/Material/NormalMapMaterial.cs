using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D.Light;

namespace Yna.Framework.Display3D.Material
{
    public class NormalMapMaterial : BaseMaterial
    {
        protected Texture2D _diffuseMap;
        protected Texture2D _normalMap;
        protected string _diffuseMapName;
        protected string _normalMapName;
        protected string _effectName;

        public NormalMapMaterial(YnObject3D object3D, BasicLight light, string diffuseMapName, string normalMapName)
        {
            // TODO : do it when we add this material on the object
            _object3D = object3D;
            _light = light;

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

        public override void Update()
        {
            // Matrices
            _effect.Parameters["World"].SetValue(_object3D.World);
            _effect.Parameters["View"].SetValue(_camera.View);
            _effect.Parameters["Projection"].SetValue(_camera.Projection);

            // Lights
            _effect.Parameters["AmbientColor"].SetValue(_light.Ambient);
            _effect.Parameters["AmbientIntensity"].SetValue(0.8f);
            _effect.Parameters["DiffuseColor"].SetValue(_light.Diffuse);
            _effect.Parameters["DiffuseIntensity"].SetValue(2.7f);
            _effect.Parameters["SpecularColor"].SetValue(_light.Specular);
            _effect.Parameters["LightDirection"].SetValue(_light.Direction);
            
            // Textures
            _effect.Parameters["ColorMapSampler"].SetValue(_diffuseMap);
            _effect.Parameters["NormalMapSampler"].SetValue(_normalMap);   
        }
    }
}
