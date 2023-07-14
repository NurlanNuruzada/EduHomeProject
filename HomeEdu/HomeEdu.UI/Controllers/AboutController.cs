using HomeEdu.DataAccess.Context;
using HomeEdu.UI.ViewModels;
using HomeEdu.UI.ViewModels.AboutViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HomeEdu.UI.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;
        public AboutController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            AboutVM aboutVM = new()
            {
                testimonials = await _context.testimonials.ToListAsync(),
                noticeBoards = await _context.noticeBoards.ToListAsync(),
                Courses = await _context.Courses.ToListAsync(),
            };
            return View(aboutVM);
        }
    }
}
