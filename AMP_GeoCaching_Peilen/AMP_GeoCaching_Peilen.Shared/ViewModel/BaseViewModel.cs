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

        public GeoCoordinate initialLongitude { get; set; }

        public GeoCoordinate initialLatitude { get; set; }

        public string distance { get; set; }

        public string direction { get; set; }

        public GeoCoordinate targetLongitude { get; set; }

        public GeoCoordinate targetLatitude { get; set; }

        public Exception LocationException { get; set; }

        public bool LocationSettingIsActive;

        public BaseViewModel()
        {
            // Actions for combobox at first page
            Items = new ObservableCollection<Item>();
            GetItems();
            SelectedItem = Items.FirstOrDefault();

            targetLongitude = new GeoCoordinate();
            targetLatitude = new GeoCoordinate();
        }

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

        // Fill the ComboBox with Items
        private void GetItems()
        {
            Items.Add(new Item() { Name = "50°25.123', 006°45.000'", Value = "1" });
            Items.Add(new Item() { Name = "50.418716° , 006.750000°", Value = "2 " });
            Items.Add(new Item() { Name = "50°25' 07.4'', 006°45' 00.0''", Value = "3" });
        }

        public void berechnePosition(String initialLongitude, String initialLatitude, String distance, String direction)
        {
            // Helper with default values for fallback
            double longitude = 52.000000;
            double latitude = 13.000000;
            double dist = 10000;
            double dire = 60;

            // Bind to/from UI and parse values
            Double.TryParse(initialLongitude, out longitude);
            Double.TryParse(initialLatitude, out latitude);
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
            targetLongitude.Coordinate = longi.ToString();
            targetLatitude.Coordinate = lati.ToString();
        }

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

                    initialLongitude.Coordinate = (geoposition.Coordinate.Longitude.ToString("0.000000") + "°");
                    initialLatitude.Coordinate = (geoposition.Coordinate.Latitude.ToString("0.000000") + "°");
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