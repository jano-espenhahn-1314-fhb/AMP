using AMP.GeoCachingTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMP.GeoCachingTools.Model
{
    public class GeoCoordinate : BaseINPC
    {
    
        private string _coordinate;

        public string Coordinate
        {
            get { return _coordinate; }
            set
            {
                _coordinate = value;
                NotifyPropertyChanged("Coordinate");
            }
        }

        public override bool Equals(object obj)
        {
            return obj is GeoCoordinate && ((GeoCoordinate)obj).Coordinate.Equals(Coordinate);
        }

        public override int GetHashCode()
        {
            return Coordinate.GetHashCode();
        }
   }
}
