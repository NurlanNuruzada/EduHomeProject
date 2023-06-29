using AutoMapper;
using HomeEdu.Core.Entities;
using HomeEdu.UI.Areas.Admin.ViewModels.SliderViewModel;
using HomeEdu.UI.Areas.Admin.ViewModels.TestimoniaViewModel;

namespace HomeEdu.UI.Mapper
{
    public class TestimonialProfile :Profile
    {
        public TestimonialProfile()
        {
            CreateMap<TestimoniaVM, testimonial>().ReverseMap();
        }
    }
}
