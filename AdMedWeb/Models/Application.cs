using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdMedWeb.Models
{

    public class Application
    {

        public Application()
        {
            TimeStamp = DateTime.Now;
        }

        public int Id { get; set; }
        [DisplayName("First Name")]
        [Required] public string FirstName { get; set; }
        [DisplayName("Last Name")]
        [Required] public string LastName { get; set; }
        [Required] public Enums.Genders Gender { get; set; }
        public string GenderString { get; set; }
        [Required] public string Allergies { get; set; }
        public string DateOfBirthString { get; set; }
        [DisplayName("Date of Birth")]
        [Required(ErrorMessage = "Date of Birth field is required.")] public DateTime DateOfBirth { get; set; }
        [DisplayName("Identity Number")]
        [Required] public string IdentityNumber { get; set; }
        [DisplayName("Medical Aid")]
        [Required] public string MedicalAid { get; set; }
        [DisplayName("Medical Aid Number")]
        [Required] public string MedicalAidNumber { get; set; }
        [DisplayName("Doctor Name")]
        [Required] public string DoctorName { get; set; }
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
        public string TimeStampString { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool Invisible { get; set; }
        public PrimaryContactApplication PrimaryContact { get; set; }
        public Enums.Approvals Approval { get; set; }
        // General information of the primary contact included in PrimaryContact

    }

}