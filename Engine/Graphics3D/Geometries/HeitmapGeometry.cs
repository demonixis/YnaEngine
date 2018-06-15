// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics3D.Geometries
{
    public class HeightmapGeometry : TerrainGeometry
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
            _size = segmentSizes;
        }

        public HeightmapGeometry(Texture2D heightmapTexture)
        {
            _heightmapAssetName = heightmapTexture.Name;
            _heightmapTexture = heightmapTexture;
        }

        public HeightmapGeometry(Texture2D heightmapTexture, Vector3 segmentSizes)
            : this(heightmapTexture)
        {
            _size = segmentSizes;
        }

        public override void Generate()
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
            _width = _heightmapTexture.Width;
            _depth = _heightmapTexture.Height;

            var colors = new Color[Width * Depth];
            _heightmapTexture.GetData(colors);
            _heightData = new float[Width, Depth];

            for (var x = 0; x < Width; x++)
                for (var y = 0; y < Depth; y++)
                    _heightData[x, y] = colors[x + y * Width].R / 10.0f;
        }

        protected override void CreateVertices()
        {
            _vertices = new VertexPositionNormalTexture[Width * Depth];

            var color = Color.White;

            for (var x = 0; x < Width; x++)
            {
                for (var z = 0; z < Depth; z++)
                {
                    Vertices[x + z * Width].Position = new Vector3(
                        x * Size.X,
                        _heightData[x, z] * Size.Y,
                        Position.Z + z * Size.Z);

                    Vertices[x + z * Width].TextureCoordinate = new Vector2(
                        (float)x / (float)Width * TextureRepeat.X,
                        (float)z / (float)Depth * TextureRepeat.Y);

                    Vertices[x + z * Width].Normal = Vector3.Zero;
                }
            }
        }
    }
}
