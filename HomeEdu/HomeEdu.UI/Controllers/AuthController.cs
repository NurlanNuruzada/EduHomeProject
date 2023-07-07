using HomeEdu.Core.Entities;
using HomeEdu.UI.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HomeEdu.UI.Controllers;

public class AuthController : Controller
{
    private readonly UserManager<AppUser> _userManager;

    public AuthController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task< IActionResult> Register(RegisterVM newUser)
    {
        if (!ModelState.IsValid) return View(newUser);
        AppUser user = new() { 
            FullName = newUser.UserName,
            Email = newUser.Email,
            UserName  = newUser.UserName,
        };
         IdentityResult result =  await _userManager.AddPasswordAsync(user, newUser.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("",error.Description);
                return View(newUser);
            }
        }
        return Ok();
    }
}
