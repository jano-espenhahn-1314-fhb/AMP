using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AMP_GeoCaching_Peilen.Model
{
    class Item : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public object Value { get; set; }

        // The number of each item that has been collected.
        private int _count;
        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
                RaisePropertyChanged("Count");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
