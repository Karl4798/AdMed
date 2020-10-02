using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdMedWeb.Models
{
    public class PrimaryContactApplication
    {
        public int Id { get; set; }
        [DisplayName("First Name")]
        [Required] public string FirstName { get; set; }
        [DisplayName("Last Name")]
        [Required] public string LastName { get; set; }
        [Required] public string Relationship { get; set; }
        [DisplayName("Physical Address")]
        [Required] public string PhysicalAddress { get; set; }
        [DisplayName("Postal Address")]
        [Required] public string PostalAddress { get; set; }
        [DisplayName("Post Code")]
        [Required] public string PostCode { get; set; }
        [DisplayName("Identity Number")]
        [Required] public string IdentityNumber { get; set; }
        [DisplayName("Home Telephone Number")]
        public string HomeTelephoneNumber { get; set; }
        [DisplayName("Work Telephone Number")]
        public string WorkTelephoneNumber { get; set; }
        [DisplayName("Cellphone Number")]
        [Required] public string CellTelephoneNumber { get; set; }
        [DisplayName("Email Address")]
        [Required] public string Email { get; set; }

    }
}