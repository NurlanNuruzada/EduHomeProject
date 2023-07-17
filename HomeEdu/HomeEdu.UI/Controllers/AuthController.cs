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
using NuGet.Common;
using NuGet.Protocol;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using System.Web.Helpers;

namespace HomeEdu.UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> signInManager;
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
            signInManager = singInManager;
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
        private async Task SendEmailConfirmation(AppUser user)
        {
            await _userManager.AddToRoleAsync(user, AppUserRole.Admin);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationUrl = $"<a href=\"{Url.Action(nameof(ConfirmEmail), "Auth", new { userId = user.Id, token }, Request.Scheme)}\">Confirm Your Accound</a>";
            var time = DateTime.Now.ToString();
            var userIp = GetUserIP().ToString();
            var username = user.UserName.ToString();

            string pathToHtmlFile = "Views\\Auth\\EmailConfirmationLetter.cshtml";
            string htmlContent = System.IO.File.ReadAllText(pathToHtmlFile);
            string tokenLifetimeString = FormatTokenLifetime(_tokenLifespan);

            htmlContent = htmlContent.Replace("{{action_url}}", confirmationUrl);
            htmlContent = htmlContent.Replace("{{Time}}", time);
            htmlContent = htmlContent.Replace("{{IpAddress}}", userIp);
            htmlContent = htmlContent.Replace("{{Username}}", username);
            htmlContent = htmlContent.Replace("{{Fullname}}", username);
            htmlContent = htmlContent.Replace("{{TokenLifetime}}", tokenLifetimeString);

            var message = new EmailVM(user.Email, htmlContent, "Confirm your email");
            _mailService.SendEmail(message);
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

            await _userManager.AddToRoleAsync(user, AppUserRole.Member);
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                await SendEmailConfirmation(user);
                ViewBag.ConfirmationEmailSent = true;
            }
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
        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginVM model = new LoginVM
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(model);
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Auth",
                                    new { ReturnUrl = returnUrl });

            var properties =
                signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginVM loginViewModel = new LoginVM
            {
                ReturnUrl = returnUrl,
                ExternalLogins =
                        (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState
                    .AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return View("Login", loginViewModel);
            }
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState
                    .AddModelError(string.Empty, "Error loading external login information.");

                return View("Login", loginViewModel);
            }
            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new AppUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await _userManager.CreateAsync(user);
                        await _userManager.AddToRoleAsync(user, AppUserRole.Member);
                    }
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        await SendEmailConfirmation(user);
                        ViewBag.ConfirmationEmailSent = true;
                    }
                    await _userManager.AddLoginAsync(user, info);
                    await signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }
                ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
                ViewBag.ErrorMessage = "Please contact support on Pragim@PragimTech.com";
                return View("Error");
            }

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

            var signInResult = await signInManager.PasswordSignInAsync(appUser, user.Password, user.RememberMe, true);

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
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await signInManager.SignOutAsync();
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
            var ResetPasswordUrl = $"<a href=\"{Url.Action(nameof(ResetPassword), "Auth", new { email = user.Email, token }, Request.Scheme)}\">reset your password</a>";

            var time = DateTime.Now.ToString();
            var userIp = GetUserIP().ToString();
            var Username = user.UserName.ToString();
            var Fullname = user.UserName.ToString();
            string pathToHtmlFile = "Views\\Auth\\ResetPasswordLetter.cshtml";
            string htmlContent = System.IO.File.ReadAllText(pathToHtmlFile);
            string tokenLifetimeString = FormatTokenLifetime(_tokenLifespan);

            htmlContent = htmlContent.Replace("{{action_url}}", ResetPasswordUrl);
            htmlContent = htmlContent.Replace("{{Time}}", time);
            htmlContent = htmlContent.Replace("{{IpAddress}}", userIp);
            htmlContent = htmlContent.Replace("{{Username}}", Username);
            htmlContent = htmlContent.Replace("{{Fullname}}", Username);
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
        [AutoValidateAntiforgeryToken]
        public IActionResult ResetPassword(string token, string email)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "home");
            }
            var user = _userManager.FindByNameAsync(email);
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "home");
            }
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
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }
        [HttpGet]
        [AutoValidateAntiforgeryToken]
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
    }
}

