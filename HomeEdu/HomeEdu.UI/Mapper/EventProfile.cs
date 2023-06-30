using AutoMapper;
using HomeEdu.UI.Areas.Admin.ViewModels.EventViewModel;

namespace HomeEdu.UI.Mapper
{
    public class EventProfile:Profile
    {
        public EventProfile()
        {
            CreateMap<EventViewModel,Event>().ReverseMap();
        }
    }
}
