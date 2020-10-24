using System.ComponentModel.DataAnnotations;

namespace AdMedWeb.Models.ViewModels
{
    public class UpdateUserViewModel
    {

        public int Id { get; set; }
        [Required] public string Username { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Role { get; set; }

    }
}
