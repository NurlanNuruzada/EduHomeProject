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
using HomeEdu.UI.Areas.Admin.ViewModels.CourseViewModels;
using HomeEdu.UI.Helpers.Extentions;
using HomeEdu.UI.Services.Interfaces;

namespace HomeEdu.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBlogService _blogService;
        private readonly IWebHostEnvironment _env;
        public CourseController(AppDbContext context, IMapper mapper, IBlogService blogService, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _blogService = blogService;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Course> courses = await _context.Courses.Include(e => e.CourseCatagory).ToListAsync();
            return View(courses);
        }
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
        public async Task<IActionResult> Create()
        {
            ViewBag.Catagories = await _context.CourseCatagories.ToListAsync();
            return View();
        }
        [HttpPost]
        [ActionName("Create")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CreateCourseViewModel courseViewModel, int CatagoryId)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Catagories = await _context.BlogCatagories.ToListAsync();
                return View(courseViewModel);
            }
            if (courseViewModel.Image is null)
            {
                ModelState.AddModelError("Image", "Image Can't be null!");
                return View(courseViewModel);
            }
            var catagory = _context.CourseCatagories.Find(CatagoryId);
            if (catagory is null)
            {
                return NotFound("Catagory not Found");
            }
            courseViewModel.CourseCatagory = catagory;
            if (!ModelState.IsValid)
            {
                return View(courseViewModel);
            }
            if (!courseViewModel.Image.CheckFileFormat("image"))
            {
                ModelState.AddModelError("Image", "Sellect Correct Format!");
                return View(courseViewModel);
            }
            if (!courseViewModel.Image.CheckFileLength(300))
            {
                ModelState.AddModelError("Image", "Size Must be less than 300 kb");
                return View(courseViewModel);
            }
            string filePath = await courseViewModel.Image.CopyFileAsync(_env.WebRootPath, "assets", "img", "course");
            if (catagory is null)
            {
                return BadRequest();
            }
            var Course = new Course
            {
                Title = courseViewModel.Title,
                Description = courseViewModel.Description,
                ImagePath = filePath,
                CourseCatagoryId = CatagoryId,
                CourseDetail = new CourseDetail
                {
                    CourseDescription = courseViewModel.CourseDescription,
                    ClassDuration = courseViewModel.ClassDuration,
                    Duration = courseViewModel.Duration,
                    Starts = courseViewModel.Starts,
                    CourseFee = courseViewModel.CourseFee,
                    StudentsCount = courseViewModel.StudentsCount,
                }

            };
            await _context.Courses.AddAsync(Course);
            await _context.CourseDetails.AddAsync(Course.CourseDetail);
            await _context.SaveChangesAsync();
            var language = new Language
            {
                CourseDetailId = Course.CourseDetail.Id,
                language = courseViewModel.Languages
            };
            var Skill = new SkillLevel
            {
                CourseDetailId = Course.CourseDetail.Id,
                skillLevel = courseViewModel.SkillLevels
            };
            var Assement = new Assesments
            {
                CourseDetailId = Course.CourseDetail.Id,
                assesments = courseViewModel.Assesments
            };
            await _context.AddRangeAsync(Assement, language, Skill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
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
            UpdateCourseViewModel updateCourseView = new();
            updateCourseView = _mapper.Map<UpdateCourseViewModel>(course);
            Course? courseDb = await _context.Courses.FindAsync(id);
            updateCourseView.CourseDescription = course.CourseDetail.CourseDescription;
            updateCourseView.ClassDuration = course.CourseDetail.ClassDuration;
            updateCourseView.Duration = course.CourseDetail.Duration;
            updateCourseView.Starts = course.CourseDetail.Starts;
            updateCourseView.StudentsCount = course.CourseDetail.StudentsCount;
            updateCourseView.CourseFee = course.CourseDetail.CourseFee;
            updateCourseView.CourseDetail.Assesments = course.CourseDetail.Assesments;
            updateCourseView.CourseDetail.Languages = course.CourseDetail.Languages;
            updateCourseView.CourseDetail.SkillLevels = course.CourseDetail.SkillLevels;
            ViewBag.Catagories = await _context.CourseCatagories.ToListAsync();
            return View(updateCourseView);
        }
        [HttpPost]
        [ActionName("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, UpdateCourseViewModel courseViewModel, int CatagoryId)
        {
            Course? course = await _context.Courses
     .Include(c => c.CourseDetail)
     .FirstOrDefaultAsync(s => s.Id == Id);

            if (course == null)
            {
                return NotFound();
            }
            course.Title = courseViewModel.Title;
            //course.ImagePath = courseViewModel.ImagePath;
            course.Description = courseViewModel.Description;
            course.CourseCatagoryId = CatagoryId;
            if (course.CourseDetail == null)
            {
                course.CourseDetail = new CourseDetail();
            }
            course.CourseDetail.CourseDescription = courseViewModel.CourseDescription;
            course.CourseDetail.ClassDuration = courseViewModel.ClassDuration;
            course.CourseDetail.Duration = courseViewModel.Duration;
            course.CourseDetail.Starts = courseViewModel.Starts;
            course.CourseDetail.StudentsCount = courseViewModel.StudentsCount;
            course.CourseDetail.CourseFee = courseViewModel.CourseFee;

            var skillLevel = await _context.SkillLevels.FirstOrDefaultAsync(a => a.CourseDetailId == course.CourseDetail.Id);
            var assesments = await _context.Assesments.FirstOrDefaultAsync(a => a.CourseDetailId == course.CourseDetail.Id);
            var language = await _context.Languages.FirstOrDefaultAsync(a => a.CourseDetailId == course.CourseDetail.Id);
            if (courseViewModel.Languages is not null)
            {
                language.language = courseViewModel.Languages;
            }
            if (courseViewModel.SkillLevels is not null)
            {
                skillLevel.skillLevel = courseViewModel.SkillLevels;
            }
            if (courseViewModel.Assesments is not null)
            {
                assesments.assesments = courseViewModel.Assesments;
            }

            _context.Entry<Course>(course).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> GetCourseCaragory()
        {
            List<CourseCatagory> courseCatagory = await _context.CourseCatagories.ToListAsync();
            return View(courseCatagory);
        }
        public IActionResult CreateCourseCaragory()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateCourseCaragory(CourseCatagory CourseCatagory)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Couldn't add Categry!");
                return View();
            }
            if (CourseCatagory is null)
            {
                return View();
            }
            CourseCatagory CCatagory = await _context.CourseCatagories.FirstOrDefaultAsync(C => C.Catagory == CourseCatagory.Catagory);
            if (CCatagory is not null)
            {
                ModelState.AddModelError("", "this Categry already exist!");
                return View();
            }
            _context.CourseCatagories.AddAsync(CourseCatagory);
            _context.SaveChangesAsync();
            return RedirectToAction("Index", "Course");
        }
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteCourseCategory(int Id)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            CourseCatagory courseCatagory = await _context.CourseCatagories.FindAsync(Id);
            if (courseCatagory is null)
            {
                return NotFound();
            }
            _context.Entry<CourseCatagory>(courseCatagory).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("Index", "Blog");
        }
    }
}
