using HomeEdu.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeEdu.DataAccess.Context;
using AutoMapper;
using HomeEdu.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using static HomeEdu.UI.Helpers.Utilities.AppUserRole;

namespace HomeEdu.UI.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CoursesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(int CaID = 0, string search = "", int pg = 1)
        {
            PagesVM pagesVM = new PagesVM()
            {
                Courses = await _context.Courses.ToListAsync(),
                CourseCatagories = await _context.CourseCatagories.ToListAsync(),
                Blogs = await _context.Blogs.ToListAsync()
            };

            if (pagesVM == null)
            {
                return NotFound();
            }

            const int pageSize = 6;

            if (pg < 1)
                pg = 1;

            int resCount = pagesVM.Courses.Count();
            var pager = new Pager(resCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;

            ViewBag.pager = pager;

            if (string.IsNullOrEmpty(search))
            {
                if (CaID != 0)
                {
                    var coursesByCategory = pagesVM.Courses.Where(c => c.CourseCatagoryId == CaID).ToList();
                    int coursesByCategoryCount = coursesByCategory.Count;
                    pager = new Pager(coursesByCategoryCount, pg, pageSize);
                    recSkip = (pg - 1) * pageSize;
                    pagesVM.Data = coursesByCategory.Skip(recSkip).Take(pager.PageSize).ToList();
                    ViewBag.pager = pager;
                }
                else
                {
                    pagesVM.Data = pagesVM.Courses.Skip(recSkip).Take(pager.PageSize).ToList();
                }

                return View(pagesVM);
            }
            else
            {
                var searchByTitle = pagesVM.Courses.Where(c => c.Title.ToLower().Contains(search.ToLower())).ToList();
                int searchResultCount = searchByTitle.Count;
                pager = new Pager(searchResultCount, pg, pageSize);
                recSkip = (pg - 1) * pageSize;
                pagesVM.Data = searchByTitle.Skip(recSkip).Take(pager.PageSize).ToList();
                ViewBag.pager = pager;

                return View(pagesVM);
            }
        }

    }
}
