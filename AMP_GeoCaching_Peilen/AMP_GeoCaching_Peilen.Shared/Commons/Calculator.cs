using System;
using System.Collections.Generic;
using System.Text;

namespace AMP.GeoCachingTools.Commons
{

    class Calculator
    {

        public double longitude { get; set; }

        public double latitude { get; set; }

        private double distance;

        private double direction;

        public Calculator(double longitude, double latitude, double distance, double direction)
        {
            this.longitude = longitude;
            this.latitude = latitude;
            this.distance = distance;
            this.direction = direction;
        }

        // The main calculation
        public void calculate ()
        {
            // Distance comes in metres. We need it in kilometres.
            distance = distance / 1000;

            // Trigonometry : deltaLongitude = (cos(direction) * distance)
            // For longitudes : 1° is round about 111,12 km
            longitude = ((Math.Cos(-direction) * distance) / 111.12) + longitude;

            // Trigonometry : deltaLatitude = (sin(direction) * distance)
            // For latitudes : 1° is round about 73,62 km
            latitude = ((Math.Sin(-direction) * distance) / 73.62) + latitude;
        }

    }
}
