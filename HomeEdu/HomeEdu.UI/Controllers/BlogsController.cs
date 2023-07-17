using AutoMapper;
using HomeEdu.UI.Areas.Admin.ViewModels.BlogViewModels;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using static HomeEdu.UI.Helpers.Utilities.AppUserRole;

namespace HomeEdu.UI.Controllers
{
    public class BlogsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public BlogsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(int CaID,string search,int pg = 1)
        {
            PagesVM pagesVM = new()
            {
                Blogs = await _context.Blogs.ToListAsync(),
                Courses = await _context.Courses.Include(c => c.CourseDetail).ToListAsync(),
                BlogCatagories = await _context.BlogCatagories.ToListAsync(),
            };
            if (pagesVM == null)
            {
                return NotFound();
            }
            const int pageSize = 6;
            if (pg < 1)
                pg = 1;
            int rescCout = pagesVM.Blogs.Count();
            var pager = new Pager(rescCout, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            this.ViewBag.pager = pager;
            pagesVM.blgoData = pagesVM.Blogs.Skip(recSkip).Take(pager.PageSize).ToList();
            if (string.IsNullOrEmpty(search))
            {
                if (CaID != 0)
                {
                    var BlogsByCategory = pagesVM.blgoData.Where(c => c.BlogCatagoryId == CaID).ToList();
                    int BlogsByCategoryCount = BlogsByCategory.Count;
                    pager = new Pager(BlogsByCategoryCount, pg, pageSize);
                    recSkip = (pg - 1) * pageSize;
                    pagesVM.blgoData = BlogsByCategory.Skip(recSkip).Take(pager.PageSize).ToList();
                    ViewBag.pager = pager;
                }
                else
                {
                    pagesVM.blgoData = pagesVM.blgoData.Skip(recSkip).Take(pager.PageSize).ToList();
                }

                return View(pagesVM);
            }
            else
            {
                var searchByTitle = pagesVM.blgoData.Where(c => c.Title.ToLower().Contains(search.ToLower())).ToList();
                int searchResultCount = searchByTitle.Count;
                pager = new Pager(searchResultCount, pg, pageSize);
                recSkip = (pg - 1) * pageSize;
                pagesVM.blgoData = searchByTitle.Skip(recSkip).Take(pager.PageSize).ToList();
                ViewBag.pager = pager;
                return View(pagesVM);
            }
        }
    }
}
