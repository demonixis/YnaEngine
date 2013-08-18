// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics3D.Lighting
{
    public class BaseLight
    {
        protected bool _enabled;
        protected Vector3 _ambientColor;
        protected float _ambientIntensity;
        protected Vector3 _diffuseColor;
        protected float _diffuseIntensity;

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

        public Vector3 DiffuseColor
        {
            get { return _diffuseColor; }
            set { _diffuseColor = value; }
        }

        public float DiffuseIntensity
        {
            get { return _diffuseIntensity; }
            set { _diffuseIntensity = value; }
        }

        /// <summary>
        /// Create a basic light with just an ambient color set to white
        /// </summary>
        public BaseLight()
        {
            _ambientColor = Vector3.One;
            _ambientIntensity = 1.0f;
            _diffuseColor = Vector3.One;
            _diffuseIntensity = 1.0f;
        }
    }
}
