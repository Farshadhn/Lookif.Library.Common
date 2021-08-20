using Lookif.Library.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookif.Library.Common.CommonModels
{
    public record BriefPropertyInfo
    {
        public BriefPropertyInfo(string propertyName, string propertyDisplayName, TypeOfProperty typeOfProperty, string propertyTypeName)
        {
            PropertyName = propertyName;
            PropertyDisplayName = propertyDisplayName;
            TypeOfProperty = typeOfProperty;
            PropertyTypeName = propertyTypeName;
        }

        public string PropertyName { get; init; }
        public string PropertyDisplayName { get; init; }
        public TypeOfProperty TypeOfProperty { get; init; }
        /// <summary>
        ///  When TypeOfProperty is Class, we need to set PropertyTypeName.
        ///  For other cases, we do have no value.
        /// </summary>
        public string PropertyTypeName { get; init; }

    }
}
