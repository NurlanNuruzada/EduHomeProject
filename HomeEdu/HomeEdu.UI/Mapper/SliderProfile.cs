using AutoMapper;
using HomeEdu.Core.Entities;
using HomeEdu.UI.Areas.Admin.ViewModels.SliderViewModel;

namespace HomeEdu.UI.Mapper
{
    public class SliderProfile : Profile
    {
        public SliderProfile()
        {
            CreateMap<SliderViewModel,Slider>().ReverseMap();
        }
    }
}
