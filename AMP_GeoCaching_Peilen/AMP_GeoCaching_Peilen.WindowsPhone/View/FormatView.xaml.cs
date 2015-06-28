using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Benutzersteuerelement" ist unter http://go.microsoft.com/fwlink/?LinkId=234236 dokumentiert.

namespace AMP.GeoCachingTools.View
{
    public partial class FormatView : UserControl
    {
        public FormatView()
        {
            InitializeComponent();
        }
    }


    //public class BoolOpposite : System.Windows.Data.IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        bool b = (bool)value;
    //        return !b;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        string s = value as string;
    //        bool b;

    //        if (bool.TryParse(s, out b))
    //        {
    //            return !b;
    //        }
    //        return false;
    //    }
    //}
}
