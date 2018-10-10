using System;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Reflection;
using System.Windows;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;
using PPPokerHandGrabber.Common.Resources;
using PPPokerHandGrabber.Common.Linq;

namespace PPPokerHandGrabber.Common.Wpf.ResX
{
    [MarkupExtensionReturnType(typeof(object))]
    [ContentProperty("Parameters")]
    public class ResXExtension : ManagedMarkupExtension, IServiceProvider
    {
        private readonly ResXParamList parameters = new ResXParamList();

        public ResXParamList Parameters
        {
            get
            {
                return parameters;
            }
            set
            {
                parameters.Clear();
                foreach (ResXParamBase item in value.OfType<ResXParamBase>())
                {
                    parameters.Add(item);
                }
            }
        }

        private WeakReference innerElement;
        private object innerProperty;

        public ResXExtension()
        {
        }

        public ResXExtension(string key) : this()
        {
            Key = key;
        }

        public ResXExtension(string key, string defaultValue)
            : this(key)
        {
            if (!string.IsNullOrEmpty(defaultValue))
            {
                DefaultValue = defaultValue;
            }
        }

        private IResXKeyProvider keyProvider;
        public IResXKeyProvider KeyProvider
        {
            get { return keyProvider ?? new CompositeKeyProvider(key); }
            set { keyProvider = value; }
        }

        private string key;
        public string Key
        {
            get { return KeyProvider.ProvideKey(null); }
            set { key = value; }
        }

        public string DefaultValue { get; set; }

        public object GetDefaultValue(string key)
        {
            object result = DefaultValue;

            if (TargetProperty == null)
            {
                return result;
            }

            Type targetType = TargetPropertyType;

            if (DefaultValue == null)
            {
                if (targetType == typeof(String) || targetType == typeof(object))
                {
                    result = String.Format("#{0}", key);
                }
            }
            else
            {
                if (targetType != typeof(String) && targetType != typeof(object))
                {
                    try
                    {
                        TypeConverter tc = TypeDescriptor.GetConverter(targetType);
                        result = tc.ConvertFromInvariantString(DefaultValue);
                    }
                    catch
                    {
                    }
                }
            }

            return result;
        }

        private void EnsureAppResourcesLoaded()
        {
            Assembly resourceDescriptionAssembly = Assembly.GetExecutingAssembly();
            Stream resourcesReferencesStream = resourceDescriptionAssembly.GetManifestResourceStream("DriveHUD.Common.Wpf.ResX.resources.xml");

            if (resourcesReferencesStream == null)
                return;

            XElement rootItem;
            using (TextReader streamReader = new StreamReader(resourcesReferencesStream))
            {
                XDocument doc = XDocument.Load(streamReader);
                rootItem = doc.Nodes().OfType<XElement>().FirstOrDefault();
            }

            if (rootItem == null) return;

            foreach (XElement resourceElement in rootItem.Elements())
            {
                try
                {
                    string asmName = resourceElement.Attribute("AssemblyName").Value;
                    string typeName = resourceElement.Attribute("ResourceRegistratorName").Value;
                    string methodName = resourceElement.Attribute("ResourceRegistratorMethodName").Value;

                    Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.Contains(asmName));

                    if (assembly != null)
                    {
                        Type resourceRegistrator = assembly.GetType(typeName);
                        MethodInfo method = resourceRegistrator.GetMethod(methodName);
                        method.Invoke(null, null);
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(string.Format("error = {0}", ex.Message));
                }
            }
        }

        private ResXKeyPart keyPart1;

        public ResXKeyPart KeyPart1
        {
            get { return keyPart1; }
            set
            {
                if (keyPart1 == value) return;
                if (value != null && Parameters.Count > 0 && Parameters[0] == value)
                    Parameters.RemoveAt(0);
                keyPart1 = value;
                Parameters.Insert(0, keyPart1);
            }
        }

        protected override object GetValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Key))
                throw new ArgumentException("Key cannot be null");

            IProvideValueTarget targetHelper = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            if (!(targetHelper.TargetProperty is DependencyProperty))
            {
                return this;
            }

            DependencyObject targetObject = targetHelper.TargetObject as DependencyObject;

            if (targetObject != null && DesignerProperties.GetIsInDesignMode(targetObject))
            {
                EnsureAppResourcesLoaded();
            }

            ResXParamList paramList = null;

            if (targetObject != null && Parameters.Count == 0)
            {
                paramList = GetResXContext(targetObject);
            }

            if (paramList == null)
            {
                paramList = Parameters;
            }

            var converter = new ResXConverter { KeyProvider = KeyProvider, ResXExtension = this, Parameters = paramList };

            Binding binding = new Binding("UICulture")
            {
                Source = CultureManager.Instance,
                Mode = BindingMode.OneWay
            };

            if (paramList.Count == 0)
            {
                binding.Converter = converter;
                return binding.ProvideValue(serviceProvider);
            }

            var multiBinding = new MultiBinding { Mode = BindingMode.OneWay, Converter = converter };

            foreach (var param in paramList)
            {
                multiBinding.Bindings.Add(param);
            }

            multiBinding.Bindings.Add(binding);

            return multiBinding.ProvideValue(serviceProvider);
        }

        public static void BindExtension(DependencyObject element, DependencyProperty property, string key)
        {
            BindExtension(element, property, key, null);
        }

        public static void BindExtension(DependencyObject element, DependencyProperty property, string key, IEnumerable<ResXParamBase> resXParams)
        {
            if (element == null || property == null || key == null)
            {
                return;
            }

            var resX = new ResXExtension(key, string.Empty)
            {
                innerElement = new WeakReference(element),
                innerProperty = property
            };

            if (resXParams != null)
            {
                resX.Parameters.AddRange(resXParams);
            }

            element.SetValue(property, resX.ProvideValue(resX));
        }

        public static string GetFormattedStringValue(object source, string key, IEnumerable<string> formatPropertyNames)
        {
            var valueFreezable = new ValueFreezable();

            BindExtension(valueFreezable, ValueFreezable.ValueProperty, key,
                formatPropertyNames.Select(s => new ResXParam(s) { Source = source, Mode = BindingMode.OneWay }));

            return valueFreezable.Value as string;
        }

        public object GetService(Type serviceType)
        {
            return serviceType == typeof(IProvideValueTarget) && innerElement.Target != null && innerElement.IsAlive
                ? new ProvideValueTarget(innerElement.Target, innerProperty)
                : null;
        }

        public class ProvideValueTarget : IProvideValueTarget
        {
            public ProvideValueTarget(object element, object property)
            {
                TargetObject = element;
                TargetProperty = property;
            }

            public object TargetObject { get; private set; }
            public object TargetProperty { get; private set; }
        }

        public static readonly DependencyProperty ResXContextProperty =
            DependencyProperty.RegisterAttached(
                "ResXContext",
                typeof(ResXParamList),
                typeof(ResXExtension),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static ResXParamList GetResXContext(DependencyObject obj)
        {
            return (ResXParamList)obj.GetValue(ResXContextProperty);
        }

        public static void SetResXContext(DependencyObject obj, ResXParamList value)
        {
            obj.SetValue(ResXContextProperty, value);
        }

    }

    public class ResXParamList : BindingList<BindingBase>
    {
    }

    public class ValueFreezable : Freezable
    {
        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(ValueFreezable), new UIPropertyMetadata(null));

        protected override Freezable CreateInstanceCore()
        {
            return (Freezable)Activator.CreateInstance(GetType());
        }
    }
}