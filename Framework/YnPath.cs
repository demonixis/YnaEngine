using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Yna.Framework.Display;

namespace Yna.Framework
{
    internal class Destination
    {
        public Vector2 Point;
        public float Speed;

        public Destination(int x, int y, float speed)
        {
            Point = new Vector2(x, y);
            Speed = speed;
        }
    }

    public class YnPath : YnBase
    {
        #region Private declarations
        private List<Destination> _destinations;
        private bool _repeat;
        private int _pathIndex;
        private YnSprite _sprite;
        private bool _ready;
        #endregion

        #region Events
        /// <summary>
        /// Trigger when the sprite begin to move
        /// </summary>
        public event EventHandler<EventArgs> Started = null;

        /// <summary>
        /// Trigger when the path is restarted
        /// </summary>
        public event EventHandler<EventArgs> Restarted = null;

        /// <summary>
        /// Trigger when the path is finised
        /// </summary>
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

        #endregion

        /// <summary>
        /// Create a path that a Sprite will follow automatically
        /// </summary>
        /// <param name="sprite">The sprite who must follow this path</param>
        /// <param name="repeat">Repeat or not the path</param>
        public YnPath(YnSprite sprite, bool repeat)
        {
            _sprite = sprite;
            _repeat = repeat;
            _pathIndex = 0;
            _destinations = new List<Destination>();
            _ready = false;
            Active = false;
        }

        /// <summary>
        /// Add a new point to the path, the coordinates must be absolute
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="speed">Velocity speed for this path</param>
        public void Add(int x, int y, float speed)
        {
            _destinations.Add(new Destination(x, y, speed));
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
                _destinations.Add(new Destination(destinationX + (int)_destinations[size - 1].Point.X, destinationY + (int)_destinations[size - 1].Point.Y, speed));
            }

            // Add new path to the destinations
            _destinations.Add(new Destination(destX, destY, speed));
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

        /// <summary>
        /// Clear the current path
        /// </summary>
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
                    Vector2 target = _destinations[_pathIndex].Point;
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