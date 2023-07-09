using AutoMapper;
using HomeEdu.UI.Areas.Admin.ViewModels.BlogViewModels;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.Areas.Admin.ViewModels.CourseViewModel;
using HomeEdu.UI.Areas.Admin.ViewModels.EventViewModel;
using HomeEdu.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static HomeEdu.UI.Helpers.Utilities.AppUserRole;

namespace HomeEdu.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
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
                CourseCatagoryId = CatagoryId,
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
        [Area("Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var Course = await _context.Courses.FindAsync(id);
            if (Course == null)
            {
                return NotFound();
            }
            return View(Course);
        }
        [HttpPost]
        [Area("Admin")]
        [ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var Course = await _context.Courses.FindAsync(id);
            if (Course == null)
            {
                return NotFound();
            }
            _context.Courses.Remove(Course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Area("Admin")]
        public async Task<IActionResult> Update(int id)
        {
            Course? course = await _context.Courses
                 .Where(e => e.Id == id)
                 .Include(e => e.CourseDetail)
                 .FirstOrDefaultAsync();

            if (course == null)
            {
                return NotFound();
            }

            if (course.CourseDetail == null)
            {
                course.CourseDetail = new CourseDetail();
            }

            var courseViewModel = _mapper.Map<CourseViewModel>(course);
            Course? courseDb = await _context.Courses.FindAsync(id);
            courseViewModel.ImagePath = courseDb.ImagePath;
            courseViewModel.HowToApply = course.CourseDetail.HowToApply;
            courseViewModel.AboutCourse = course.CourseDetail.AboutCourse;
            courseViewModel.Certification = course.CourseDetail.Certification;
            courseViewModel.ClassDuration = course.CourseDetail.ClassDuration;
            courseViewModel.Duration= course.CourseDetail.Duration ;
          courseViewModel.Starts=   course.CourseDetail.Starts ;
           courseViewModel.CourseFee = course.CourseDetail.CourseFee  ;
            ViewBag.Catagories = await _context.CourseCatagories.ToListAsync();


            return View(courseViewModel);
        }

        [HttpPost]
        [Area("Admin")]
        [ActionName("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, CourseViewModel courseViewModel, int CatagoryId)
        {
            Course? course = await _context.Courses
     .Include(c => c.CourseDetail)
     .FirstOrDefaultAsync(s => s.Id == Id);

            if (course == null)
            {
                return NotFound();
            }

            course.Title = courseViewModel.Title;
            course.ImagePath = courseViewModel.ImagePath;
            course.Description = courseViewModel.Description;
            course.CourseCatagoryId = CatagoryId;

            if (course.CourseDetail == null)
            {
                course.CourseDetail = new CourseDetail();
            }

            course.CourseDetail.HowToApply = courseViewModel.HowToApply;
            course.CourseDetail.AboutCourse = courseViewModel.AboutCourse;
            course.CourseDetail.Certification = courseViewModel.Certification;
            course.CourseDetail.ClassDuration = courseViewModel.ClassDuration;
            course.CourseDetail.Duration = courseViewModel.Duration;
            course.CourseDetail.Starts = courseViewModel.Starts;
            course.CourseDetail.CourseFee = courseViewModel.CourseFee;
            _context.Entry<Course>(course).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
