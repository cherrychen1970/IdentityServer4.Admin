using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Skoruba.Helpers
{
    public static class PropHelper
    {
        public static PropertyInfo GetProp(this Type itemType, string key)
        {
            return itemType.GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        }
    }
}