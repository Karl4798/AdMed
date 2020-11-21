using System.ComponentModel.DataAnnotations;

namespace AdMedWeb.Models.ViewModels
{
    public class UpdateUserViewModel
    {
        public int Id { get; set; }
        [Required] public string Username { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        public Enums.Roles RolesEnum { get; set; }
        public string Role { get; set; }
        [Required] public int ResidentId { get; set; }
        public Resident Resident { get; set; }
    }
}