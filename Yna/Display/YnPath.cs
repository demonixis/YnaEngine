using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Yna.Display;

namespace Yna.Display
{
    public class YnPath : YnBase
    {
        private YnSprite _sprite;
        private List<Vector2> _destinations;
        private Vector2 [] _directions;
        private float[] _lenghts;
        private int _index;
        private float _stagePosition;
        private float _speed;

        public YnPath(YnSprite sprite)
        {
            _sprite = sprite;
            _destinations = new List<Vector2>();
            Active = false;
            _speed = 2;
        }

        public void Begin(int x, int y)
        {
            if (!Active && _destinations.Count == 0)
            {
                _index = 0;
                _destinations.Add(new Vector2(x, y));
            }
        }
    
        public void Add(int x, int y)
        {
            _destinations.Add(new Vector2(x, y));
        }

        /// <summary>
        /// Close the path
        /// </summary>
        public void End()
        {
            if (_destinations.Count > 2)
            {
                Active = true;
                
                int count = _destinations.Count - 1;
                
                _lenghts = new float[count];
                _directions = new Vector2[count];

                for (int i = 0; i < count; i++)
                {
                    _directions[i] = _destinations[i + 1] - _destinations[i];
                    _lenghts[i] = _directions[i].Length();
                    _directions[i].Normalize();
                }
            }
        }

        public void Clear()
        {
            Active = false;
            _destinations.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                if (_index != _destinations.Count - 1)
                {
                    _stagePosition = _speed * gameTime.TotalGameTime.Seconds;

                    while (_stagePosition > _lenghts[_index])
                    {
                        _stagePosition -= _lenghts[_index];
                        _index++;

                        if (_index == _destinations.Count - 1)
                        {
                            _sprite.Position = _destinations[_index];
                            return;
                        }
                    }
                    _sprite.Position = _destinations[_index] + _directions[_index] * _stagePosition;
                }
            }
        }
    }
}
