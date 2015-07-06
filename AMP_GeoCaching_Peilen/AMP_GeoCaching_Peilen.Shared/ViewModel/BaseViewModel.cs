using System;
using System.Windows;
using System.Collections.ObjectModel;
using AMP.GeoCachingTools.Model;
using System.ComponentModel;
using System.Linq;
using Windows.Devices.Geolocation;
using AMP.GeoCachingTools.Commons;
using System.Collections.Generic;

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
                if (value != _selectedItem)
                {
                    PreviousSelectedItem = _selectedItem;
                }

                _selectedItem = value;
                NotifyPropertyChanged("Item");
            }
        }

        private Item PreviousSelectedItem;

        private double longitude;

        private double latitude;

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
            Items.Add(new Item() { Name = "52° 31.249', 13° 24.567'", Value = Constants.DegreesMinutes });
            // Decimal degrees format
            Items.Add(new Item() { Name = "52.520817, 13.40945", Value = Constants.Degrees });
            // Degres, minutes and seconds format
            Items.Add(new Item() { Name = "52° 31' 14.941'', 13° 24' 34.020''", Value = Constants.DegreesMinutesSeconds });
        }

        /*
         * Calculate the target Geoposition coordinates
         */
        public void calculatePosition()
        {
            // Default = no exception
            Exception = null;

            // Helper
            longitude = -1;
            latitude = -1;
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
                    longitude = convertTo(initialCoordinate.LongitudeCoordinate, SelectedItem.Value);
                    latitude = convertTo(initialCoordinate.LatitudeCoordinate, SelectedItem.Value);

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

                    initialCoordinate.LongitudeCoordinate = convertBack(geoposition.Coordinate.Point.Position.Longitude, SelectedItem.Value);
                    initialCoordinate.LatitudeCoordinate = convertBack(geoposition.Coordinate.Point.Position.Latitude, SelectedItem.Value);
                }
                catch (Exception ex)
                {
                    Exception = ex;
                    System.Diagnostics.Debug.WriteLine("Fehler : " + ex.Message.ToString());
                }
            }
        }

        // Convert the input values into the needed formats for calculation
        private double convertTo(string value, string format)
        {
            double result = 0;

            switch (format)
            {
                case Constants.DegreesMinutes:
                    result = converter.convertDegreesMinutesToDegrees(value);
                    break;

                case Constants.Degrees:
                    result = converter.convertDegreesStringToDegrees(value);
                    break;

                case Constants.DegreesMinutesSeconds:
                    result = converter.convertDegreesMinutesSecondsToDegrees(value);
                    break;
            }

            return result;
        }

        // Validate the values for the needed formats, if not valid throw exception
        private bool validateFormat(string format)
        {
            validator = new Validator(initialCoordinate);
            bool isValid = true;

            switch (format)
            {
                case Constants.DegreesMinutes:
                    if (!validator.validateFormat(Constants.DegreesMinutes))
                    {
                        Exception = new Exception("DegreesMinutes");
                        isValid = false;
                    }
                    break;

                case Constants.Degrees:
                    if (!validator.validateFormat(Constants.Degrees))
                    {
                        Exception = new Exception("Degrees");
                        isValid = false;
                    }
                    break;

                case Constants.DegreesMinutesSeconds:
                    if (!validator.validateFormat(Constants.DegreesMinutesSeconds))
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
                case Constants.DegreesMinutes:
                    result = converter.convertDegreesToDegreesMinutes(value);
                    break;

                case Constants.Degrees:
                    result = converter.convertDegreesToDegreesString(value);
                    break;

                case Constants.DegreesMinutesSeconds:
                    result = converter.convertDegreesToDegreesMinutesSeconds(value);
                    break;
            }

            return result;
        }

        // If the combobox change their value, change the format of the coordinates
        public void changeCoordinates()
        {
            // Input values
            if (initialCoordinate.LongitudeCoordinate != null && initialCoordinate.LatitudeCoordinate != null)
            {
                initialCoordinate.LongitudeCoordinate = convertBack(convertTo(initialCoordinate.LongitudeCoordinate, PreviousSelectedItem.Value), SelectedItem.Value);
                initialCoordinate.LatitudeCoordinate = convertBack(convertTo(initialCoordinate.LatitudeCoordinate, PreviousSelectedItem.Value), SelectedItem.Value);
            }

            // Output values
            if (targetCoordinate.LongitudeCoordinate != null && targetCoordinate.LatitudeCoordinate != null)
            {
                targetCoordinate.LongitudeCoordinate = convertBack(convertTo(targetCoordinate.LongitudeCoordinate, PreviousSelectedItem.Value), SelectedItem.Value);
                targetCoordinate.LatitudeCoordinate = convertBack(convertTo(targetCoordinate.LatitudeCoordinate, PreviousSelectedItem.Value), SelectedItem.Value);
            }
        }

    }
}
