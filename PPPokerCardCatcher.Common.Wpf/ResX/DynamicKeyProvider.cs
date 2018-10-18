using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using PPPokerCardCatcher.Common.Resources;

namespace PPPokerCardCatcher.Common.Wpf.ResX
{
    public class DynamicKeyProvider : MarkupExtension, IResXKeyProvider
    {
        public string ProvideKey(IEnumerable<object> parameters)
        {
            if (parameters != null)
            {
                var firstParam = parameters.FirstOrDefault();

                if (firstParam != null && firstParam != DependencyProperty.UnsetValue)
                {
                    return firstParam.ToString();
                }
            }

            return "Key_Unassigned";
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}