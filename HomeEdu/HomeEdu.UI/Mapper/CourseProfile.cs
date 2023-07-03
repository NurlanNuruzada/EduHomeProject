using AutoMapper;
using HomeEdu.Core.Entities;
using HomeEdu.UI.Areas.Admin.ViewModels.CourseViewModel;

namespace HomeEdu.UI.Mapper
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CourseViewModel, Course>().ReverseMap();
        }
    }
}
