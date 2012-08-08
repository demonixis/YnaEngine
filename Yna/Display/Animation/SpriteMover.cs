using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Yna;

namespace Yna.Display.Animation
{
    public enum SpriteMovements
    {
        Up = 0, Down, Left, Right, Idle
    }

    public class SpriteMover : YnBase
    {
        protected Sprite _sprite;
        protected float _elapsedTime;
        protected SpriteMovements _movement;
        protected int _nbMovements;
        protected float _refreshInterval;
        protected float _moveSpeed;

        public SpriteMover(Sprite sprite)
        {
            _sprite = sprite;
            _elapsedTime = 0;
            _movement = SpriteMovements.Idle;
            _refreshInterval = 2000f;
            _moveSpeed = 2;
        }

        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            Vector2 direction = _sprite.Direction;
            Vector2 target = _sprite.Position;

            if (_elapsedTime >= _refreshInterval)
            {
                Random rand = new Random((int)(DateTime.Now.Millisecond * gameTime.TotalGameTime.Milliseconds));
                int randMovement = rand.Next(6);

                switch (randMovement)
                {
                    case 0:
                        direction = Vector2.Zero;
                        break;
                    case 1:
                        direction = new Vector2(0, -1);
                        break;
                    case 2:
                        direction = new Vector2(0, 1);
                        break;
                    case 3:
                        direction = new Vector2(-1, 0);
                        break;
                    case 4:
                        direction = new Vector2(1, 0);
                        break;
                }
                _elapsedTime = 0;
            }

            target = new Vector2(_sprite.X + _moveSpeed * direction.X, _sprite.Y + _moveSpeed * direction.Y);

            CheckCollide(ref target, ref direction);

            _sprite.Position = target;
            _sprite.Direction = direction;
        }

        protected virtual void CheckCollide(ref Vector2 target, ref Vector2 direction)
        {
            // Gestion des collisions avec les bords
            if (target.X < 0)
            {
                target = new Vector2(target.X + _moveSpeed, target.Y);
                direction = new Vector2(1, direction.Y);
            }
            else if (target.X + _sprite.Width > YnG.Width)
            {
                target = new Vector2(target.X - _moveSpeed, target.Y);
                direction = new Vector2(-1, direction.Y);
            }

            if (target.Y < 0)
            {
                target = new Vector2(target.X, target.Y + _moveSpeed);
                direction = new Vector2(direction.X, 1);
            }
            else if (target.Y + _sprite.Height > YnG.Height)
            {
                target = new Vector2(target.X, target.Y - _moveSpeed);
                direction = new Vector2(direction.X, -1);
            }
        }
    }
}
