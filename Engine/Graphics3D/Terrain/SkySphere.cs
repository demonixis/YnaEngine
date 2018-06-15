﻿// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Cameras;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Materials;

namespace Yna.Engine.Graphics3D.Terrain
{
    public class SkySphere : YnEntity3DGeometry
    {
        public SkySphere(string textureName, float size)
            : base(new IcoSphereGeometry(size, 2, true), new BasicMaterial(textureName))
        {
            _scale *= -1 * size;
        }
    }
}
