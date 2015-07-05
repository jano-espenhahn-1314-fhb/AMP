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

        private string messageDialogTitle;

        private string messageDialogContent;

        public MainPage()
        {
            init();
        }

        // Initialize the components; own method because of deletion function in the UI
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

            if (bvm.Exception != null)
            {
                // Exceptionhandling for empty fields
                if (bvm.Exception.Message.Equals("emptyFields"))
                {
                    string dialogHelper;
                    string dialogHelperVerb;

                    if (bvm.tbvm.emptyFields.Count == 1)
                    {
                        dialogHelper = "Das Feld ";
                        dialogHelperVerb = " ist ";

                        foreach (string field in bvm.tbvm.emptyFields)
                        {
                            dialogHelper = dialogHelper + field;
                        }
                    }
                    else
                    {
                        dialogHelper = "Die Felder ";
                        dialogHelperVerb = " sind ";

                        int length = bvm.tbvm.emptyFields.Count - 1;

                        for (int counter = 0; counter <= length; counter++)
                        {
                            if (counter < length - 1)
                            {
                                dialogHelper = dialogHelper + bvm.tbvm.emptyFields[counter] + ", ";
                            }
                            else if (counter == length - 1)
                            {
                                dialogHelper = dialogHelper + bvm.tbvm.emptyFields[counter] + " und ";
                            }
                            else
                            {
                                dialogHelper = dialogHelper + bvm.tbvm.emptyFields[counter];
                            }

                        }
                    }

                    messageDialogTitle = "Ungültige Parametrisierung!";
                    messageDialogContent = dialogHelper + dialogHelperVerb + "leer. Bitte geben Sie einen Wert ein.";

                    createMessageDialog(messageDialogContent, messageDialogTitle);
                }
                // Exceptionhandling for wrong values like '2132dsfd'
                else if (bvm.Exception.Message.Equals("wrongValue"))
                {
                    messageDialogTitle = "Ungültiger Wert!";
                    messageDialogContent = "Sie haben einen ungültigen Wert eingegeben. Bitte geben Sie eine Zahl im Format 'x.xx' ein";

                    createMessageDialog(messageDialogContent, messageDialogTitle);
                }
            }
        }


        private void getPosition(object sender, RoutedEventArgs e)
        {
            bvm.getPosition();

            // Exceptionhandling for disabled locationsettings or other errors
            if (bvm.Exception != null)
            {
                if (bvm.Exception.Message.Equals("LocationSettingIsDisabled"))
                {
                    messageDialogTitle = "Ortung inaktiv!";
                    messageDialogContent = "Bitte aktivieren Sie die Ortung in den Einstellungen.";
                }
                else
                {
                    messageDialogTitle = "Fehler bei Ortung!";
                    messageDialogContent = "Es ist ein Fehler bei der Ortung aufgetreten.";
                }

                createMessageDialog(messageDialogContent, messageDialogTitle);
            }

        }

        private void delete(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void CommandHandlers(IUICommand commandLabel)
        {
            var Actions = commandLabel.Label;
            switch (Actions)
            {
                case "Ok":
                    // nothing to do
                    break;
            }
        }

        private async void createMessageDialog(string messageDialogContent, string messageDialogTitle)
        {
            MessageDialog msg = new MessageDialog(messageDialogContent, messageDialogTitle);

            msg.Commands.Add(new UICommand("Ok", new UICommandInvokedHandler(CommandHandlers)));

            await msg.ShowAsync();
        }
    }
}
