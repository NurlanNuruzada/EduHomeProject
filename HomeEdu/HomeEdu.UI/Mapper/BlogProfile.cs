using AutoMapper;
using EduHome.UI.Areas.Admin.ViewModels.CourseViewModels;
using HomeEdu.Core.Entities;

namespace HomeEdu.UI.Mapper
{
    public class BlogProfile:Profile
    {
        public BlogProfile()
        {
            CreateMap<BlogPostVM,Blog>().ReverseMap();
        }
    }
}
