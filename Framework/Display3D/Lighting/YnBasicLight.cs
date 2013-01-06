using System;
using Microsoft.Xna.Framework;

namespace Yna.Framework.Display3D.Lighting
{
    public class YnBasicLight
    {
        protected bool _enabled;
        protected YnDirectionalLight[] _directionalLights;
        protected Vector3 _ambientColor;
        protected float _ambientIntensity;

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public Vector3 AmbientColor
        {
            get { return _ambientColor; }
            set { _ambientColor = value; }
        }

        public float AmbientIntensity
        {
            get { return _ambientIntensity; }
            set { _ambientIntensity = value; }
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
        {
            _ambientColor = Vector3.One;
            _ambientIntensity = 1.0f;

            InitializeDirectionalLights();
        }

        public YnBasicLight(Color ambientColor)
        {
            _ambientColor = ambientColor.ToVector3();
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
