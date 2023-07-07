using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.ViewModels.Users;

public class RegisterVM
{
    [Required]
    public string FullName { get; set; } = null!;
    [Required]
    public string UserName { get; set; } = null!;
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = null!;
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
}
