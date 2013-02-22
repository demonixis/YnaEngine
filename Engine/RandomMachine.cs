using System;
using Microsoft.Xna.Framework;

namespace Yna.Engine
{
    public class RandomMachine
    {
        private Random _random;

        public RandomMachine()
            : base (DateTime.Now.Millisecond)
        {

        }

        public RandomMachine(int seed)
        {
            _random = new Random(seed);
        }

        public float GetFloat(float min, float max)
        {
            return (float)(_random.NextDouble() * (max - min) + min);
        }

        public float GetInteger(int min, int max)
        {
            return _random.Next(min, max);
        }

        public Vector2 GetVector2(float v1Min, float v1Max, float v2Min, float v2Max)
        {
            return new Vector2(
                GetFloat(v1Min, v1Max),
                GetFloat(v2Min, v2Max));
        }
    }
}

