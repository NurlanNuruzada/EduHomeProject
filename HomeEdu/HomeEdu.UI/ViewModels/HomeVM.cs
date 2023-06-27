using HomeEdu.Core.Entities;

namespace HomeEdu.UI.ViewModels
{
    public class BlogVM
    {
        public IEnumerable<Blog> Blogs { get; set; } = null!;
        public IEnumerable<BlogCatagory> blogCatagories { get; set; } = null!;
    }
}
