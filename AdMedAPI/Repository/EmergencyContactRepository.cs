using System.Collections.Generic;
using System.Linq;
using AdMedAPI.Data;
using AdMedAPI.Models;
using AdMedAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ParkyAPI.Repository
{
    public class EmergencyContactRepository : IEmergencyContactRepository
    {

        private readonly ApplicationDbContext _db;

        public EmergencyContactRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public ICollection<EmergencyContact> GetEmergencyContacts()
        {
            return _db.EmergencyContacts.Include(c => c.Application).OrderBy(a => a.Id).ToList();
        }

        public ICollection<EmergencyContact> GetEmergencyContactsInApplication(int applicationId)
        {
            return _db.EmergencyContacts.Include(c => c.Application).Where(c => c.ApplicationId == applicationId).ToList();
        }

        public EmergencyContact GetEmergencyContact(int emergencyContactId)
        {
            return _db.EmergencyContacts.Include(c => c.Application).FirstOrDefault(a => a.Id == emergencyContactId);
        }

        public bool EmergencyContactExists(string identityNumber)
        {
            bool value = _db.EmergencyContacts.Any(a => a.IdentityNumber.ToLower().Trim() == identityNumber.ToLower());
            return value;
        }

        public bool EmergencyContactExists(int emergencyContactId)
        {
            return _db.EmergencyContacts.Any(a => a.Id == emergencyContactId);
        }

        public bool CreateEmergencyContact(EmergencyContact emergencyContact)
        {
            _db.EmergencyContacts.Add(emergencyContact);
            return Save();
        }

        public bool UpdateEmergencyContact(EmergencyContact emergencyContact)
        {
            _db.EmergencyContacts.Update(emergencyContact);
            return Save();
        }

        public bool DeleteEmergencyContact(EmergencyContact emergencyContact)
        {
            _db.EmergencyContacts.Remove(emergencyContact);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
