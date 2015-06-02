using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMP_GeoCaching_Peilen.Model;

namespace AMP_GeoCaching_Peilen.ViewModel
{
    class ComboBoxViewModel
    {
        public ObservableCollection<Item> Items { get; set; }

        public void GetItems()
        {
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.Count > 0)
            {
                GetSavedItems();
            }
            else
            {
                GetDefaultItems();
            }
        }

        public void GetDefaultItems()
        {
            ObservableCollection<Item> a = new ObservableCollection<Item>();

            // Items for the ComboBox
            a.Add(new Item() { Name = "N50°25.123', E006°45.000'", Value = "1" });
            a.Add(new Item() { Name = "N50.418716° , E006.750000°", Value = "2 " });
            a.Add(new Item() { Name = "N50°25' 07.4'', E006°45' 00.0''", Value = "3" });

            Items = a;
        }

        public void GetSavedItems()
        {
            ObservableCollection<Item> a = new ObservableCollection<Item>();

            foreach (Object o in Windows.Storage.ApplicationData.Current.LocalSettings.Values)
            {
                a.Add((Item)o);
            }

            Items = a;
        }
    }
}
