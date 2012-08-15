using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Yna.Display;

namespace Yna.Display
{
    internal class Path
    {
        public Vector2 Destination;
        public float Speed;
        public bool Arrived;

        public Path(int x, int y, float speed)
        {
            Destination = new Vector2(x, y);
            Speed = speed;
        }
    }

    public class YnPath : YnBase
    {
        private List<Path> _destinations;
        private bool _repeat;
        private int _pathIndex;
        private YnSprite _sprite;
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

        public YnPath(YnSprite sprite, bool repeat)
        {
            _sprite = sprite;
            _repeat = repeat;
            _pathIndex = 0;
            _destinations = new List<Path>();
            _ready = false;
            Active = false;
        }

        /// <summary>
        /// Add a new point to the path, the coordinates must be absolute
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="speed">Velocity speed for this path</param>
        public void Add(int x, int y, float speed = 2)
        {
            _destinations.Add(new Path(x, y, speed));
        }

        /// <summary>
        /// Add a new point to the path, the coordinates are relative to the sprite used for this path
        /// </summary>
        /// <param name="destinationX">Destination X</param>
        /// <param name="destinationY">Destination Y</param>
        /// <param name="speed">The speed of translation</param>
        public void AddTo(int destinationX, int destinationY, float speed)
        {
            // If is the begining of the path, it's relative to the sprite's position
            int destX = destinationX + _sprite.X;
            int destY = destinationY + _sprite.Y;

            // Else it's relative to the last path coordinates
            if (_destinations.Count > 1)
            {
                int size = _destinations.Count;
                _destinations.Add(new Path(destinationX + (int)_destinations[size - 1].Destination.X, destinationY + (int)_destinations[size - 1].Destination.Y, speed));
            }

            // Add new path to the destinations
            _destinations.Add(new Path(destX, destY, speed));
        }

        /// <summary>
        /// Close the path
        /// </summary>
        public void End()
        {
            if (_destinations.Count > 2)
            {
                _ready = true;

                Active = true;

                _pathIndex = 0;
            }
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
                    Vector2 distance = target - position;

                    if (position == target)
                    {
                        _pathIndex++;
                        return;
                    }

                    Vector2 direction = distance;
                    direction.Normalize();

                    Vector2 newPosition = Vector2.Multiply(direction, 1);
                    position += newPosition;
                    _sprite.Position = position;
                }
            }
        }
    }
}