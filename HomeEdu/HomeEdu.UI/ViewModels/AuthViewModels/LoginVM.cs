using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.ViewModels.AuthViewModels;

public class LoginVM
{
    [Required]
    public string UserName { get; set; } = null!;

    [Required , DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    public bool RememberMe { get; set; }
}
