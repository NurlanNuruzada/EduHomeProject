using HomeEdu.UI.ViewModels.EmailViewModels;
using MailKit;

namespace HomeEdu.UI.Services.EmailService;
public interface IEmailService
{

    void SendEmail(EmailVM emailVM);
}
