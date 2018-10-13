//-----------------------------------------------------------------------
// <copyright file="EnumToLocalizedStringConverter.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerCardCatcher.Common.Resources;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PPPokerCardCatcher.Common.Wpf.Converters
{
    [ValueConversion(typeof(object), typeof(string))]
    public class EnumToLocalizedStringConverter : MarkupExtensionConverterBase
    {
        #region IValueConverter Members

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Enum))
            {
                return string.Empty;
            }

            if (parameter == null)
            {
                return CommonResourceManager.Instance.GetEnumResource((Enum)value) ?? string.Empty;
            }

            var key = CompositeKeyProvider.GetKey((string)parameter, new[] { value }) ?? string.Empty;

            return CommonResourceManager.Instance.GetResourceString(key);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        #endregion
    }
}