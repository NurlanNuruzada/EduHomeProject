using HomeEdu.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace HomeEdu.UI.Areas.Admin.ViewModels.EventViewModels
{
    public class AddEventSpeakerViewModel
    {
        public Event @event { get; set; } = null!;
        public int? EventDetailId { get; set; }
        public EventDetail? EventDetail { get; set; }
        public IEnumerable<Speaker> speakers { get; set; } = null!;
        [Required(ErrorMessage = "Speaker is required.")]
        public int SpeakerId { get; set; }

    }
}
