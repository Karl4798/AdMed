using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdMedWeb
{
    public static class SD
    {

        public static string APIBaseUrl = "https://admedapi.azurewebsites.net/";
        public static string ApplicationAPIPath = APIBaseUrl + "api/v1/applications/";
        public static string EmergencyContactAPIPath = APIBaseUrl + "api/v1/emergencycontacts/";
        public static string AccountAPIPath = APIBaseUrl + "api/v1/Users/";

    }
}
