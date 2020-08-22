using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdMedWeb.Models.ViewModels
{
    public class IndexViewModel
    {

        public IEnumerable<Application> NationalParkList { get; set; }
        public IEnumerable<Application> TrailList { get; set; }

    }
}
