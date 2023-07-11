namespace HomeEdu.UI.ViewModels.EmailViewModels
{

    public class EmailVM
    {
        public EmailVM()
        {

        }
        public EmailVM(string to, string body, string subject):this()
        {
            To = to;
            Body = body;
            Subject = subject;
        }

        public string To { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
    }
}
