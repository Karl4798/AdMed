using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AdMedWeb.Models.ViewModels
{
    public class AccountViewModel
    {
        public UpdateUserViewModel User { get; set; }
        public IEnumerable<SelectListItem> ResidentList { get; set; }
    }
}