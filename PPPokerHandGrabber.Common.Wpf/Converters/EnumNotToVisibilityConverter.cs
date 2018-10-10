//-----------------------------------------------------------------------
// <copyright file="EnumNotToVisibilityConverter.cs" company="Ace Poker Solutions">
// Copyright © 2015 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PPPokerHandGrabber.Common.Wpf.Converters
{
    /// <summary>
    /// EnumToVisibilityConverter
    /// </summary>
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class EnumNotToVisibilityConverter : MarkupExtensionConverterBase
    {
        public EnumNotToVisibilityConverter()
        {
            HiddenVisibility = Visibility.Hidden;
        }

        public Visibility HiddenVisibility { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return HiddenVisibility;
            }

            if (value.GetType() == parameter.GetType())
            {
                return value.Equals(parameter) ? HiddenVisibility : Visibility.Visible;
            }

            var strParameter = parameter.ToString().Trim();

            if (string.IsNullOrEmpty(strParameter))
            {
                return HiddenVisibility;
            }

            if (!Enum.IsDefined(value.GetType(), strParameter))
            {
                return HiddenVisibility;
            }

            try
            {
                return Enum.Parse(value.GetType(), strParameter).Equals(value) ? HiddenVisibility : Visibility.Visible;
            }
            catch
            {
            }

            return HiddenVisibility;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    [ValueConversion(typeof(object), typeof(Visibility))]
    public class EnumNotToVisibilityConverterCollapsedByDef : EnumNotToVisibilityConverter
    {
        public EnumNotToVisibilityConverterCollapsedByDef()
            : base()
        {
            HiddenVisibility = Visibility.Collapsed;
        }
    }
}