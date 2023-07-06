using AutoMapper;
using HomeEdu.UI.Areas.Admin.ViewModels.BlogViewModels;
using HomeEdu.Core.Entities;

namespace HomeEdu.UI.Mapper
{
    public class BlogProfile:Profile
    {
        public BlogProfile()
        {
            CreateMap<BlogVM,Blog>().ReverseMap();
        }
    }
}
