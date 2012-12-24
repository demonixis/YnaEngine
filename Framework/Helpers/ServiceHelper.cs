using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Yna.Framework.Helpers
{
    /// <summary>
    /// Service helper who is used to add, get and remove services
    /// </summary>
    public static class ServiceHelper
    {
        static Game _game;

        public static Game Game
        {
            set { _game = value; }
        }

        /// <summary>
        /// Add a new service to the services container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        public static void Add<T>(T service) where T : class
        {
            _game.Services.AddService(typeof(T), service);
        }

        /// <summary>
        /// Get a service from the services container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>() where T : class
        {
            return _game.Services.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Remove a service from the services container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        public static void Remove<T>(T service) where T : class
        {
            _game.Services.RemoveService(typeof(T));
        }
    }
}
