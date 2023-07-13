using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.ViewModels.AuthViewModels
{
    public class ResetAccPasswordViewModel
    {
        [Required]
        public string userId { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
