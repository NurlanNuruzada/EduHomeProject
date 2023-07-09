using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static HomeEdu.UI.Helpers.Utilities.AppUserRole;

namespace HomeEdu.UI.Controllers
{
    public class CourseDetailController : Controller
    {
        private readonly AppDbContext _context;

        public CourseDetailController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int id)
        {
            var courseDetailVMs = await _context.Courses
                .Include(c => c.CourseDetail)
                .Include(c => c.CourseCatagory)
                .Where(c => c.Id == id)
                .Select(c => new CouseDetailViewModel
                {
                    Blogs = _context.Blogs.Include(B=>B.BlogCatagory).ToList(),
                    Course = c,
                    CourseDetail = c.CourseDetail,
                    CourseCatagory = c.CourseCatagory,
                    Assesments = _context.Assesments.FirstOrDefault(a => a.CourseDetailId == c.CourseDetail.Id),
                    SkillLevel = _context.SkillLevels.FirstOrDefault(s => s.CourseDetailId == c.CourseDetail.Id),
                    Language = _context.Languages.FirstOrDefault(l => l.CourseDetailId == c.CourseDetail.Id),
                })
                .ToListAsync();
            if(courseDetailVMs == null)
            {
                return NotFound();
            }
            foreach (var courseDetailVM in courseDetailVMs)
            {
                if (courseDetailVM.Assesments == null)
                {
                    courseDetailVM.Assesments = new Assesments();
                }
                if (courseDetailVM.SkillLevel == null)
                {
                    courseDetailVM.SkillLevel = new SkillLevel(); 
                }
                if (courseDetailVM.Language == null)
                {
                    courseDetailVM.Language = new Language(); 
                }
            }

            return View(courseDetailVMs.FirstOrDefault());
        }

    }
}
