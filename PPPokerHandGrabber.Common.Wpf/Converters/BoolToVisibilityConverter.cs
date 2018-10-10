using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PPPokerHandGrabber.Common.Wpf.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : MarkupExtensionConverterBase
    {
        public BoolToVisibilityConverter()
        {
            HiddenVisibility = Visibility.Collapsed;
        }

        public Visibility HiddenVisibility { get; set; }

        public static object Convert(object value, Type targetType, object parameter, Visibility hiddenVisibility)
        {
            bool b = (bool)(value ?? false);
			if (parameter != null)
				b = !b;
            return b ? Visibility.Visible : hiddenVisibility;
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, HiddenVisibility);
        }

        public static object ConvertBack(object value, Type targetType, object parameter)
        {
            Visibility v = (Visibility)value;
            bool b = v == Visibility.Visible;
			return parameter == null ? b : !b;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertBack(value, targetType, parameter);
        }
    }
    
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverterHiddenByDef : BoolToVisibilityConverter
    {
        public BoolToVisibilityConverterHiddenByDef()
        {
            HiddenVisibility = Visibility.Hidden;
        }
    }
}