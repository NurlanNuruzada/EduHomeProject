using AutoMapper;
using HomeEdu.UI.Areas.Admin.ViewModels.SpeakersViewModels;

namespace HomeEdu.UI.Mapper
{
    public class SpeakerProfile : Profile
    {
        public SpeakerProfile()
        {
            CreateMap<Speaker, CreateSpekerVM>().ReverseMap();
        }
    }
}
