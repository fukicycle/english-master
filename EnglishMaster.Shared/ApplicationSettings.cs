using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishMaster.Shared
{
    public static class ApplicationSettings
    {
        public static ApplicationMode Mode = ApplicationMode.Prod;
        public static byte[] JWT_KEY = new byte[64];
    }
}
