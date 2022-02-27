using System;
using System.Collections.Generic;
using System.Linq;

namespace Lookif.Library.Common.Utilities
{
    public static class TypeExtensions
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="type"></param>
        /// <param name="direct"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetDirectInterfaces(this Type type, bool direct)
        {
            if (direct)
                return type.GetInterfaces().Except(type.BaseType?.GetInterfaces());
            else
                return type.GetInterfaces();

        }
    }
}
