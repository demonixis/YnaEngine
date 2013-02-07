using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Yna.Samples
{
    sealed partial class App : Application
    {
        // Our two pages, Menu and Game
        public static MainPage MainPage;
        public static GamePage GamePage;

        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            MainPage = Window.Current.Content as MainPage;

            // Create the main page
            if (MainPage == null)
                MainPage = new MainPage();

            // Create the game page
            if (GamePage == null)
                GamePage = new GamePage(args.Arguments);

            // Active the main page
            Window.Current.Content = MainPage;
            Window.Current.Activate();
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}
