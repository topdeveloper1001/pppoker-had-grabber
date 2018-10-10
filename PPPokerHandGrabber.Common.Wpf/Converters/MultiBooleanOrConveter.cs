using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPPokerHandGrabber.Common.Wpf.Converters
{
    public class MultiBooleanOrConverter : MarkupExtensionMultiConverterBase
    {
        #region Implementation of IMultiValueConverter

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = values.Any(c => c is bool && (bool)c);

            if (parameter != null)            
                result = !result;
            
            return result;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}