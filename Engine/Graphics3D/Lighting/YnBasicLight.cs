using System;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics3D.Lighting
{
    public class YnBasicLight : YnLight
    {
        protected Vector3 _specularColor;
        protected float _specularIntensity;

        protected YnDirectionalLight[] _directionalLights;

        public Vector3 SpecularColor
        {
            get { return _specularColor; }
            set { _specularColor = value; }
        }

        public float SpecularIntensity
        {
            get { return _specularIntensity; }
            set { _specularIntensity = value; }
        }

        public YnDirectionalLight[] DirectionalLights
        {
            get { return _directionalLights; }
            set { _directionalLights = value; }
        }

        /// <summary>
        /// Create a basic light with just an ambient color set to white
        /// </summary>
        public YnBasicLight()
            : base()
        {
            _specularColor = Vector3.Zero;
            _specularIntensity = 1.0f;

            InitializeDirectionalLights();
        }

        private void InitializeDirectionalLights()
        {
            _directionalLights = new YnDirectionalLight[3];

            for (int i = 0; i < 3; i++)
            {
                _directionalLights[i] = new YnDirectionalLight();
                if (i > 0)
                    _directionalLights[i].Enabled = false;
            }

            _enabled = true;
        }    
    }
}
