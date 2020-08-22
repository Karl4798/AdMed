using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdMedWeb.Models
{
    public class Application
    {

        public int Id { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        public enum Genders { Female, Male, Other }
        [Required] public Genders Gender { get; set; }
        public string GenderString { get; set; }
        [Required] public string Allergies { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
        [Required] public string IdentityNumber { get; set; }
        [Required] public string MedicalAid { get; set; }
        public string MedicalAidNumber { get; set; }
        public string DoctorName { get; set; }
        public string HomeTelephoneNumber { get; set; }
        public string WorkTelephoneNumber { get; set; }
        [Required] public string CellTelephoneNumber { get; set; }
        [Required] public string Undertaker { get; set; }
        [Required] public string UndertakerTelephoneNumber { get; set; }
        [Required] public string PharmacyName { get; set; }
        [Required] public string PharmacyTelephoneNumber { get; set; }
        [Required] public string PharmacyFaxNumber { get; set; }

    }
}
