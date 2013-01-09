using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Yna.Framework.Display3D.Camera;
using Yna.Framework.Display3D.Primitive;
using Yna.Framework.Display3D.Material;

namespace Yna.Framework.Display3D.Terrain
{
    public class Skybox : CubeShape
    {
        private string[] _textureNames;
        private Texture2D[] _textures;
        private TextureCube _textureCube;

        public new BaseMaterial Material
        {
            get { return _material; }
            protected set { _material = value; }
        }

        public new Vector3 Scale
        {
            get { return _scale; }
            set
            {
                _scale.X = value.X < 0 ? value.X : -value.X;
                _scale.Y = value.Y < 0 ? value.Y : -value.Y;
                _scale.Z = value.Z < 0 ? value.Z : -value.Z;
            }
        }

        public Skybox(Vector3 position, Vector3 size, string[] textures)
            : base("", size, position)
        {
            _textureNames = textures;
            _textures = new Texture2D[6];
            _scale *= -1;
            EnvironmentMapMaterial material = new EnvironmentMapMaterial("", textures);
            material.FresnelFactor = 0.0f;
            

            _material = material;
        }
    }
}
