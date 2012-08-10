using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Yna.Display
{
    internal class Destination
    {
        public Vector2 Destination;
        public float Speed;

        public Destination(Vector2 destination, float speed)
        {
            Destination = destination;
            Speed = speed;
        }
    }

    public class YnPath : YnBase
    {
        private List<Destination> _destinations;
        private bool _repeat;
        private int _pathIndex;
        private Sprite _sprite;

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
            _destinations = new List<Destination>();
            Active = false;
        }

        public void AddPoint(int x, int y, float speed = 2)
        {
            _destinations.Add(new Destination(new Vector2(x, y), speed));
        }

        public void Start()
        {
            Active = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                _pathIndex++;

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
                    
                }
            }
        }
    }
}
