using System;
using System.Windows;
using System.Collections.ObjectModel;
using AMP.GeoCachingTools.Model;
using System.ComponentModel;
using System.Linq;

namespace AMP.GeoCachingTools.ViewModel
{
    public class BaseViewModel : BaseINPC
    {
        public ObservableCollection<Item> Items { get; set; }

        public GeoCoordinate targetLongitude { get; set; }

        public GeoCoordinate targetLatitude { get; set; }

        //private double longitude { get; set; }

        //private double latitude { get; set; }

        //private double distance { get; set; }

        //private double direction { get; set; }

        public BaseViewModel()
        {
            // Actions for combobox at first page
            this.Items = new ObservableCollection<Item>();
            this.GetItems();
            this.SelectedItem = this.Items.FirstOrDefault();

            targetLongitude = new GeoCoordinate();
            targetLatitude = new GeoCoordinate();
        }

        private Item selectedItem;
        public Item SelectedItem
        {
            get { return this.selectedItem; }
            set
            {
                this.selectedItem = value;
                RaisePropertyChanged("Item");
            }
        }

        private void GetItems()
        {
            // Fill the ComboBox with Items
            this.Items.Add(new Item() { Name = "50°25.123', 006°45.000'", Value = "1" });
            this.Items.Add(new Item() { Name = "50.418716° , 006.750000°", Value = "2 " });
            this.Items.Add(new Item() { Name = "50°25' 07.4'', 006°45' 00.0''", Value = "3" });
        }

        public void BerechnePosition()
        {
            double longitude = 25.123;

            double latitude = 45.000;

            double distance = 1000;

            double direction = 60;

            double deltaLongitude = ((distance / 1853) * Math.Cos(direction)) * (180 / Math.PI);

            System.Diagnostics.Debug.WriteLine("deltaLongitude = " + deltaLongitude.ToString());

            // Target-coordinate: longitude
            double longi = longitude + deltaLongitude;

            double deltaLatitude = (distance / 1853) * (Math.Sin(direction) / Math.Cos(longitude + longi));

            System.Diagnostics.Debug.WriteLine("deltaLatitude = " + deltaLatitude.ToString());

            // Target-coordinate: latitude
            double lati = latitude + deltaLatitude;

            this.targetLongitude.Coordinate = longi.ToString();
            this.targetLatitude.Coordinate = lati.ToString();
        }

    }      

}