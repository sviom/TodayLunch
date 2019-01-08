using System;
using System.Collections.Generic;
using System.Text;

namespace LunchCommon
{
    public static class PreDefined
    {
#if DEBUG
        public static string ServiceApiUrl => "http://localhost:7011/api/";
#else
        public static string ServiceApiUrl => "http://todaylunchapi.azurewebsites.net/api/";
#endif
    }
}
