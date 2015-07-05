using System;
using System.Windows;
using System.Collections.ObjectModel;
using AMP.GeoCachingTools.Model;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using System.Collections.Generic;
using AMP.GeoCachingTools.Commons;

namespace AMP.GeoCachingTools.Commons
{
    public class Validator : BaseINPC
    {

        private GeoCoordinate initialCoordinate { get; set; }

        private string distance { get; set; }

        private string direction { get; set; }

        private List<string> values = new List<string>();

        public List<string> emptyFields { get; set; }

        public List<double> validatedValues = new List<double>();

        public Validator(GeoCoordinate initialCoordinate, string distance, string direction)
        {
            this.initialCoordinate = initialCoordinate;
            this.distance = distance;
            this.direction = direction;

            values.Add(initialCoordinate.LongitudeCoordinate);
            values.Add(initialCoordinate.LatitudeCoordinate);
            values.Add(distance);
            values.Add(direction);
        }
        
        public void checkTextboxes()
        {
            if (!checkEmptyTextboxes())
            {
                foreach (string value in values)
                {
                    validateDoubleValue(value);
                }
            }
        }

        // Check and parse values
        private void validateDoubleValue(string value)
        {
            double doubleValue;

            if (Double.TryParse(value, out doubleValue))
            {
                validatedValues.Add(doubleValue);
            }
            else
            {
                validatedValues.Add(-1);
            }
        }

        // Simple check, if fields are empty
        private bool checkEmptyTextboxes()
        {
            bool isEmpty = false;

            emptyFields = new List<string>();

            if (distance == null)
            {
                emptyFields.Add("Entfernung");
                isEmpty = true;
            }

            if (direction == null)
            {
                emptyFields.Add("Richtung");
                isEmpty = true;
            }

            if (initialCoordinate.LatitudeCoordinate == null)
            {
                emptyFields.Add("Breitengrad");
                isEmpty = true;
            }

            if (initialCoordinate.LongitudeCoordinate == null)
            {
                emptyFields.Add("Längengrad");
                isEmpty = true;
            }

            return isEmpty;
        }

        public bool isInDirectionRange(double value)
        {
            bool isInRange = false;

            System.Diagnostics.Debug.WriteLine("Wert ist : " + value.ToString());

            if (value >= 0 && value <= 360)
            {
                isInRange = true;
            }

            System.Diagnostics.Debug.WriteLine("Wert ist : " + isInRange.ToString());

            return isInRange;
        }
    }
}
