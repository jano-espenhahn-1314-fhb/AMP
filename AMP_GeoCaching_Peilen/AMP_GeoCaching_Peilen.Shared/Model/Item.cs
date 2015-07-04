using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AMP.GeoCachingTools.Commons;

namespace AMP.GeoCachingTools.Model
{
    public class Item : BaseINPC
    {

        private string _name;

        // The name of the item.
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private string _value;

        // The Value of the item.
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                NotifyPropertyChanged("Value");
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Item && ((Item)obj).Value.Equals(Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}