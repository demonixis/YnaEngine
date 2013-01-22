﻿using System;
using Microsoft.Xna.Framework;

namespace Yna.Framework.Display3D.Lighting
{
    public class YnDirectionalLight
    {
        public Vector3 DiffuseColor { get; set; }
        public float DiffuseIntensity { get; set; }
        public Vector3 Direction { get; set; }
        public Vector3 SpecularColor { get; set; }
        public float SpecularIntensity { get; set; }
        public bool Enabled { get; set; }

        public YnDirectionalLight()
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
