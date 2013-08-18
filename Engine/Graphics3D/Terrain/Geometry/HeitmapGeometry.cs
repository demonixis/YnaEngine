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

namespace Yna.Engine.Graphics3D.Terrain.Geometry
{
    public class HeightmapGeometry : BaseTerrainGeometry
    {
        private string _heightmapAssetName;
        private Texture2D _heightmapTexture;
        private float[,] _heightData;

        public float[,] HeightmapData
        {
            get { return _heightData; }
            protected set { _heightData = value; }
        }

        public HeightmapGeometry(string heightmapName)
        {
            _heightmapAssetName = heightmapName;
        }

        public HeightmapGeometry(string heightmapName, Vector3 segmentSizes)
            : this(heightmapName)
        {
            _segmentSizes = segmentSizes;
        }

        public HeightmapGeometry(Texture2D heightmapTexture)
        {
            _heightmapAssetName = heightmapTexture.Name;
            _heightmapTexture = heightmapTexture;
        }

        public HeightmapGeometry(Texture2D heightmapTexture, Vector3 segmentSizes)
            : this(heightmapTexture)
        {
            _segmentSizes = segmentSizes;
        }

        public override void GenerateGeometry()
        {
            if (_heightmapTexture == null)
                _heightmapTexture = YnG.Content.Load<Texture2D>(_heightmapAssetName);

            LoadHeightDatas();
            CreateVertices();
            CreateIndices();
            CreateBuffers();

            ComputeNormals(ref _vertices);
        }

        private void LoadHeightDatas()
        {
            Width = _heightmapTexture.Width;
            Depth = _heightmapTexture.Height;

            Color[] colors = new Color[Width * Depth];
            _heightmapTexture.GetData(colors);

            _heightData = new float[Width, Depth];

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Depth; y++)
                    _heightData[x, y] = colors[x + y * Width].R / 10.0f; // Max height 25.5f
        }

        protected override void CreateVertices()
        {
            Vertices = new VertexPositionNormalTexture[Width * Depth];

            Color color = Color.White;

            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    Vertices[x + z * Width].Position = new Vector3(
                        x * SegmentSizes.X,
                        _heightData[x, z] * SegmentSizes.Y,
                        Position.Z + z * SegmentSizes.Z);

                    Vertices[x + z * Width].TextureCoordinate = new Vector2(
                        (float)x / (float)Width * TextureRepeat.X,
                        (float)z / (float)Depth * TextureRepeat.Y);

                    Vertices[x + z * Width].Normal = Vector3.Zero;
                }
            }
        }
    }
}
