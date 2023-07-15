using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.Areas.Admin.ViewModels.SpeakersViewModels
{
    public class UpdateSpeakerViewModel
    {
        public IFormFile? Image { get; set; }
        public string? Name { get; set; } = null!;
        public string? Surname { get; set; } = null!;
        public string? passCode { get; set; }
        public string? Position { get; set; } = null!;
    }
}
