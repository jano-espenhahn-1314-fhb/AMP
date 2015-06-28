using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AMP.GeoCachingTools
{
    public abstract class BaseINPC : INotifyPropertyChanged  
    {
        public bool IsDirty { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged; 

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            IsDirty = true;
        }

        public void Save()
        {
            IsDirty = false;
        }
    }
}
