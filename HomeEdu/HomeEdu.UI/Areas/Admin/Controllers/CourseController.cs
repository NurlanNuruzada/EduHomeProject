using AutoMapper;
using EduHome.UI.Areas.Admin.ViewModels.CourseViewModels;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.Areas.Admin.ViewModels.CourseViewModel;
using HomeEdu.UI.Areas.Admin.ViewModels.EventViewModel;
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
        [Area("Admin")]
        public async Task<IActionResult> Details(int id)
        {
            Course? course = await _context.Courses
                .Include(c => c.CourseCatagory)
                .Include(c => c.CourseDetail)
                    .ThenInclude(cd => cd.SkillLevels)
                .Include(c => c.CourseDetail)
                    .ThenInclude(cd => cd.Languages)
                .Include(c => c.CourseDetail)
                    .ThenInclude(cd => cd.Assesments)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }
        [Area("Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Catagories = await _context.CourseCatagories.ToListAsync();

            return View();
        }
        [HttpPost]
        [Area("Admin")]
        [ActionName("Create")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CourseViewModel courseViewModel, int CatagoryId)
        {

            if (ModelState.IsValid)
            {
                ViewBag.Catagories = await _context.CourseCatagories.ToListAsync();
                return View(courseViewModel);
            }
            var catagory = _context.CourseCatagories.Find(CatagoryId);
            if (catagory is null)
            {
                return BadRequest();
            }
            var Course = new Course
            {
                Title = courseViewModel.Title,
                Description = courseViewModel.Description,
                ImagePath = courseViewModel.ImagePath,
                CourseCatagoryId= CatagoryId,
                CourseDetail = new CourseDetail
                {
                    AboutCourse = courseViewModel.AboutCourse,
                    HowToApply = courseViewModel.HowToApply,
                    Certification = courseViewModel.Certification,
                    ClassDuration = courseViewModel.ClassDuration,
                    Duration = courseViewModel.Duration,
                    Starts = courseViewModel.Starts,
                    CourseFee = courseViewModel.CourseFee,
                }

            };
            await _context.Courses.AddAsync(Course);
            await _context.CourseDetails.AddAsync(Course.CourseDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}
