using AMP.GeoCachingTools.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMP.GeoCachingTools.Commons
{
    class Converter
    {

        // Convert from '50 25.123' to '50.4187166666'
        public double convertDegreesMinutesToDegrees(string coordinate)
        {
            double degrees;
            double arcminutes;
            string[] finalValues = new string[2];
            string[] values = coordinate.Split(' ');
            int length = values.Length;

            for (int count = 0; count < length; count++)
            {
                finalValues[count] = values[count];
            }

            if (length < finalValues.Length)
            {
                finalValues[1] = "0";
            }

            Double.TryParse(finalValues[0], out degrees);
            Double.TryParse(finalValues[1], out arcminutes);

            // Arcminutes / 1000  = solution for decimal and (solution / 60) for decimal degrees
            arcminutes = arcminutes / 1000 / 60;
            // We have to add the decimal degrees from arcminutes to the integer of degrees
            degrees = degrees + arcminutes;

            return degrees;
        }

        // Convert from '10.102' to '10,102'
        public double convertDegreesStringToDegrees(string coordinate)
        {
            double degrees;
            Double.TryParse(coordinate.Replace(".", ","), out degrees);
            return degrees;
        }

        // Convert from '52 31 14.941' to '50.4187166666'
        public double convertDegreesMinutesSecondsToDegrees(string coordinate)
        {
            double degrees;
            double arcminutes;
            double arcseconds;
            string[] finalValues = new string[3];
            string[] values = coordinate.Split(' ');
            int length = values.Length;

            for (int count = 0; count < length; count++)
            {
                finalValues[count] = values[count];
            }

            if (length == 1)
            {
                finalValues[1] = "0";
            }
            else if (length == 2)
            {
                finalValues[2] = "0";
            }

            Double.TryParse(finalValues[0], out degrees);
            Double.TryParse(finalValues[1], out arcminutes);
            Double.TryParse(finalValues[2], out arcseconds);

            // Arcseconds / 1000  = solution for decimal and (solution / 60) for arcminutes
            arcseconds = arcseconds / 1000 / 60;
            // Arcminutes / 1000  = solution for decimal and (solution / 60) for decimal degrees and add the arcseconds
            arcminutes = (arcminutes / 1000 / 60) + arcseconds;
            // We have to add the decimal degrees from arcminutes to the integer of degrees
            degrees = degrees + arcminutes;

            return degrees;
        }

        // Convert from '50.4187166666' to '50 25.123'
        public string convertDegreesToDegreesMinutes(double coordinate)
        {
            double degrees = Math.Truncate(coordinate);
            double arcminutes = Math.Round((coordinate - degrees) * 60, 3);

            return degrees.ToString() + " " + arcminutes.ToString().Replace(",", ".");
        }

        // Convert from '10,102' to '10.102'
        public string convertDegreesToDegreesString(double coordinate)
        {
            return Math.Round(coordinate, 6).ToString().Replace(",", ".");
        }

        // Convert from '50.4187166666' to '50 25.123'
        public string convertDegreesToDegreesMinutesSeconds(double coordinate)
        {
            double degrees = Math.Truncate(coordinate);
            double arcminutesDecimal = coordinate - degrees;
            double arcminutes = Math.Truncate(arcminutesDecimal);
            double arcseconds = Math.Round((arcminutesDecimal - arcminutes) * 60, 3);

            return degrees.ToString() + " " + arcminutes.ToString().Replace(",", ".") + " " + arcseconds.ToString().Replace(",", ".");
        }

    }
}
