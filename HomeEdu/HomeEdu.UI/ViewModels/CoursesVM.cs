using HomeEdu.Core.Entities;

namespace HomeEdu.UI.ViewModels
{
    public class CoursesVM
    {
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Blog> Blogs { get; set; }

    }
}
