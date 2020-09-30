using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdMedAPI.Models
{
    public class Medication
    {

        [Key]
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public int Quantity { get; set; }
        [Required] public string Notes { get; set; }

    }
}