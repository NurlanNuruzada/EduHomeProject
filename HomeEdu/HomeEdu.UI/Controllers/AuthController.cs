using HomeEdu.Core.Entities;
using HomeEdu.UI.ViewModels.AuthViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeEdu.UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _singInManager;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> singInManager)
        {
            _userManager = userManager;
            _singInManager = singInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM newUser)
        {
            if (!ModelState.IsValid) return View(newUser);
            AppUser user = new()
            {
                UserName = newUser.UserName,
                Email = newUser.Email,
                FullName = string.Concat(newUser.UserName, " ", newUser.LastName),
            };
            IdentityResult result = await _userManager.CreateAsync(user, newUser.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(newUser);
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            return View();

        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM User)
        {
            if (!ModelState.IsValid) return View(User);
             AppUser UserName=await _userManager.FindByNameAsync(User.UserName);
            if(User is null)
            {
                ModelState.AddModelError("", "Invalid Login!");
            }
            var signInResult= await _singInManager.PasswordSignInAsync(UserName, User.Password,User.RememberMe,true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Your accound Is Locked Try Later!");
                return View(User);
            } 
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Invalid Login!");
                return View(User);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _singInManager.SignOutAsync();
            }
                return RedirectToAction("Index", "Home");
        }
    }
}
