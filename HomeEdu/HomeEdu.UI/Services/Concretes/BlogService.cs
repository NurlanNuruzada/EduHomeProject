using AutoMapper;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.Areas.Admin.ViewModels.BlogViewModels;
using HomeEdu.UI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeEdu.UI.Services.Concretes;

public class BlogService : IBlogService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public BlogService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<bool> CreateBlogAsync(BlogVM blogVM, int categoryId)
    {
        throw new NotImplementedException();
    }

    async Task<List<Blog>> IBlogService.GetAllBlogAsync()
    {
        var blogs = await _context.Blogs
     .Include(c => c.BlogCatagory)
     .ToListAsync();

        var blogVMs = _mapper.Map<List<Blog>>(blogs);

        return blogVMs;
    }
    Task<BlogVM> IBlogService.GetBlogDetailAsync(int Id)
    {
        throw new NotImplementedException();
    }
}


