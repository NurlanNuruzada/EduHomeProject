using AutoMapper;
using HomeEdu.Core.Entities;
using HomeEdu.UI.Areas.Admin.ViewModels.CourseViewModel;
using HomeEdu.UI.Areas.Admin.ViewModels.TeacherViewModels;

namespace HomeEdu.UI.Mapper
{
    public class TeachersProfile : Profile
    {
        public TeachersProfile()
        {
            CreateMap<CreateTeacherViewModel, Teachers>().ReverseMap();
            CreateMap<CreateTeacherViewModel, TeacherDetails>().ReverseMap();
        }
    }
}
