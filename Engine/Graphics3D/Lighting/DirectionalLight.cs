// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics3D.Lighting
{
    public class DirectionalLight
    {
        public Vector3 DiffuseColor { get; set; }
        public float DiffuseIntensity { get; set; }
        public Vector3 Direction { get; set; }
        public Vector3 SpecularColor { get; set; }
        public float SpecularIntensity { get; set; }
        public bool Enabled { get; set; }

        public DirectionalLight()
        {
            DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
            DiffuseIntensity = 1.0f;
            Direction = Vector3.Zero;
            SpecularColor = Vector3.Zero;
            SpecularIntensity = 1.0f;
            Enabled = true;
        }
    }
}
