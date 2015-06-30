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
using Windows.Devices.Geolocation;

namespace AMP.GeoCachingTools
{

    public partial class MainPage : Page
    {

        private BaseViewModel bvm;

        public MainPage()
        {
            init();
        }

        private void init()
        {
            this.InitializeComponent();

            bvm = new BaseViewModel();
            //Set the data context of the window
            DataContext = bvm;

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void berechnePosition(object sender, RoutedEventArgs e)
        {
            bvm.BerechnePosition();
        }

        private async void getPosition(object sender, RoutedEventArgs e)
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 100;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                     maximumAge: TimeSpan.FromMinutes(5),
                     timeout: TimeSpan.FromSeconds(10)
                    );

                //With this 2 lines of code, the app is able to write on a Text Label the Latitude and the Longitude
                tBox_Longitude.Text = (geoposition.Coordinate.Longitude.ToString("0.000000") + "°");
                tBox_Latitude.Text = (geoposition.Coordinate.Latitude.ToString("0.000000") + "°");
            }
            //If an error is catch 2 are the main causes: the first is that you forgot to include ID_CAP_LOCATION in your app manifest. 
            //The second is that the user doesn't turned on the Location Services
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message.ToString());
            }

        }

        private void delete(object sender, RoutedEventArgs e)
        {
            init();
        }
    }
}
