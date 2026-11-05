
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Web.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username Required!")]
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}