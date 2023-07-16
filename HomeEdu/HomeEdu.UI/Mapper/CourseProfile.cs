using AutoMapper;
using HomeEdu.Core.Entities;
using HomeEdu.UI.Areas.Admin.ViewModels.CourseViewModel;
using HomeEdu.UI.Areas.Admin.ViewModels.CourseViewModels;

namespace HomeEdu.UI.Mapper
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CourseViewModel, Course>().ReverseMap();
            CreateMap<UpdateCourseViewModel, Course>().ReverseMap();
        }
    }
}
