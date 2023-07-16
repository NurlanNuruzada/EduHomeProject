using HomeEdu.Core.Entities;

namespace HomeEdu.UI.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Blog> Blogs { get; set; } = null!;
        public IEnumerable<BlogCatagory> blogCatagories { get; set; } = null!;
        public IEnumerable<Slider> sliders { get; set; } = null!;
        public IEnumerable<testimonial> testimonials { get; set; } = null!;
        public IEnumerable<NoticeBoard> noticeBoards { get; set; } = null!; 
        public IEnumerable<Event> events { get; set; } = null!; 
        public IEnumerable<Course> Courses { get; set; } = null!; 
        public IEnumerable<BlogCatagory>? BlogCatagories { get; set; }

    }
}
