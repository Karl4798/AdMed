using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdMedWeb
{
    public static class SD
    {

        public static string APIBaseUrl = "https://admedapi.azurewebsites.net/";
        //public static string APIBaseUrl = "https://localhost:44399/";
        public static string ApplicationAPIPath = APIBaseUrl + "api/v1/applications/";
        public static string ResidentAPIPath = APIBaseUrl + "api/v1/residents/";
        public static string MedicationAPIPath = APIBaseUrl + "api/v1/medications/";
        public static string AccountAPIPath = APIBaseUrl + "api/v1/users/";

    }
}