using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace neutrino_gui.ViewModels
{
    public class BoolToDoubleConverter : System.Windows.Data.IValueConverter
    {
        public BoolToDoubleConverter()
        {
            this.TrueValue = 1.0;
            this.FalseValue = 0.0;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return b ? this.TrueValue : this.FalseValue;
            }
            else
            {
                return this.FalseValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        public double TrueValue { get; set; }

        public double FalseValue { get; set; }
    }
}
