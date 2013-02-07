using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace Yna.Samples
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoqué lorsque cette page est sur le point d'être affichée dans un frame.
        /// </summary>
        /// <param name="e">Données d'événement décrivant la manière dont l'utilisateur a accédé à cette page. La propriété Parameter
        /// est généralement utilisée pour configurer la page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        private void btn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button != null)
            {
                String [] temp = button.Name.Split(new char[] { '_' });

                if (temp.Length == 3)
                {
                    string stateName = "spritesSample";
                    int id = int.Parse(temp[2]);

                    switch (id)
                    {
                        case 0: stateName = "spritesSample"; break;
                        case 1: stateName = "cubesSample"; break;
                        case 2: stateName = "heightmapSample"; break;
                        case 3: stateName = "storageSample"; break;
                    }

                    GamePage gamePage = App.GamePage;

                    // Prepare demo
                    gamePage.LaunchDemo(stateName);

                    // Switch to the GamePage
                    Window.Current.Content = gamePage;
                    Window.Current.Activate();
                }
            }
        }
    }
}
