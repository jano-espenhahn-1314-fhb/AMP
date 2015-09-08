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
        public ObservableCollection<Item> items { get; set; }

        public GeoCoordinate initialCoordinate { get; set; }

        public GeoCoordinate targetCoordinate { get; set; }

        public Exception exception { get; set; }

        public Validator validator;

        public string distance { get; set; }

        public string direction { get; set; }

        private Calculator calculator = new Calculator();

        private Item _selectedItem;
        public Item selectedItem
        {
            get { return _selectedItem; }
            set
            {
                // Store the previous value
                if (value != _selectedItem)
                {
                    previousSelectedItem = _selectedItem;
                }

                _selectedItem = value;
                NotifyPropertyChanged("Item");
            }
        }

        private Item previousSelectedItem;

        private double longitude;

        private double latitude;

        //Default constructor
        public BaseViewModel()
        {
            // Actions for combobox at first page
            items = new ObservableCollection<Item>();
            GetItems();
            selectedItem = items.FirstOrDefault();

            initialCoordinate = new GeoCoordinate();
            targetCoordinate = new GeoCoordinate();
        }

        // Fill the ComboBox with items
        private void GetItems()
        {
            // Degrees and minutes format
            items.Add(new Item() { Name = "52° 31.249', 13° 24.567'", Value = Constants.DegreesMinutes });
            // Decimal degrees format
            items.Add(new Item() { Name = "52.520817, 13.40945", Value = Constants.Degrees });
            // Degres, minutes and seconds format
            items.Add(new Item() { Name = "52° 31' 14.941'', 13° 24' 34.020''", Value = Constants.DegreesMinutesSeconds });
        }

        /*
         * Calculate the target Geoposition coordinates
         */
        public void calculatePosition()
        {
            // Default = no exception
            exception = null;

            // Helper
            longitude = -1;
            latitude = -1;
            double dist = -1;
            double dire = -1;
            bool isCalculable = true;
            string longi;
            string lati;

            List<string> values = new List<string>();
            values.Add(distance);
            values.Add(direction);

            // Check the values of the Textboxes
            validator = new Validator(initialCoordinate, distance, direction);
            if (validator.checkEmptyTextboxes())
            {
                // exception for exceptionhandling in the view
                exception = new Exception("emptyFields");
            }
            else
            {
                // Bind to/from UI
                for (int counter = 0; counter < values.Count(); counter++)
                {
                    if (validator.validateDoubleValue(values[counter]) == 0)
                    {
                        exception = new Exception("wrongValue");
                        break;
                    }
                    else if (counter == 0)
                    {
                       dist = validator.validateDoubleValue(values[counter]);
                    }
                    else if (counter == 1)
                    {
                        dire = validator.validateDoubleValue(values[counter]);

                        // Degrees in direction have to be between 0° and 360°
                        if (!validator.isInDirectionRange(dire))
                        {
                            exception = new Exception("notInRange");
                            break;
                        }
                    }
                }

                // Check the format of the coordinates
                if (!validateFormat(selectedItem.Value))
                {
                    isCalculable = false;
                }

                // if all values are correct, than it's calculabe, so calculate
                if (isCalculable)
                {
                    System.Diagnostics.Debug.WriteLine("Breitengrad vor der Konvertierung : " + initialCoordinate.LatitudeCoordinate);
                    System.Diagnostics.Debug.WriteLine("Längengrad vor der Konvertierung : " + initialCoordinate.LongitudeCoordinate);

                    // Convert the input values to the needed format
                    longitude = convertTo(initialCoordinate.LongitudeCoordinate, selectedItem.Value);
                    latitude = convertTo(initialCoordinate.LatitudeCoordinate, selectedItem.Value);

                    System.Diagnostics.Debug.WriteLine("Breitengrad nach der Konvertierung : " + latitude.ToString());
                    System.Diagnostics.Debug.WriteLine("Längengrad nach der Konvertierung : " + longitude.ToString());

                    Calculator calc = new Calculator(longitude, latitude, dist, dire);
                    calc.calculate();
                    longi = convertBack(calc.longitude, selectedItem.Value);
                    lati = convertBack(calc.latitude, selectedItem.Value);
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
            exception = null;

            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 100;

            if (geolocator.LocationStatus == PositionStatus.Disabled)
            {
                // exception for exceptionhandling in the view
                exception = new Exception("LocationSettingIsDisabled");
            }
            else
            {
                try
                {
                    Geoposition geoposition = await geolocator.GetGeopositionAsync(
                         maximumAge: TimeSpan.FromMinutes(5),
                         timeout: TimeSpan.FromSeconds(10)
                        );

                    initialCoordinate.LongitudeCoordinate = convertBack(geoposition.Coordinate.Point.Position.Longitude, selectedItem.Value);
                    initialCoordinate.LatitudeCoordinate = convertBack(geoposition.Coordinate.Point.Position.Latitude, selectedItem.Value);
                }
                catch (Exception ex)
                {
                    exception = ex;
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
                    result = calculator.convertDegreesMinutesToDegrees(value);
                    break;

                case Constants.Degrees:
                    result = calculator.convertDegreesStringToDegrees(value);
                    break;

                case Constants.DegreesMinutesSeconds:
                    result = calculator.convertDegreesMinutesSecondsToDegrees(value);
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
                    if (!validator.validateFormat(format))
                    {
                        exception = new Exception("DegreesMinutes");
                        isValid = false;
                    }
                    break;

                case Constants.Degrees:
                    if (!validator.validateFormat(format))
                    {
                        exception = new Exception("Degrees");
                        isValid = false;
                    }
                    break;

                case Constants.DegreesMinutesSeconds:
                    if (!validator.validateFormat(format))
                    {
                        exception = new Exception("DegreesMinutesSeconds");
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
                    result = calculator.convertDegreesToDegreesMinutes(value);
                    break;

                case Constants.Degrees:
                    result = calculator.convertDegreesToDegreesString(value);
                    break;

                case Constants.DegreesMinutesSeconds:
                    result = calculator.convertDegreesToDegreesMinutesSeconds(value);
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
                initialCoordinate.LongitudeCoordinate = convertBack(convertTo(initialCoordinate.LongitudeCoordinate, previousSelectedItem.Value), selectedItem.Value);
                initialCoordinate.LatitudeCoordinate = convertBack(convertTo(initialCoordinate.LatitudeCoordinate, previousSelectedItem.Value), selectedItem.Value);
            }

            // Output values
            if (targetCoordinate.LongitudeCoordinate != null && targetCoordinate.LatitudeCoordinate != null)
            {
                targetCoordinate.LongitudeCoordinate = convertBack(convertTo(targetCoordinate.LongitudeCoordinate, previousSelectedItem.Value), selectedItem.Value);
                targetCoordinate.LatitudeCoordinate = convertBack(convertTo(targetCoordinate.LatitudeCoordinate, previousSelectedItem.Value), selectedItem.Value);
            }
        }

    }
}
