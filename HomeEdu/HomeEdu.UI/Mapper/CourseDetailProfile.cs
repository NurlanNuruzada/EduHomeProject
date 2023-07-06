using AutoMapper;
using HomeEdu.UI.Areas.Admin.ViewModels.BlogViewModels;
using HomeEdu.Core.Entities;
using HomeEdu.UI.Areas.Admin.ViewModels.CourseViewModel;

namespace HomeEdu.UI.Mapper
{
    public class CourseDetailProfile : Profile
    {
        public CourseDetailProfile()
        {
            CreateMap<CourseViewModel, CourseDetail>().ReverseMap();
        }
    }
}
