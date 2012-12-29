using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D.Light;

namespace Yna.Framework.Display3D.Material
{
    public class BasicMaterial : BaseMaterial
    {
        protected bool _useDefaultLightning;
        protected bool _useTexture;
        protected Texture2D _texture;
        protected string _textureName;

        public BasicMaterial(YnObject3D object3D, BasicLight ligth, string textureName)
        {
            _object3D = object3D;
            _light = ligth;
            _textureName = textureName;
        }

        public override void LoadContent()
        {
            _effect = new BasicEffect(YnG.GraphicsDevice);

            if (_textureName != String.Empty)
            {
                _texture = YnG.Content.Load<Texture2D>(_textureName);
                _useTexture = true;
            }
            else
                _useTexture = false;
        }

        public override void Update()
        {
            BasicEffect basicEffect = (BasicEffect)_effect;

            // Matrices
            basicEffect.World = _object3D.World;
            basicEffect.View = _camera.View;
            basicEffect.Projection = _camera.Projection;

            // Texture
            basicEffect.TextureEnabled = _useTexture;
            basicEffect.Texture = _texture;

            // Lights
            if (_useDefaultLightning)
            {
                basicEffect.EnableDefaultLighting();
            }
            else
            {
                basicEffect.LightingEnabled = !_useDefaultLightning;
                basicEffect.DirectionalLight0.Enabled = true;
                basicEffect.DirectionalLight0.Direction = _light.Direction;
                basicEffect.DirectionalLight0.DiffuseColor = _light.Diffuse;
                basicEffect.DirectionalLight0.SpecularColor = _light.Specular;
                basicEffect.AmbientLightColor = _light.Ambient;
                basicEffect.EmissiveColor = _light.Emissive;
                basicEffect.Alpha = _light.Alpha;
            }
        }
    }
}
