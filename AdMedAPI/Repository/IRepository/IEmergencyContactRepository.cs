using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdMedAPI.Models;

namespace AdMedAPI.Repository.IRepository
{
    public interface IEmergencyContactRepository
    {

        ICollection<EmergencyContact> GetEmergencyContacts();
        ICollection<EmergencyContact> GetEmergencyContactsInApplication(int applicationId);
        EmergencyContact GetEmergencyContact(int emergencyContactId);
        bool EmergencyContactExists(String identificationNumber);
        bool EmergencyContactExists(int emergencyContactId);
        bool CreateEmergencyContact(EmergencyContact emergencyContact);
        bool UpdateEmergencyContact(EmergencyContact emergencyContact);
        bool DeleteEmergencyContact(EmergencyContact emergencyContact);
        bool Save();

    }
}
