using System.ComponentModel.DataAnnotations;

namespace AdMedWeb.Models
{
    public class EmergencyContact
    {

        public int Id { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Relationship { get; set; }
        [Required] public string PhysicalAddress { get; set; }
        [Required] public string PostalAddress { get; set; }
        [Required] public string PostCode { get; set; }
        [Required] public string IdentityNumber { get; set; }
        public string HomeTelephoneNumber { get; set; }
        public string WorkTelephoneNumber { get; set; }
        [Required] public string CellTelephoneNumber { get; set; }
        [Required] public string Email { get; set; }
        [Required] public int ApplicationId { get; set; }
        public Application Application { get; set; }

    }
}
