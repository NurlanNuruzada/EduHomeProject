using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.ViewModels.AuthViewModels;

public class RegisterVM
{
    public string? FullName { get; set; } 
    [Required]
    public string UserName { get; set; } = null!; 
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    [Required, DataType(DataType.Password),Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = null!;
    public string? ReturnUrl { get; set; }
    public IList<AuthenticationScheme>? ExternalLogins { get; set; }
}
