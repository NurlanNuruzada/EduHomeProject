namespace HomeEdu.UI.ViewModels
{
    public class EventDetailViewModel
    {
        public Event? Event { get; set; }
        public IEnumerable<EventDetail> eventDetails { get; set; } = null!;
        public IEnumerable<Speaker>? Speakers { get; set; }
    }
}
