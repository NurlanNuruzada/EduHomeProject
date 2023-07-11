using HomeEdu.UI.Services.EmailService;
using HomeEdu.UI.ViewModels.EmailViewModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;

namespace HomeEdu.UI.Controllers
{
    public class EmailController : Controller
    {
        public readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public IActionResult SentEmail()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SentEmail(EmailVM emailVM)
        {
            _emailService.SendEmail(emailVM);
            return Ok();
        }
    }
}
