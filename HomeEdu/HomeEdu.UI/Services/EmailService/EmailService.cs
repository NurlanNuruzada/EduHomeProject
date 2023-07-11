using HomeEdu.UI.ViewModels.EmailViewModels;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Org.BouncyCastle.Asn1.Ocsp;

namespace HomeEdu.UI.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(EmailVM emailVM)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailConfiguration:Username").Value));
            email.To.Add(MailboxAddress.Parse(emailVM.To));
            email.Subject = emailVM.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = emailVM.Body };

            using var smtp = new SmtpClient();
            int port = _config.GetValue<int>("EmailConfiguration:Port");
            string smtpServer = _config.GetSection("EmailConfiguration:SmtpServer").Value;
            string username = _config.GetSection("EmailConfiguration:Username").Value;
            string password = _config.GetSection("EmailConfiguration:Password").Value;

            smtp.Connect(smtpServer, port, SecureSocketOptions.Auto);
            smtp.Authenticate(username, password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

    }
}
