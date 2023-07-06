using HomeEdu.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeEdu.DataAccess.Context;
using AutoMapper;
using HomeEdu.UI.ViewModels;

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
        public async Task<IActionResult> Index()
        {
            PagesVM pagesVM = new()
            {
                Courses = await _context.Courses.ToListAsync(),
                Blogs = await _context.Blogs.ToListAsync()
            };
            if(pagesVM == null)
            {
                return NotFound();
            }
            return View(pagesVM);
        }
    }
}
