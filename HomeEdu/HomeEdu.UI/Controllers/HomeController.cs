using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static HomeEdu.UI.Helpers.Utilities.AppUserRole;

namespace HomeEdu.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new()
            {
                Blogs = await _context.Blogs.ToListAsync(),
                sliders = await _context.Sliders.ToListAsync(),
                testimonials = await _context.testimonials.ToListAsync(),
                Courses = await _context.Courses.ToListAsync(),
                noticeBoards = await _context.noticeBoards.ToListAsync(),
                events = await _context.Events.ToListAsync()
            };
            return View(homeVM);
        }
    }
}
