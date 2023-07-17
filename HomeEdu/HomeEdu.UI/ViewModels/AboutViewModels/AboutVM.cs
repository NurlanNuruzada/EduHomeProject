using HomeEdu.Core.Entities;

namespace HomeEdu.UI.ViewModels.AboutViewModels
{
    public class AboutVM
    {
        public IEnumerable<testimonial> testimonials { get; set; } = null!;
        public IEnumerable<NoticeBoard> noticeBoards { get; set; } = null!;
        public IEnumerable<Course> Courses { get; set; } = null!;
        public IEnumerable<Teachers> Teachers { get; set; } = null!;
    }
}
