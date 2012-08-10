using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Yna.Display
{
    internal class Path
    {
        public Vector2 Destination;
        public float Speed;

        public Path(Vector2 destination, float speed)
        {
            Destination = destination;
            Speed = speed;
        }
    }

    public class YnPath : YnBase
    {
        private List<Path> _destinations;
        private bool _repeat;
        private int _pathIndex;
        private Sprite _sprite;
        private bool _ready;

        public event EventHandler<EventArgs> Started = null;
        public event EventHandler<EventArgs> Restarted = null;
        public event EventHandler<EventArgs> Arrived = null;

        private void OnStarted(EventArgs e)
        {
            if (Started != null)
                Started(this, e);
        }

        private void OnRestarted(EventArgs e)
        {
            if (Restarted != null)
                Restarted(this, e);
        }

        private void OnArrived(EventArgs e)
        {
            if (Arrived != null)
                Arrived(this, e);
        }

        public YnPath(Sprite sprite, bool repeat)
        {
            _sprite = sprite;
            _repeat = repeat;
            _pathIndex = 0;
            _destinations = new List<Path>();
            _ready = false;
            Active = false;
        }

        public void Begin(int x, int y, float speed = 2)
        {
            if (!Active && _destinations.Count == 0)
                _destinations.Add(new Path(new Vector2(x, y), speed);
        }

        public void Add(int x, int y, float speed = 2)
        {
            _destinations.Add(new Path(new Vector2(x, y), speed));
        }

        public void End()
        {
            if (_destinations.Count > 2)
                _ready = true;

            _pathIndex = 0;
        }

        public void Clear()
        {
            Active = false;
            _ready = false;
            _destinations.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            if (Active && _ready)
            {
                if (_destinations.Count == _pathIndex)
                {
                    if (!_repeat)
                    {
                        Active = false;
                    }
                    else
                    {
                        OnRestarted(EventArgs.Empty);
                    }
                    _pathIndex = 0;
                    OnArrived(EventArgs.Empty);
                }
                else
                {
                    Vector2 position = _sprite.Position;
                    Vector2 target = _destinations[_pathIndex].Destination;
                    Vector2 distance = Vector2.Subtract(target, position);
                    double angle = Math.Atan2(distance.Y, distance.X);

                    if (position == target)
                    {
                        _pathIndex++;
                    }
                    else
                    {
                        _sprite.X += (int)(Math.Sin(angle) * _destinations[_pathIndex].Speed);
                        _sprite.Y += (int)(Math.Cos(angle) * _destinations[_pathIndex].Speed);
                    }
                }
            }
        }
    }
}
