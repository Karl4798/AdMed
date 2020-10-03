using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdMedWeb.Models
{

    public class Medication
    {

        public int Id { get; set; }

        [DisplayName("Medication Name")]
        public string Name { get; set; }
        [DisplayName("Quantity")]
        public int Quantity { get; set; }
        [DisplayName("Time Schedule")]
        public string TimeSchedule { get; set; }
        [DisplayName("Notes")]
        public string Notes { get; set; }
        [Required] public int ResidentId { get; set; }
        public Resident Resident { get; set; }

    }

}
