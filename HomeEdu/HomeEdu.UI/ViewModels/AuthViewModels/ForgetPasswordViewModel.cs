using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.ViewModels.AuthViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
