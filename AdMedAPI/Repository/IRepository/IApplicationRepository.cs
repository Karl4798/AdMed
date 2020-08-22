using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdMedAPI.Models;

namespace AdMedAPI.Repository.IRepository
{
    public interface IApplicationRepository
    {

        ICollection<Application> GetApplications();
        Application GetApplication(int applicationId);
        bool ApplicationExists(String id);
        bool ApplicationExists(int applicationId);
        bool CreateApplication(Application application);
        bool UpdateApplication(Application application);
        bool DeleteApplication(Application application);
        bool Save();

    }
}
