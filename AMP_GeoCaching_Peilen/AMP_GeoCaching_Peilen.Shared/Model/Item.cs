using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AMP_GeoCaching_Peilen.Model
{
    public class Item : BaseINPC
    {

        private string _name;

        // The name of the item.
        public string Name
        {
            get { return this._name; }
            set
            {
                this._name = value;
                RaisePropertyChanged("Name");
            }
        }

        private string _value;

        // The Value of the item.
        public string Value
        {
            get { return this._value; }
            set
            {
                this._value = value;
                RaisePropertyChanged("Value");
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