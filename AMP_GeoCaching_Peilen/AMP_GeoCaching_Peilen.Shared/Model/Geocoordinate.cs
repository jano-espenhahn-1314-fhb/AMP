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
            get { return this._coordinate; }
            set
            {
                this._coordinate = value;
                RaisePropertyChanged("Coordinate");
            }
        }
   }
}
