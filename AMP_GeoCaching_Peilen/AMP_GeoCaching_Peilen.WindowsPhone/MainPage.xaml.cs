using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AMP.GeoCachingTools.ViewModel;
using AMP.GeoCachingTools;

namespace AMP.GeoCachingTools
{

    public partial class MainPage : Page
    {

        private BaseViewModel bvm;

        public MainPage()
        {
            this.InitializeComponent();

            bvm = new BaseViewModel();
            //Set the data context of the window
            DataContext = bvm;

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Wird aufgerufen, wenn diese Seite in einem Rahmen angezeigt werden soll.
        /// </summary>
        /// <param name="e">Ereignisdaten, die beschreiben, wie diese Seite erreicht wurde.
        /// Dieser Parameter wird normalerweise zum Konfigurieren der Seite verwendet.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void BerechnePosition(object sender, RoutedEventArgs e)
        {
            //bvm.BerechnePosition();

            Frame.Navigate(typeof(TargetPage));

            // for later
            // Frame.Navigate(typeof(TargetPage), param);
        }
    }
}
