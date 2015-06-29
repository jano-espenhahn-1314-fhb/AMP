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

        private double longitude;

        private double latitude;

        public BaseViewModel()
        {
            this.Items = new ObservableCollection<Item>();
            this.GetItems();

            this.SelectedItem = this.Items.FirstOrDefault();
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
            this.Items.Add(new Item() { Name = "N50°25.123', E006°45.000'", Value = "1" });
            this.Items.Add(new Item() { Name = "N50.418716° , E006.750000°", Value = "2 " });
            this.Items.Add(new Item() { Name = "N50°25' 07.4'', E006°45' 00.0''", Value = "3" });
        }

        public void BerechnePosition()
        {
            double distance;
            double direction;

           // longitude = (distance / 1850) * Math.Cos(direction);
        }

    }
}