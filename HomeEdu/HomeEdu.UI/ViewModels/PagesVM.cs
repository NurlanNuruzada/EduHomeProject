using HomeEdu.Core.Entities;

namespace HomeEdu.UI.ViewModels
{
    public class PagesVM
    {
        public IEnumerable<Course>? Courses { get; set; }
        public IEnumerable<Blog>? Blogs { get; set; }
        public IEnumerable<Event>? Events { get; set; }
        public IEnumerable<BlogCatagory>? BlogCatagories { get; set; }
        public IEnumerable<CourseCatagory>? CourseCatagories { get; set; }
    }
}
