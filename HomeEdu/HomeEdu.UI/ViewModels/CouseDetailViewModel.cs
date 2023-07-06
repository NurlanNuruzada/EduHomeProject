using HomeEdu.Core.Entities;

namespace HomeEdu.UI.ViewModels
{
    public class CouseDetailViewModel
    {
        public Course Course { get; set; }
        public List<Blog> Blogs { get; set; } 
        public CourseDetail CourseDetail { get; set; } = null!;
        public CourseCatagory CourseCatagory { get; set; }
        public Assesments Assesments { get; set; }
        public SkillLevel SkillLevel { get; set; }
        public Language Language { get; set; }
    }
}
