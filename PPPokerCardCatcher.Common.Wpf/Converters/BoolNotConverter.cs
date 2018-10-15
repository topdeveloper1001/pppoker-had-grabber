using System;
using System.Windows.Data;

namespace PPPokerCardCatcher.Common.Wpf.Converters
{
    [ValueConversion(typeof(bool?), typeof(bool?))]
    public class BoolNotConverter : MarkupExtensionConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool? b = value as bool?;
            return b != true;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool? b = value as bool?;
            return b != true;
        }
    }
}