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
        public async Task<IActionResult> Index(int pg = 1)
        {
            PagesVM pagesVM = new()
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
            int rescCout = pagesVM.Courses.Count();
            var pager = new Pager(rescCout, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            this.ViewBag.pager = pager;
            pagesVM.Data = pagesVM.Courses.Skip(recSkip).Take(pager.PageSize).ToList();
            return View(pagesVM);
        }
    }
}
