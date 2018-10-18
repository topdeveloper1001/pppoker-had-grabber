//-----------------------------------------------------------------------
// <copyright file="TypeExtension.cs" company="Ace Poker Solutions">
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
using System.Linq;
using System.Reflection;

namespace PPPokerCardCatcher.Common.Reflection
{
    public static class TypeExtension
    {
        public static bool InterfaceFilter(Type type, object filterCriteria)
        {
            Type filterInterface = filterCriteria as Type;
            return filterInterface == null ? false : filterInterface.IsAssignableFrom(type);
        }

        public static bool TypeIsInterface(this Type type, Type interfaceType)
        {
            if (interfaceType.IsGenericType)
            {
                Type[] genericArguments = interfaceType.GetGenericArguments();

                foreach (var intf in type.GetInterfaces().Where(i => i.IsGenericType))
                {
                    var intfGenericArguments = intf.GetGenericArguments();
                    if (genericArguments
                        .Where((t, i) => intfGenericArguments[i] == t || intfGenericArguments[i].IsSubclassOf(t))
                        .Count() == genericArguments.Length)
                        return true;
                }

                return false;
            }

            TypeFilter typeFilter = InterfaceFilter;

            return type.FindInterfaces(typeFilter, interfaceType).Length > 0;
        }
    }
}