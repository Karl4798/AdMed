using System.ComponentModel.DataAnnotations;

namespace AdMedWeb.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
        [Required] [Compare("Password")] public string ConfirmPassword { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}