using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Develops.Utils
{
    static internal class StringSanitizationExtensions
    {
        public static string Sanitize(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            return input
                .Trim()
                .Replace("'", "''")
                .Replace(";", "")
                .Replace("--", "")
                .Replace("/*", "")
                .Replace("*/", "");
        }
    }
}
