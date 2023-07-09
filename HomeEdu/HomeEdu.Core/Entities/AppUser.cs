using Microsoft.AspNetCore.Identity;

namespace HomeEdu.Core.Entities;

public class AppUser : IdentityUser
{
    public string? FullName { get; set; }
}
