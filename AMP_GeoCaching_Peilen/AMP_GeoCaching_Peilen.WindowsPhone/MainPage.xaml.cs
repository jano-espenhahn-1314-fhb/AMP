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
            bvm.berechnePosition();
        }


        private async void getPosition(object sender, RoutedEventArgs e)
        {
            string messageDialogTitle = "Fehler bei Ortung!";
            string messageDialogContent;

            bvm.getPosition();

            if (bvm.LocationException != null)
            {
                if (!bvm.LocationSettingIsActive)
                {
                    // TODO: Überlegung, hier vielleicht besser Nutzer direkt in die Einstellungen zu schicken?
                    messageDialogContent = "Bitte aktivieren Sie die Ortung in den Einstellungen.";
                }
                else
                {
                    messageDialogContent = "Es ist ein Fehler bei der Ortung aufgetreten.";
                }

                MessageDialog msg = new MessageDialog(messageDialogContent, messageDialogTitle);

                msg.Commands.Add(new UICommand("Ok", new UICommandInvokedHandler(CommandHandlers)));
                msg.Commands.Add(new UICommand("Schliessen", new UICommandInvokedHandler(CommandHandlers)));

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
                    // TODO
                    break;
                case "Schliessen":
                    Application.Current.Exit();
                    break;
            }
        }
    }
}
