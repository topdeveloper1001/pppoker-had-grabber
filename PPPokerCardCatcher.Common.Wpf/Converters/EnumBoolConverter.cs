//-----------------------------------------------------------------------
// <copyright file="EnumBoolConverter.cs" company="Ace Poker Solutions">
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

namespace PPPokerCardCatcher.Common.Wpf.Converters
{
    /// <summary>
    /// Converts enums to bool 
    /// <RadioButton IsChecked="{Binding ReplaceMode, Mode=TwoWay, Converter={StaticResource EnumBoolConverter}, ConverterParameter={x:Static data:ReplaceMode.Any}}" />
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public class EnumBoolConverter : MarkupExtensionConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return DependencyProperty.UnsetValue;
            }

            if (value.GetType() == parameter.GetType())
            {
                return value.Equals(parameter);
            }

            var strParameter = parameter.ToString().Trim();

            if (string.IsNullOrEmpty(strParameter))
            {
                return DependencyProperty.UnsetValue;
            }

            if (!Enum.IsDefined(value.GetType(), strParameter))
            {
                return false;
            }

            try
            {
                return Enum.Parse(value.GetType(), strParameter).Equals(value);
            }
            catch (ArgumentException)
            {
            }

            return DependencyProperty.UnsetValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null || targetType == null || value == null)
            {
                return DependencyProperty.UnsetValue;
            }

            bool v = System.Convert.ToBoolean(value);

            if (v != true)
            {
                return DependencyProperty.UnsetValue;
            }
            if (targetType == parameter.GetType())
            {
                return parameter;
            }

            var strParameter = parameter.ToString().Trim();

            if (string.IsNullOrEmpty(strParameter))
            {
                return DependencyProperty.UnsetValue;
            }

            try
            {
                var cTargetType = Nullable.GetUnderlyingType(targetType);

                if (cTargetType != null)
                {
                    targetType = cTargetType;
                }

                if (!Enum.IsDefined(targetType, strParameter))
                {
                    return DependencyProperty.UnsetValue; ;
                }

                return Enum.Parse(targetType, strParameter);
            }
            catch (ArgumentException) { }

            return DependencyProperty.UnsetValue;
        }
    }
}