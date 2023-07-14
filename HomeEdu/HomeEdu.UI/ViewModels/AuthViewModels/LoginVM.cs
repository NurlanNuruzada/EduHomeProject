using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.ViewModels.AuthViewModels;

public class LoginVM
{
    [Required]
    public string? LoginIdentifier { get; set; }
    [Required , DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }
    public IList<AuthenticationScheme>? ExternalLogins { get; set; }
}
