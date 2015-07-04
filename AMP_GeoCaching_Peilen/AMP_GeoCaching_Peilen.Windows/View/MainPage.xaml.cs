using AMP.GeoCachingTools.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace AMP.GeoCachingTools
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Frames navigiert werden kann.
    /// </summary>
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

        private void berechnePosition(object sender, RoutedEventArgs e)
        {
            bvm.berechnePosition(tBox_initialLongitude.Text, tBox_initialLatitude.Text, tBox_Distance.Text, tBox_Direction.Text);
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
