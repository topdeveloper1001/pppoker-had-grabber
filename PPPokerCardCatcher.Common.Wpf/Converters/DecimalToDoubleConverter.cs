using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PPPokerCardCatcher.Common.Wpf.Converters
{
    [ValueConversion(typeof(decimal?), typeof(double?))]
    public class DecimalToDoubleConverter : MarkupExtensionConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal? b = value as decimal?;
            return (double?)b;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double? b = value as double?;
            return (decimal?)b;
        }
    }
}