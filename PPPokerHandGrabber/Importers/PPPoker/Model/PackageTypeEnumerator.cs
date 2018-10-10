using System;
using System.Reflection;

namespace PPPokerHandGrabber.Importers.PPPoker.Model
{
    class PackageTypeEnumerator
    {
        public static void ForEach(Action<PackageType, Type> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            foreach (var enumValue in Enum.GetValues(typeof(PackageType)))
            {
                Type packageObjectType = Assembly.GetExecutingAssembly().GetType($"{typeof(PackageTypeEnumerator).Namespace}.{enumValue}");

                if (packageObjectType != null)
                {
                    action((PackageType)enumValue, packageObjectType);
                }
            }
        }
    }
}
