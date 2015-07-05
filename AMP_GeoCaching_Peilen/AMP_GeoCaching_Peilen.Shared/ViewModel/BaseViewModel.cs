using System;
using System.Windows;
using System.Collections.ObjectModel;
using AMP.GeoCachingTools.Model;
using System.ComponentModel;
using System.Linq;
using Windows.Devices.Geolocation;
using AMP.GeoCachingTools.Commons;
using System.Collections.Generic;
using AMP_GeoCaching_Peilen.Commons;

namespace AMP.GeoCachingTools.ViewModel
{
    public class BaseViewModel : BaseINPC
    {
        public ObservableCollection<Item> Items { get; set; }

        public GeoCoordinate initialCoordinate { get; set; }

        public GeoCoordinate targetCoordinate { get; set; }

        public Exception Exception { get; set; }

        public Validator validator;

        public string distance { get; set; }

        public string direction { get; set; }

        private Converter converter = new Converter();

        private Item _selectedItem;
        public Item SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged("Item");
            }
        }

        // The format examples as string

        public const string DegreesMinutes = "52° 31.249'";

        public const string Degrees = "52.520817";

        public const string DegreesMinutesSeconds = "52° 31' 14.941''";
        
        //Default constructor
        public BaseViewModel()
        {
            // Actions for combobox at first page
            Items = new ObservableCollection<Item>();
            GetItems();
            SelectedItem = Items.FirstOrDefault();

            initialCoordinate = new GeoCoordinate();
            targetCoordinate = new GeoCoordinate();
        }

        // Fill the ComboBox with Items
        private void GetItems()
        {
            // Degrees and minutes format
            Items.Add(new Item() { Name = "52° 31.249', 13° 24.567'", Value = DegreesMinutes });
            // Decimal degrees format
            Items.Add(new Item() { Name = "52.520817, 13.40945", Value = Degrees });
            // Degres, minutes and seconds format
            Items.Add(new Item() { Name = "52° 31' 14.941'', 13° 24' 34.020''", Value = DegreesMinutesSeconds });
        }

        /*
         * Calculate the target Geoposition coordinates
         */
        public void calculatePosition()
        {
            // Default = no exception
            Exception = null;

            // Helper
            double longitude = -1;
            double latitude = -1;
            double dist = -1;
            double dire = -1;
            bool isCalculable = false;
            string longi;
            string lati;

            // Check the values of the Textboxes
            validator = new Validator(initialCoordinate, distance, direction);
            validator.checkTextboxes();

            if (validator.emptyFields.Count > 0)
            {
                // Exception for exceptionhandling in the view
                Exception = new Exception("emptyFields");
            }
            else
            {
                // Bind to/from UI
                for (int counter = 0; counter < validator.validatedValues.Count(); counter++)
                {
                    if (validator.validatedValues[counter] == -1)
                    {
                        Exception = new Exception("wrongValue");
                        break;
                    }

                    if (counter == 0)
                    {
                        dist = validator.validatedValues[counter];
                    }
                    else if (counter == 1)
                    {
                        dire = validator.validatedValues[counter];
                        isCalculable = true;
                    }
                }

                // Check the format of the coordinates
                if (!validateFormat(SelectedItem.Value))
                {
                    isCalculable = false;
                }

                // Degrees in direction have to be between 0° and 360°
                if (!validator.isInDirectionRange(dire))
                {
                    Exception = new Exception("notInRange");
                    isCalculable = false;
                }

                // if all values are correct, than it's calculabe, so calculate
                if (isCalculable)
                {
                    // Convert the input values to the needed format
                    convertTo(SelectedItem.Value);

                    Double.TryParse(initialCoordinate.LongitudeCoordinate, out longitude);
                    Double.TryParse(initialCoordinate.LatitudeCoordinate, out latitude);

                    Calculator calc = new Calculator(longitude, latitude, dist, dire);
                    calc.calculate();
                    longi = convertBack(calc.longitude, SelectedItem.Value);
                    lati = convertBack(calc.latitude, SelectedItem.Value);
                } 
                else
                {
                    longi = "";
                    lati = "";
                }

                targetCoordinate.LongitudeCoordinate = longi;
                targetCoordinate.LatitudeCoordinate = lati;
            }
        }

        // Get the actual Geoposition, if Locationsetting is active
        public async void getPosition()
        {
            // Default = no exception
            Exception = null;

            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 100;

            if (geolocator.LocationStatus == PositionStatus.Disabled)
            {
                // Exception for exceptionhandling in the view
                Exception = new Exception("LocationSettingIsDisabled");
            } 
            else
            { 
                try
                {
                    Geoposition geoposition = await geolocator.GetGeopositionAsync(
                         maximumAge: TimeSpan.FromMinutes(5),
                         timeout: TimeSpan.FromSeconds(10)
                        );

                    initialCoordinate.LongitudeCoordinate = (geoposition.Coordinate.Longitude.ToString("0.000000") + "°");
                    initialCoordinate.LatitudeCoordinate = (geoposition.Coordinate.Latitude.ToString("0.000000") + "°");
                }
                catch (Exception ex)
                {
                    Exception = ex;
                    System.Diagnostics.Debug.WriteLine("Fehler : " + ex.Message.ToString());
                }
            }
        }

        // Convert the input values into the needed formats for calculation
        private void convertTo(string format)
        {
            
            switch (format)
            {
                case DegreesMinutes:
                    initialCoordinate.LongitudeCoordinate = converter.convertDegreesMinutesToDegrees(initialCoordinate.LongitudeCoordinate);
                    initialCoordinate.LatitudeCoordinate = converter.convertDegreesMinutesToDegrees(initialCoordinate.LatitudeCoordinate);
                    break;

                case DegreesMinutesSeconds:
                    initialCoordinate.LongitudeCoordinate = converter.convertDegreesMinutesSecondsToDegrees(initialCoordinate.LongitudeCoordinate);
                    initialCoordinate.LatitudeCoordinate = converter.convertDegreesMinutesSecondsToDegrees(initialCoordinate.LatitudeCoordinate);
                    break;
            }

        }

        // Validate the values for the needed formats, if not valid throw exception
        private bool validateFormat(string format)
        {
            validator = new Validator(initialCoordinate);
            bool isValid = true;

            switch (format)
            {
                case DegreesMinutes:
                    if (!validator.validateFormat(DegreesMinutes))
                    {
                        Exception = new Exception("DegreesMinutes");
                        isValid = false;
                    }
                    break;

                case Degrees:
                    if (!validator.validateFormat(Degrees))
                    {
                        Exception = new Exception("Degrees");
                        isValid = false;
                    }
                    break;

                case DegreesMinutesSeconds:
                    if (!validator.validateFormat(DegreesMinutesSeconds))
                    {
                        Exception = new Exception("DegreesMinutesSeconds");
                        isValid = false;
                    }
                    break;
            }
            
            return isValid;
        }

        // Convert calculated values into the needed formats for output
        private string convertBack(double value, string format)
        {
            string result = "";

            switch (format)
            {
                case DegreesMinutes:
                    result = converter.convertDegreesToDegreesMinutes(value);
                    break;

                case DegreesMinutesSeconds:
                    result = converter.convertDegreesToDegreesMinutesSeconds(value);
                    break;
            }

            return result;
        }

    }
}
