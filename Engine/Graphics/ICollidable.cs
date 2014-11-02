// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics
{
    /// <summary>
    /// An interface to collide with 2D object
    /// </summary>
    public interface ICollidable2
    {
        Rectangle Rectangle { get; }
    }

    public interface IColladable2PerfectPixel
    {
        Vector2 Scale { get; }
        Texture2D Texture { get; }
        Rectangle Rectangle { get; }
    }
}
