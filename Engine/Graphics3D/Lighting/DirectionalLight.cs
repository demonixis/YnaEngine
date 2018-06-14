// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics3D.Lighting
{
    public class DirectionalLight
    {
        public Vector3 DiffuseColor { get; set; } = Vector3.One;
        public float DiffuseIntensity { get; set; } = 1.0f;
        public Vector3 SpecularColor { get; set; } = new Vector3(0.3f);
        public float SpecularIntensity { get; set; } = 0.5f;
        public Vector3 Direction { get; set; } = new Vector3(1, 1, 0);
        public bool Enabled { get; set; } = true;
    }
}
