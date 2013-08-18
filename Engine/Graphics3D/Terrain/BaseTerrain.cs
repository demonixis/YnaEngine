// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Engine.Graphics3D.Terrain
{
    /// <summary>
    /// Abstract class that represent a basic Terrain
    /// </summary>
    public abstract class BaseTerrain : YnMeshGeometry
    {
        public BaseTerrain()
            : this(0, 0, 0)
        {

        }

        public BaseTerrain(float width, float height, float depth)
            : base()
        {
            _width = width;
            _height = height;
            _depth = depth;
            _geometry = null;
            _material = null;
        }

        public override void LoadContent()
        {
            _geometry.GenerateGeometry();
            _material.LoadContent();
            UpdateBoundingVolumes();
            _initialized = true; 
        }

        public override void UpdateBoundingVolumes()
        {
            // Reset bounding box to min/max values
            _boundingBox.Min = new Vector3(float.MaxValue);
            _boundingBox.Max = new Vector3(float.MinValue);

            for (int i = 0; i < _geometry.Vertices.Length; i++)
            {
                _boundingBox.Min.X = Math.Min(_boundingBox.Min.X, _geometry.Vertices[i].Position.X);
                _boundingBox.Min.Y = Math.Min(_boundingBox.Min.Y, _geometry.Vertices[i].Position.Y);
                _boundingBox.Min.Z = Math.Min(_boundingBox.Min.Z, _geometry.Vertices[i].Position.Z);
                _boundingBox.Max.X = Math.Max(_boundingBox.Max.X, _geometry.Vertices[i].Position.X);
                _boundingBox.Max.Y = Math.Max(_boundingBox.Max.Y, _geometry.Vertices[i].Position.Y);
                _boundingBox.Max.Z = Math.Max(_boundingBox.Max.Z, _geometry.Vertices[i].Position.Z);
            }

            // Apply scale on the object
            _boundingBox.Min *= _scale;
            _boundingBox.Max *= _scale;

            // Update size of the object
            _width = _boundingBox.Max.X - _boundingBox.Min.X;
            _height = _boundingBox.Max.Y - _boundingBox.Min.Y;
            _depth = _boundingBox.Max.Z - _boundingBox.Min.Z;

            // Update bouding sphere
            _boundingSphere.Center.X = X + (_width / 2);
            _boundingSphere.Center.Y = Y + (_height / 2);
            _boundingSphere.Center.Z = Z + (_depth / 2);
            _boundingSphere.Radius = Math.Max(Math.Max(_width, _height), _depth) / 2;
        }
    }
}
