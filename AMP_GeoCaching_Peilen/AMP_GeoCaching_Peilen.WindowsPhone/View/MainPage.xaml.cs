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
using Windows.UI.Popups;

namespace AMP.GeoCachingTools
{

    public sealed partial class MainPage : Page
    {

        private BaseViewModel bvm;

        public MainPage()
        {
            init();
        }

        private void init()
        {
            InitializeComponent();

            bvm = new BaseViewModel();

            //Set the data context of the window
            DataContext = bvm;

            NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void calculatePosition(object sender, RoutedEventArgs e)
        {
            bvm.calculatePosition();
        }


        private async void getPosition(object sender, RoutedEventArgs e)
        {
            string messageDialogTitle;
            string messageDialogContent;

            bvm.getPosition();

            if (bvm.LocationException != null)
            {
                if (!bvm.LocationSettingIsActive)
                {
                    messageDialogTitle = "Ortung inaktiv!";
                    messageDialogContent = "Bitte aktivieren Sie die Ortung in den Einstellungen.";
                }
                else
                {
                    messageDialogTitle = "Fehler bei Ortung!";
                    messageDialogContent = "Es ist ein Fehler bei der Ortung aufgetreten.";
                }

                MessageDialog msg = new MessageDialog(messageDialogContent, messageDialogTitle);

                msg.Commands.Add(new UICommand("Ok", new UICommandInvokedHandler(CommandHandlers)));

                await msg.ShowAsync();
            }

        }

        private void delete(object sender, RoutedEventArgs e)
        {
            init();
        }

        public void CommandHandlers(IUICommand commandLabel)
        {
            var Actions = commandLabel.Label;
            switch (Actions)
            {
                case "Ok":
                    // nothing to do
                    break;
            }
        }
    }
}
