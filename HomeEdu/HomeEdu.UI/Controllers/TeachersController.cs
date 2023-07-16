using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.ViewModels;
using HomeEdu.UI.ViewModels.TeacherViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeEdu.UI.Controllers
{
    public class TeachersController : Controller
    {
        private readonly AppDbContext _context;
        public TeachersController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Teachers> Teachers = await _context.Teachers.ToListAsync();
            return View(Teachers);
        }
        public async Task<IActionResult> TeacherDetails(int id)
        {
            Teachers? teacher = await _context.Teachers
               .Where(e => e.Id == id)
               .Include(e => e.TeacherDetails)
               .FirstOrDefaultAsync();
            if(teacher is null)
            {
                return NotFound();
            }
            return View(teacher);
        }
    }
}
