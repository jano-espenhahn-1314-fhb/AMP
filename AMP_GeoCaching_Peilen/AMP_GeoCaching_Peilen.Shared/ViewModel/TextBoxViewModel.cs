using System;
using System.Windows;
using System.Collections.ObjectModel;
using AMP.GeoCachingTools.Model;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using System.Collections.Generic;
using AMP.GeoCachingTools.Commons;

namespace AMP.GeoCachingTools.ViewModel
{
    public class TextBoxViewModel : BaseINPC
    {

        private GeoCoordinate initialCoordinate { get; set; }

        private string distance { get; set; }

        private string direction { get; set; }

        public List<string> emptyFields { get; set; }

        public TextBoxViewModel(GeoCoordinate initialCoordinate, string distance, string direction)
        {
            this.initialCoordinate = initialCoordinate;
            this.distance = distance;
            this.direction = direction;
        }

        // Simple check, if fields are empty
        public void checkEmptyTextboxes()
        {
            emptyFields = new List<string>();

            if (distance == null)
            {
                emptyFields.Add("Entfernung");
            }

            if (direction == null)
            {
                emptyFields.Add("Richtung");
            }

            if (initialCoordinate.LongitudeCoordinate == null)
            {
                emptyFields.Add("Breitengrad");
            }

            if (initialCoordinate.LatitudeCoordinate == null)
            {
                emptyFields.Add("Längengrad");
            }

        }

    }
}