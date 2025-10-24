using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaPlatformm.Infrastructure
{
    // C# Gereksinimi: Statik Yardımcı Sınıf
    public static class Guard
    {
        public static void IsNull(object obj, string paramName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName, $"{paramName} boş olamaz.");
            }
        }

        public static void IsNullOrWhitespace(string str, string paramName)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException($"{paramName} boş olamaz.", paramName);
            }
        }
    }
}
