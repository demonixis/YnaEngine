using System;
using Microsoft.Xna.Framework;

namespace Yna.Framework.Display3D.Light
{
    public class BasicLight
    {
        protected Vector3 [] _directions;
        protected float _alpha;
        protected Vector3 _ambientLightColor;
        protected Vector3 _emissiveLightColor;
        protected Vector3 _diffuseLightColor;
        protected Vector3 _specularLightColor;

        public Vector3 Direction
        {
            get { return _directions[0]; }
            set { _directions[0] = value; }
        }

        public float Alpha
        {
            get { return _alpha; }
            set
            {
                if (value > 1.0f)
                    _alpha = 1.0f;
                else if (value < 0.0f)
                    _alpha = 0.0f;
                else
                    _alpha = value;
            }
        }

        public Vector3 Ambient
        {
            get { return _ambientLightColor; }
            set { _ambientLightColor = value; }
        }

        public Vector3 Emissive
        {
            get { return _emissiveLightColor; }
            set { _emissiveLightColor = value; }
        }

        public Vector3 Diffuse
        {
            get { return _diffuseLightColor; }
            set { _diffuseLightColor = value; }
        }

        public Vector3 Specular
        {
            get { return _specularLightColor; }
            set { _specularLightColor = value; }
        }


        /// <summary>
        /// Create a basic light with just an ambient color set to white
        /// </summary>
        public BasicLight()
        {
            Initialize();
            _ambientLightColor = new Vector3(1.0f);
            _diffuseLightColor = Vector3.Zero;
            _emissiveLightColor = Vector3.Zero;
            _specularLightColor = Vector3.Zero;
            _alpha = 1.0f;
        }

        public BasicLight(Color ambientColor, Color emissiveColor, Color diffuseColor, Color specularColor, float alpha)
        {
            Initialize();
            _ambientLightColor = ColorToVector3(ref ambientColor);
            _diffuseLightColor = ColorToVector3(ref emissiveColor);
            _emissiveLightColor = ColorToVector3(ref diffuseColor);
            _specularLightColor = ColorToVector3(ref specularColor);
            _alpha = alpha;
        }

        private void Initialize()
        {
            _directions = new Vector3[3];

            for (int i = 0; i < 3; i++)
                _directions[i] = new Vector3(1, 0, 0);
        }

        protected Vector3 ColorToVector3(ref Color color)
        {
            return color.ToVector3();
        }

        protected Color Vector3ToColor(ref Vector3 vec3)
        {
            return new Color(vec3.X, vec3.Y, vec3.Z);
        }
    }
}
