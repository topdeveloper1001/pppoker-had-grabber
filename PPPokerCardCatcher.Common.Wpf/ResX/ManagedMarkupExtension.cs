using System;
using System.Windows.Markup;
using System.Reflection;
using System.Windows;

namespace PPPokerCardCatcher.Common.Wpf.ResX
{
    public abstract class ManagedMarkupExtension : MarkupExtension
    {
        private object targetProperty;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var targetHelper = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            if (targetHelper == null || targetHelper.TargetObject == null)
            {
                return null;
            }

            targetProperty = targetHelper.TargetProperty;

            if (targetHelper.TargetObject is DependencyObject || !(targetProperty is DependencyProperty))
            {
                return GetValue(serviceProvider);
            }

            return this;
        }

        protected object TargetProperty
        {
            get { return targetProperty as DependencyProperty; }
        }

        protected Type TargetPropertyType
        {
            get
            {
                Type propertyType = null;

                if (targetProperty is DependencyProperty)
                {
                    propertyType = (targetProperty as DependencyProperty).PropertyType;
                }
                else if (targetProperty is PropertyInfo)
                {
                    propertyType = (targetProperty as PropertyInfo).PropertyType;
                }

                return propertyType;
            }
        }

        protected abstract object GetValue(IServiceProvider serviceProvider);
    }
}