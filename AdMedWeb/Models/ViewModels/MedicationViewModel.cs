using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AdMedWeb.Models.ViewModels
{
    public class MedicationViewModel
    {

        public IEnumerable<SelectListItem> ResidentList { get; set; }
        public Medication Medication { get; set; }

    }
}