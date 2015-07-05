using AMP.GeoCachingTools.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMP_GeoCaching_Peilen.Commons
{
    class Converter
    {

        // Convert from '50 25.123' to '50.4187166666'
        public string convertDegreesMinutesToDegrees(string coordinate)
        {
            double degrees;
            double arcminutes;

            string[] values = coordinate.Split(' ');

            Double.TryParse(values[0], out degrees);
            Double.TryParse(values[1], out arcminutes);

            // Arcminutes / 1000  = solution for decimal and (solution / 60) for decimal degrees
            arcminutes = arcminutes / 1000 / 60;
            // We have to add the decimal degrees from arcminutes to the integer of degrees
            degrees = degrees + arcminutes;

            return degrees.ToString().Replace(",", ".");
        }

        // Convert from '52 31 14.941' to '50.4187166666'
        public string convertDegreesMinutesSecondsToDegrees(string coordinate)
        {
            double degrees;
            double arcminutes;
            double arcseconds;

            string[] values = coordinate.Split(' ');

            Double.TryParse(values[0], out degrees);
            Double.TryParse(values[1], out arcminutes);
            Double.TryParse(values[2], out arcseconds);

            // Arcseconds / 1000  = solution for decimal and (solution / 60) for arcminutes
            arcseconds = arcseconds / 1000 / 60;
            // Arcminutes / 1000  = solution for decimal and (solution / 60) for decimal degrees and add the arcseconds
            arcminutes = (arcminutes / 1000 / 60) + arcseconds;
            // We have to add the decimal degrees from arcminutes to the integer of degrees
            degrees = degrees + arcminutes;

            return degrees.ToString().Replace(",", ".");
        }

    }
}
