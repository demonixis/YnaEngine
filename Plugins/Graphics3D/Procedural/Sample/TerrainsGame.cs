using System;
using Yna.Engine;

namespace Yna.Samples
{
    public class TerrainsGame : YnGame
    {
        public TerrainsGame()
                : base(1024, 768, "Yna : Heightmap")
        {

        }

        protected override void Initialize()
        {
            base.Initialize();
            stateManager.Add(new ProceduralSandbox("proceduralSandboxSample"), true);
        }
#if !WINDOWS_PHONE && !MACOSX
        public static void Main(string[] args)
        {
            using (TerrainsGame game = new TerrainsGame())
            {
                game.Run();
            }
        }
#elif MACOSX
		static void Main (string[] args)
		{
			MonoMac.AppKit.NSApplication.Init ();
			
			using (var p = new MonoMac.Foundation.NSAutoreleasePool ()) 
			{
				MonoMac.AppKit.NSApplication.SharedApplication.Delegate = new AppDelegate ();
				MonoMac.AppKit.NSApplication.Main (args);
			}
		}
		
		class AppDelegate : MonoMac.AppKit.NSApplicationDelegate
		{
			TerrainsGame game;
			public override void FinishedLaunching (MonoMac.Foundation.NSObject notification)
			{
				game = new TerrainsGame();
				game.Run();
			}
			
			public override bool ApplicationShouldTerminateAfterLastWindowClosed (MonoMac.AppKit.NSApplication sender)
			{
				return true;
			}
		}
#endif
    }
}
