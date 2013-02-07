using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MonoGame.Framework;


namespace Yna.Samples
{
    /// <summary>
    /// The root page used to display the game.
    /// </summary>
    public sealed partial class GamePage : SwapChainBackgroundPanel
    {
        readonly ModernUIGame _game;

        public GamePage(string launchArguments)
        {
            this.InitializeComponent();

            // Create the game.
            _game = XamlGame<ModernUIGame>.Create(launchArguments, Window.Current.CoreWindow, this);
        }

        // Launch a demo
        public void LaunchDemo(string stateName)
        {
            _game.RunState(stateName);
        }

        // Return to menu
        private void Image_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            _game.Pause();
            var mainPage = App.MainPage;
            Window.Current.Content = mainPage;
            Window.Current.Activate();
        }
    }
}
