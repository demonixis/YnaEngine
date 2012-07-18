using System;

namespace Yna.Samples
{
    static class Program
    {
        /// <summary>
        /// Point d’entrée principal pour l’application.
        /// </summary>
        static void Main(string[] args)
        {
            using (YnaSample game = new YnaSample())
            {
                game.Run();
            }
        }
    }
}

