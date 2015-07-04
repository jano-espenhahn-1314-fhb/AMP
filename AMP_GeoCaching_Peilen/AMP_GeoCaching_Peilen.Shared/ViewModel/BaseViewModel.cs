using System;
using System.Windows;
using System.Collections.ObjectModel;
using AMP.GeoCachingTools.Model;
using System.ComponentModel;
using System.Linq;
using Windows.Devices.Geolocation;
using AMP.GeoCachingTools.Commons;

namespace AMP.GeoCachingTools.ViewModel
{
    public class BaseViewModel : BaseINPC
    {
        public ObservableCollection<Item> Items { get; set; }

        public GeoCoordinate initialCoordinate { get; set; }

        public GeoCoordinate targetCoordinate { get; set; }

        public Exception LocationException { get; set; }

        public string distance { get; set; }

        public string direction { get; set; }

        public bool LocationSettingIsActive;

        private Item selectedItem;
        public Item SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                NotifyPropertyChanged("Item");
            }
        }

        /*
         * Default constructor
         */
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
            Items.Add(new Item() { Name = "50°25.123', 006°45.000'", Value = "1" });
            Items.Add(new Item() { Name = "50.418716° , 006.750000°", Value = "2 " });
            Items.Add(new Item() { Name = "50°25' 07.4'', 006°45' 00.0''", Value = "3" });
        }

        /*
         * Calculate the target Geoposition coordinates
         */
        public void calculatePosition()
        {
            // Helper
            double longitude;
            double latitude;
            double dist;
            double dire;

            // Bind to/from UI and parse values
            Double.TryParse(initialCoordinate.LongitudeCoordinate, out longitude);
            Double.TryParse(initialCoordinate.LatitudeCoordinate, out latitude);
            Double.TryParse(distance, out dist);
            Double.TryParse(direction, out dire);

            /*
             * Distance = ((way from initial to target) / (radius of the earth)) * ((180 * 60') / PI)
             *          =  (way from initial to target) / (1853m)
             */
            dist = (dist / 1853);

            // DeltaLongitude = ((way from initial to target) / (1853m)) * (cos(direction)) 
            double deltaLongitude = (dist * Math.Cos(dire)) * (180 / Math.PI);

            // Target-coordinate: initial longitude + deltaLongitude
            double longi = longitude + deltaLongitude;

            // DeltaLongitude = ((way from initial to target) / (1853m)) * (sin(direction)/cos(initial longitude + deltaLongitude)
            double deltaLatitude = dist * (Math.Sin(dire) / Math.Cos(longi));

            // Target-coordinate: initial latitude + deltaLatitude
            double lati = latitude + deltaLatitude;

            // Bind to UI
            targetCoordinate.LongitudeCoordinate = longi.ToString();
            targetCoordinate.LatitudeCoordinate = lati.ToString();
        }

        /*
         * Get the actual Geoposition, if Locationsetting is active
         */
        public async void getPosition()
        {   
            // Default values
            LocationSettingIsActive = true;
            LocationException = null;

            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 100;

            if (geolocator.LocationStatus == PositionStatus.Disabled)
            {
                LocationSettingIsActive = false;

                // Blank Exception - only for Exceptionhandling in the View
                LocationException = new Exception();
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
                    LocationException = ex;
                    System.Diagnostics.Debug.WriteLine("Fehler : " + ex.Message.ToString());
                }
            }
        }

    }

}