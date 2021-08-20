using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookif.Library.Common.Attributes
{ 
    /// <summary>
    /// This is used for reporting when there is an object named "X" but in its database it is called "Y"
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ColumnNameAttribute : System.Attribute
    {
        public string columnName { get; init; }

        public ColumnNameAttribute(string columnName)
        {
            this.columnName = columnName;
        }
    }
}
