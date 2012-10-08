using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display3D.Camera;

namespace Yna.Display3D
{
    public class Model3D : YnBase3D
    {
        private BaseCamera _camera;
        private Model _model;
        private string _modelName;
        private Texture2D _texture;
        private BasicEffect _basicEffect;

        public BaseCamera Camera
        {
            get { return _camera; }
            set { _camera = value; }
        }

        public Model3D(string modelName, Vector3 position)
            : base()
        {
            _position = position;
            _modelName = modelName;
        }

        public void LoadContent()
        {
            _basicEffect = new BasicEffect(YnG.GraphicsDevice);
            _model = YnG.Content.Load<Model>(_modelName);
        }

        public void SetupShader()
        {

        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(GraphicsDevice device)
        {
            Matrix [] transforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = 
                         transforms[mesh.ParentBone.Index] *
                        _camera.World * Matrix.CreateScale(Scale) *
                        Matrix.CreateRotationX(Rotation.X) *
                        Matrix.CreateRotationY(Rotation.Y) *
                        Matrix.CreateRotationZ(Rotation.Z) *
                        Matrix.CreateTranslation(Position);
                       

                    effect.View = _camera.View;
                    effect.Projection = _camera.Projection;
                }
                mesh.Draw();
            }
        }
    }
}
