using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.Helpers.Utilities;
using HomeEdu.UI.Services.EmailService;
using HomeEdu.UI.ViewModels.AuthViewModels;
using HomeEdu.UI.ViewModels.EmailViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NuGet.Protocol;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;

namespace HomeEdu.UI.Controllers
{
    public class AuthController
        : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _singInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IEmailService _mailService;
        private readonly TimeSpan _tokenLifespan;

        public AuthController(UserManager<AppUser> userManager,
                              SignInManager<AppUser> singInManager,
                              RoleManager<IdentityRole> roleManager,
                              AppDbContext context,
                              IEmailService mailService,
                              IOptions<DataProtectionTokenProviderOptions> tokenOptions)
        {
            _userManager = userManager;
            _singInManager = singInManager;
            _roleManager = roleManager;
            _context = context;
            _mailService = mailService;
            _tokenLifespan = tokenOptions.Value.TokenLifespan;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM newUser)
        {
            if (!ModelState.IsValid)
            {
                return View(newUser);
            }
            AppUser user = new AppUser
            {
                UserName = newUser.UserName,
                Email = newUser.Email,
                FullName = string.Concat(newUser.Name, " ", newUser.LastName),
                EmailConfirmed = false
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

            await _userManager.AddToRoleAsync(user, AppUserRole.Admin);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationUrl = $"<a href=\"{Url.Action(nameof(ConfirmEmail), "Auth", new { userId = user.Id , token}, Request.Scheme)}\">Confirm your password</a>";

            var time = DateTime.Now.ToString();
            var userIp = GetUserIP().ToString();
            var Username = user.UserName.ToString();
            var Fullname = user.FullName.ToString();

            string pathToHtmlFile = "Views\\Auth\\EmailConfirmationLetter.cshtml";
            string htmlContent = System.IO.File.ReadAllText(pathToHtmlFile);
            string tokenLifetimeString = FormatTokenLifetime(_tokenLifespan);

            htmlContent = htmlContent.Replace("{{action_url}}", confirmationUrl);
            htmlContent = htmlContent.Replace("{{Time}}", time);
            htmlContent = htmlContent.Replace("{{IpAddress}}", userIp);
            htmlContent = htmlContent.Replace("{{Username}}", Username);
            htmlContent = htmlContent.Replace("{{Fullname}}", Fullname);
            htmlContent = htmlContent.Replace("{{TokenLifetime}}", tokenLifetimeString);

            var message = new EmailVM(user.Email, htmlContent, "Confirm your email");////////////////////
            _mailService.SendEmail(message);
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                // Invalid user ID or token
                return RedirectToAction("Error");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // User not found
                return RedirectToAction("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                // Failed to confirm email
                return RedirectToAction("Error");
            }
            return View();
        }
        public IActionResult EmailConfirmation()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GoogleSignIn()
        {
            var redirectUrl = Url.Action("GoogleSignInCallback", "Auth", null, Request.Scheme);
            var properties = _singInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleSignInCallback()
        {
            var info = await _singInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("Login");
            }
            string password = GenerateRandomPassword();
            var user = await _userManager.FindByEmailAsync(info.Principal.FindFirstValue(ClaimTypes.Email));

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                    FullName = info.Principal.FindFirstValue(ClaimTypes.Name),
                    EmailConfirmed = true // Assuming Google already confirmed the email
                };

                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    // Handle the case when user creation fails
                    // You may want to redirect to an error page or display an error message
                    return RedirectToAction("Register");
                }

                // Assign a default role to the new user
                await _userManager.AddToRoleAsync(user, AppUserRole.Member);
            }
            var FindUser = await _userManager.FindByEmailAsync(info.Principal.FindFirstValue(ClaimTypes.Email));
            // Sign in the user
            var signInResult = await _singInManager.PasswordSignInAsync(FindUser, password, false, true);
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Invalid login!");
                return RedirectToAction("Login");
            }

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "home");
            }
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }  

            AppUser? appUser = null;
            if (!string.IsNullOrEmpty(user.LoginIdentifier))
            {
                appUser = await _userManager.FindByEmailAsync(user.LoginIdentifier);

                if (appUser == null)
                {
                    appUser = await _userManager.FindByNameAsync(user.LoginIdentifier);
                }
            }

            if (appUser == null)
            {
                ModelState.AddModelError("", "Invalid login!");
                return View(user);
            }

            var signInResult = await _singInManager.PasswordSignInAsync(appUser, user.Password, user.RememberMe, true);

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Your account is locked. Please try again later.");
                return View(user);
            }

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Invalid login!");
                return View(user);
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _singInManager.SignOutAsync();
            }
            return RedirectToAction("Index", "Home");
        }
    
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordViewModel ForgetPaVM)
        {
            if (!ModelState.IsValid)
                return View(ForgetPaVM);

            var user = await _userManager.FindByEmailAsync(ForgetPaVM.Email);
            if (user == null)
                return RedirectToAction(nameof(ForgotPasswordConfirmation));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var ResetPasswordUrl = $"<a Class=\"btn btn-danger\" href=\"{Url.Action(nameof(ResetPassword), "Auth", new { token, email = user.Email }, Request.Scheme)}\">reset your password</a>";

            var time = DateTime.Now.ToString();
            var userIp = GetUserIP().ToString();
            var Username = user.UserName.ToString();
            var Fullname = user.FullName.ToString();
            string pathToHtmlFile = "Views\\Auth\\ResetPasswordLetter.cshtml";
            string htmlContent = System.IO.File.ReadAllText(pathToHtmlFile);
            string tokenLifetimeString = FormatTokenLifetime(_tokenLifespan);

            htmlContent = htmlContent.Replace("{{action_url}}", ResetPasswordUrl);
            htmlContent = htmlContent.Replace("{{Time}}", time);
            htmlContent = htmlContent.Replace("{{IpAddress}}", userIp);
            htmlContent = htmlContent.Replace("{{Username}}", Username);
            htmlContent = htmlContent.Replace("{{Fullname}}", Fullname);
            htmlContent = htmlContent.Replace("{{TokenLifetime}}", tokenLifetimeString);

            var message = new EmailVM(user.Email, htmlContent, "reset password");
            _mailService.SendEmail(message);

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var user = _userManager.FindByNameAsync(email);
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordModel);
            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
                RedirectToAction(nameof(ResetPasswordConfirmation));
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        public static string GetUserIP()
        {
            string ip = string.Empty;

            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress[] addresses = hostEntry.AddressList;

                foreach (IPAddress address in addresses)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ip = address.ToString();
                        break;
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Error retrieving IP address: " + ex.Message);
            }

            return ip;
        }
        private static string FormatTokenLifetime(TimeSpan tokenLifetime)
        {
            string formattedLifetime = "";

            if (tokenLifetime.Days > 0)
            {
                formattedLifetime += $"{tokenLifetime.Days} days ";
            }

            if (tokenLifetime.Hours > 0)
            {
                formattedLifetime += $"{tokenLifetime.Hours} hours";
            }

            return formattedLifetime.Trim();
        }
        public string GenerateRandomPassword()
        {
            const string uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
            const string nonAlphabeticChars = "!@#$%^&*";
            const string numericChars = "0123456789";
            const string allChars = uppercaseLetters + lowercaseLetters + nonAlphabeticChars + numericChars;

            Random random = new Random();

            StringBuilder passwordBuilder = new StringBuilder();
            passwordBuilder.Append(uppercaseLetters[random.Next(uppercaseLetters.Length)]);
            passwordBuilder.Append(lowercaseLetters[random.Next(lowercaseLetters.Length)]);
            passwordBuilder.Append(nonAlphabeticChars[random.Next(nonAlphabeticChars.Length)]);
            passwordBuilder.Append(numericChars[random.Next(numericChars.Length)]);

            for (int i = 0; i < 4; i++)
            {
                passwordBuilder.Append(allChars[random.Next(allChars.Length)]);
            }

            string password = passwordBuilder.ToString();

            return password;
        }


    }
}

