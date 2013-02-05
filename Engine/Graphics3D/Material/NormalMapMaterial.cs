using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Camera;

namespace Yna.Engine.Graphics3D.Material
{
    public class NormalMapMaterial : BaseMaterial
    {
        protected Texture2D _normalMap;
        protected string _normalMapName;
        protected Vector4 _specularColor;
        protected float _specularIntensity;
        protected Vector3 _diffuseDirection;

        /// <summary>
        /// Gets the normal texture
        /// </summary>
        public Texture2D NormalTexture
        {
            get { return _normalMap; }
            set { _normalMap = value; }
        }

        /// <summary>
        /// Gets or sets the specular color
        /// </summary>
        public Vector4 SpecularColor
        {
            get { return _specularColor; }
            set { _specularColor = value; }
        }

        /// <summary>
        /// Gets or sets the specular intensity
        /// </summary>
        public float SpecularIntensitiy
        {
            get { return _specularIntensity; }
            set { _specularIntensity = value; }
        }

        /// <summary>
        /// Gets or sets the diffuse direction
        /// </summary>
        public Vector3 DiffuseDirection
        {
            get { return _diffuseDirection; }
            set { _diffuseDirection = value; }
        }

        public NormalMapMaterial(string diffuseMapName, string normalMapName)
        {
            _diffuseIntensity = 1.0f;
            _textureName = diffuseMapName;
            _normalMapName = normalMapName;
            _effectName = "NormalMapEffect";
            _specularColor = Color.Black.ToVector4();
            _specularIntensity = 1.0f;
            _diffuseDirection = Vector3.Zero;
            _textureLoaded = false;
        }

        public NormalMapMaterial(string diffuseMapName, string normalMapName, string effectName)
            : this(diffuseMapName, normalMapName)
        {
            _effectName = effectName;
        }

        public override void LoadContent()
        {
            _texture = YnG.Content.Load<Texture2D>(_textureName);
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
            _effect.Parameters["AmbientColor"].SetValue(_ambientColor);
            _effect.Parameters["AmbientIntensity"].SetValue(_ambientIntensity);
            _effect.Parameters["DiffuseColor"].SetValue(_diffuseColor);
            _effect.Parameters["DiffuseIntensity"].SetValue(_diffuseIntensity);

            _effect.Parameters["SpecularColor"].SetValue(_specularColor * _specularIntensity);
            _effect.Parameters["LightDirection"].SetValue(_diffuseDirection);

            // Textures
            _effect.Parameters["ColorMapSampler"].SetValue(_texture);
            _effect.Parameters["NormalMapSampler"].SetValue(_normalMap);

            // Position
            _effect.Parameters["EyePosition"].SetValue(position);
        }

        public static Vector4 ToVector4(Vector3 vector3)
        {
            return new Vector4(vector3.X, vector3.Y, vector3.Z, 1.0f);
        }
    }
}
