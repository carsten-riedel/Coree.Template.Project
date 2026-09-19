using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace __SourceName__.Extensions
{
    public static class ExtensionsString
    {
        public static string EmptyIfNull(this string? value)
        {
            if (value == null)
                return String.Empty;
            return value;
        }
    }
}
