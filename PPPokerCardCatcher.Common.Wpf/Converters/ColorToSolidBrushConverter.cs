using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PPPokerCardCatcher.Common.Wpf.Converters
{
    [ValueConversion(typeof(Color), typeof(SolidColorBrush))]
    public class ColorToSolidBrushConverter : MarkupExtensionConverterBase
    {
        public ColorToSolidBrushConverter()
        {
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = value as Color?;

            if (color == null)
            {
                return DependencyProperty.UnsetValue;
            }
        
            var brush = new SolidColorBrush(color.Value);

            return brush;
        }


        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = value as SolidColorBrush;

            if (brush == null)
            {
                return DependencyProperty.UnsetValue;
            }

            return brush.Color;
        }
    }
}