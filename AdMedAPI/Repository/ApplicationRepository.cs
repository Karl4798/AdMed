using System.Collections.Generic;
using System.Linq;
using AdMedAPI.Data;
using AdMedAPI.Models;
using AdMedAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace AdMedAPI.Repository
{
    public class ApplicationRepository : IApplicationRepository
    {

        private readonly ApplicationDbContext _db;

        public ApplicationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public ICollection<Application> GetApplications()
        {
            return _db.Applications.Include(a => a.PrimaryContact).OrderBy(a => a.Id).ToList();
        }

        public Application GetApplication(int applicationId)
        {
            return _db.Applications.Include(a => a.PrimaryContact).FirstOrDefault(a => a.Id == applicationId);
        }

        public bool ApplicationExists(string identityNumber)
        {
            bool value = _db.Applications.Any(a => a.IdentityNumber.ToLower().Trim() == identityNumber.ToLower());
            return value;
        }

        public bool ApplicationExists(int applicationId)
        {
            return _db.Applications.Any(a => a.Id == applicationId);
        }

        public bool CreateApplication(Application application)
        {
            application.GenderString = application.Gender.ToString();
            _db.Applications.Add(application);
            return Save();
        }

        public bool UpdateApplication(Application application)
        {
            application.GenderString = application.Gender.ToString();
            _db.Applications.Update(application);
            return Save();
        }

        public bool DeleteApplication(Application application)
        {
            _db.Applications.Remove(application);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
