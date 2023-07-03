using AutoMapper;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeEdu.UI.Areas.Admin.Controllers
{

    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CourseController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [Area("Admin")]
        public async Task<IActionResult> Index()
        {
            List<Course> courses = await _context.Courses.Include(e => e.CourseCatagory).ToListAsync();
            return View(courses);
        }

    }
}
