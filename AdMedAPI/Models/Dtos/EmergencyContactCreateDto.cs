using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdMedAPI.Models.Dtos
{
    public class EmergencyContactCreateDto
    {

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

    }
}
