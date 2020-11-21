using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdMedWeb.Models.ViewModels
{
    public class AdminResetPasswordViewModel
    {

        [Required] public string Username { get; set; }
        [Required] [DisplayName("New Password")] public string Password { get; set; }
        [Required] [DisplayName("Confirm Password")] public string ConfirmPassword { get; set; }

    }
}