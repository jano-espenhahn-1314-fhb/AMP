using AMP.GeoCachingTools.Commons;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMP.GeoCachingTools.Model
{
    public class GeoCoordinate : BaseINPC
    {
    
        private string _longitudeCoordinate;

        public string LongitudeCoordinate
        {
            get { return _longitudeCoordinate; }
            set
            {
                _longitudeCoordinate = value;
                NotifyPropertyChanged("LongitudeCoordinate");
            }
        }

        private string _latitudeCoordinate;

        public string LatitudeCoordinate
        {
            get { return _latitudeCoordinate; }
            set
            {
                _latitudeCoordinate = value;
                NotifyPropertyChanged("LatitudeCoordinate");
            }
        }

   }
}
