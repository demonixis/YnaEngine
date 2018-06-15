// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics3D.Lighting
{
    public class SceneLight
    {
        public bool Enabled { get; set; } = true;
        public Vector3 AmbientColor { get; set; } = Color.White.ToVector3();
        public float AmbientIntensity { get; set; } = 1.0f;
        public Vector3 DiffuseColor { get; set; } = Color.White.ToVector3();
        public float DiffuseIntensity { get; set; } = 1.0f;
        public Vector3 SpecularColor { get; set; } = new Vector3(0.3f);
        public float SpecularIntensity { get; set; } = 0.4f;
        public DirectionalLight[] DirectionalLights { get; set; }
        public bool DefaultLighting { get; set; } = false;

        /// <summary>
        /// Create a basic light with just an ambient color set to white
        /// </summary>
        public SceneLight()
        {
            DirectionalLights = new DirectionalLight[3];

            for (var i = 0; i < DirectionalLights.Length; i++)
            {
                DirectionalLights[i] = new DirectionalLight();
                if (i > 0)
                    DirectionalLights[i].Enabled = false;
            }
        }
    }
}
