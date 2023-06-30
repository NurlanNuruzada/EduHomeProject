using AutoMapper;
using HomeEdu.UI.Areas.Admin.ViewModels.EventViewModel;

namespace HomeEdu.UI.Mapper
{
    public class EventDetailProfile : Profile
    {
        public EventDetailProfile()
        {
            CreateMap<EventViewModel,EventDetail>().ReverseMap();
        }
    }
}
