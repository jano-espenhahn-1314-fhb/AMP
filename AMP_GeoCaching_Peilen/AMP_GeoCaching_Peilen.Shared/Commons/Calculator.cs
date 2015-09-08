using System;
using System.Collections.Generic;
using System.Text;

namespace AMP.GeoCachingTools.Commons
{

    class Calculator
    {

        public double longitude { get; set; }

        public double latitude { get; set; }

        public double longitudeDegress { get; set; }

        public double latitudeDegress { get; set; }

        // The main calculation
        public void calculate (double lati, double longi, double distance, double direction)
        {
            double deltaLatitude = 0;
            double deltaLongitude = 0;

            // Trigonometry : Delta to North = (cos(direction) * distance)
            // New latitude = (Delta to North / 1850) + old latitude
            deltaLatitude = (Math.Cos(degreeToRadian(direction)) * distance) / 1850;
            lati += deltaLatitude;

            latitude = latitudeDegress + (lati / 60);

            // Trigonometry : Delta to East = (sin(direction) * distance)
            // New latitude = (Delta to North / 1850 * cos(latitude)) + old latitude
            deltaLongitude = (Math.Sin(degreeToRadian(direction)) * distance) / (1850 * Math.Cos(degreeToRadian(latitude)));
            longi += deltaLongitude;

            longitude = longitudeDegress + (longi / 60);
        }

        // Math.Cos and Math.Sin doesn't support input in degrees, 
        // that's why we have to convert it into radian
        private double degreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        // ############################
        // ## Methods for Converting ##
        // ############################

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
            double arcminutesDecimal = (coordinate - degrees) * 60;
            double arcminutes = Math.Truncate(arcminutesDecimal);
            double arcseconds = Math.Round((arcminutesDecimal - arcminutes) * 60, 3);

            return degrees.ToString() + " " + arcminutes.ToString().Replace(",", ".") + " " + arcseconds.ToString("N3").Replace(",", ".");
        }

        // if a arcminute or arcsecond has not six elements, the conversation is going to be failed
        private string fillWithZeros(string value)
        {
            return value.PadRight(6, '0');
        }

        // Convert from '50 25.123' to '50 25,123'
        public double convertDegreesMinutesStringToDegreesMinutes(string coordinate, bool? isLatitude)
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

            if (isLatitude == true)
            {
                latitudeDegress = degrees;
            }
            else if (isLatitude == false)
            {
                longitudeDegress = degrees;
            }

            arcminutes /= 1000 ;

            return arcminutes;
        }

        // Convert from '50.4187166666' to '50 25,123'
        public double convertDegreesStringToDegreesMinutes(string coordinate, bool? isLatitude)
        {
            double degrees;
            double arcminutes;
            string arcminuteString;
            coordinate = coordinate.Replace(".", ",");
            string[] finalValues = new string[2];
            string[] values = coordinate.Split(',');
            int length = values.Length;

            for (int count = 0; count < length; count++)
            {
                finalValues[count] = values[count];
            }

            arcminuteString = "0," + finalValues[1];

            Double.TryParse(finalValues[0], out degrees);
            Double.TryParse(arcminuteString, out arcminutes);

            if (isLatitude == true)
            {
                latitudeDegress = degrees;
            }
            else if (isLatitude == false)
            {
                longitudeDegress = degrees;
            }

            arcminutes = arcminutes * 60;

            return arcminutes;
        }

        // Convert from '52 31 14.941' to '50 25,123'
        public double convertDegreesMinutesSecondsStringToDegreesMinutes(string coordinate, bool? isLatitude)
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
                finalValues[2] = "0";
            }
            else if (length == 2)
            {
                finalValues[2] = "0";
            }

            finalValues[2] = finalValues[2].Replace(".", ",");

            Double.TryParse(finalValues[0], out degrees);
            Double.TryParse(finalValues[1], out arcminutes);
            Double.TryParse(finalValues[2], out arcseconds);

            if (isLatitude == true)
            {
                latitudeDegress = degrees;
            }
            else if (isLatitude == false)
            {
                longitudeDegress = degrees;
            }

            arcseconds /= 60;

            arcminutes += arcseconds;

            return arcminutes;
        }

    }
}
