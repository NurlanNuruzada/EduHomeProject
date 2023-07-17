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

            var filteredCourses = pagesVM.Courses;

            if (!string.IsNullOrEmpty(search))
            {
                filteredCourses = filteredCourses.Where(c => c.Title.ToLower().Contains(search.ToLower()));
            }

            if (CaID != 0)
            {
                filteredCourses = filteredCourses.Where(c => c.CourseCatagoryId == CaID);
            }

            int resCount = filteredCourses.Count();
            var pager = new Pager(resCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;

            ViewBag.pager = pager;

            pagesVM.Data = filteredCourses.Skip(recSkip).Take(pager.PageSize).ToList();

            return View(pagesVM);
        }


    }
}
