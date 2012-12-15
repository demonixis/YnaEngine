using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Yna.Framework.Helpers
{
    public static class ServiceHelper
    {
        static Game _game;

        public static Game Game
        {
            set { _game = value; }
        }

        public static void Add<T>(T service) where T : class
        {
            _game.Services.AddService(typeof(T), service);
        }

        public static T Get<T>() where T : class
        {
            return _game.Services.GetService(typeof(T)) as T;
        }

        public static void Remove<T>(T service) where T : class
        {
            _game.Services.RemoveService(typeof(T));
        }
    }
}
