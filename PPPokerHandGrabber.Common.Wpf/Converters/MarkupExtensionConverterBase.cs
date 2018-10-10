using System;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Windows.Data;

namespace PPPokerHandGrabber.Common.Wpf.Converters
{
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public abstract class MarkupExtensionConverterBase : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public abstract object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture);
    }

    [MarkupExtensionReturnType(typeof(IMultiValueConverter))]
    public abstract class MarkupExtensionMultiConverterBase : MarkupExtension, IMultiValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public abstract object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture);

        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture);
    }
}
