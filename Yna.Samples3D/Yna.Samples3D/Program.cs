using System;

namespace Yna.Samples3D
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (YnaSample3D game = new YnaSample3D())
            {
                game.Run();
            }
        }
    }
#endif
}

