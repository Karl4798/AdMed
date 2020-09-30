using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdMedWeb.Models
{
    public class Application
    {

        public int Id { get; set; }
        [DisplayName("First Name")]
        [Required] public string FirstName { get; set; }
        [DisplayName("Last Name")]
        [Required] public string LastName { get; set; }
        public enum Genders { Female, Male, Other }
        [Required] public Genders Gender { get; set; }
        public string GenderString { get; set; }
        [Required] public string Allergies { get; set; }
        [DisplayName("Date of Birth")]
        [Required] public DateTime DateOfBirth { get; set; }
        [DisplayName("Identity Number")]
        [Required] public string IdentityNumber { get; set; }
        [DisplayName("Medical Aid")]
        [Required] public string MedicalAid { get; set; }
        [DisplayName("Medical Aid Number")]
        public string MedicalAidNumber { get; set; }
        [DisplayName("Doctor Name")]
        public string DoctorName { get; set; }
        [DisplayName("Home Telephone Number")]
        public string HomeTelephoneNumber { get; set; }
        [DisplayName("Work Telephone Number")]
        public string WorkTelephoneNumber { get; set; }
        [DisplayName("Cellphone Number")]
        [Required] public string CellTelephoneNumber { get; set; }
        [Required] public string Undertaker { get; set; }
        [DisplayName("Undertaker Telephone Number")]
        [Required] public string UndertakerTelephoneNumber { get; set; }
        [DisplayName("Pharmacy Name")]
        [Required] public string PharmacyName { get; set; }
        [DisplayName("Pharmacy Telephone Number")]
        [Required] public string PharmacyTelephoneNumber { get; set; }
        [DisplayName("Pharmacy Fax Number")]
        [Required] public string PharmacyFaxNumber { get; set; }
        public PrimaryContact PrimaryContact { get; set; }

        // General information of the primary contact included in PrimaryContact

    }
}
