using Microsoft.AspNetCore.Identity;

namespace HomeEdu.Core.Entities;

public class AppUser : IdentityUser
{
    public string? ResetPasswordToken { get; set; }
    public string? FullName { get; set; }
}
