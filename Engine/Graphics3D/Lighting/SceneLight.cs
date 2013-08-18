// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics3D.Lighting
{
    public class SceneLight : BaseLight
    {
        protected Vector3 _specularColor;
        protected float _specularIntensity;

        protected DirectionalLight[] _directionalLights;

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

        public DirectionalLight[] DirectionalLights
        {
            get { return _directionalLights; }
            set { _directionalLights = value; }
        }

        /// <summary>
        /// Create a basic light with just an ambient color set to white
        /// </summary>
        public SceneLight()
            : base()
        {
            _specularColor = Vector3.Zero;
            _specularIntensity = 1.0f;

            InitializeDirectionalLights();
        }

        private void InitializeDirectionalLights()
        {
            _directionalLights = new DirectionalLight[3];

            for (int i = 0; i < 3; i++)
            {
                _directionalLights[i] = new DirectionalLight();
                if (i > 0)
                    _directionalLights[i].Enabled = false;
            }

            _enabled = true;
        }    
    }
}
