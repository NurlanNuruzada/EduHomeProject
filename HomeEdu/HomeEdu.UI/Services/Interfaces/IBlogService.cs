using HomeEdu.Core.Entities;
using HomeEdu.UI.Areas.Admin.ViewModels.BlogViewModels;
namespace HomeEdu.UI.Services.Interfaces;

public interface IBlogService
{
    Task<List<Blog>> GetAllBlogAsync();
    Task<BlogVM> GetBlogDetailAsync(int Id);
    Task<bool> CreateBlogAsync(BlogVM blogVM, int categoryId);
}
