using AutoMapper;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.Areas.Admin.ViewModels.SpeakersViewModels;
using HomeEdu.UI.Areas.Admin.ViewModels.TeacherViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeEdu.UI.Helpers.Extentions;
using System.Xml.Linq;

namespace HomeEdu.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeachersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public TeachersController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Teachers> Teachers = await _context.Teachers.ToListAsync();
            return View(Teachers);
        }
        public async Task<IActionResult> Addteacher()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Addteacher(CreateTeacherViewModel CreateTeacherViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(CreateTeacherViewModel);
            }
            if (CreateTeacherViewModel.Image is null)
            {
                ModelState.AddModelError("Image", "Please sellect Image!");
                return View(CreateTeacherViewModel);
            }
            if (!CreateTeacherViewModel.Image.CheckFileFormat("image"))
            {
                ModelState.AddModelError("Image", "Sellect Correct Format!");
                return View(CreateTeacherViewModel);
            }
            if (!CreateTeacherViewModel.Image.CheckFileLength(600))
            {
                ModelState.AddModelError("Image", "Size Must be less than 300 kb");
                return View(CreateTeacherViewModel);
            }
            string filePath = await CreateTeacherViewModel.Image.CopyFileAsync(_env.WebRootPath, "assets", "img", "teacher");
            Teachers teachers = new();
            TeacherDetails teacherDetails = new();
            teacherDetails = _mapper.Map<TeacherDetails>(CreateTeacherViewModel);
            await _context.AddAsync(teacherDetails);
            await _context.SaveChangesAsync();
            teachers = _mapper.Map<Teachers>(CreateTeacherViewModel);
            teachers.TeacherDetailsId = teacherDetails.Id;
            teachers.ImagePath = filePath;
            await _context.AddAsync(teachers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [AutoValidateAntiforgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var teachers = await _context.Teachers.FindAsync(id);
            _context.Teachers.Remove(teachers);
            var TeacherDetail = await _context.TeacherDetails.FirstOrDefaultAsync(D => D.Id == teachers.TeacherDetailsId);
            _context.TeacherDetails.Remove(TeacherDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            var teachers = await _context.Teachers
              .Include(t => t.TeacherDetails)
              .FirstOrDefaultAsync(t => t.Id == id);
            if (teachers == null)
            {
                return NotFound();
            }

            UpdateTeacherViewModel teacherVM = new UpdateTeacherViewModel
            {
                Id = teachers.Id,
                TeacherDetailsId = teachers.TeacherDetailsId,
                Name = teachers.Name,
                Surname = teachers.Surname,
                Profession = teachers.Profession,
                Degree = teachers.TeacherDetails?.Degree,
                Exoerience = teachers.TeacherDetails?.Exoerience,
                Hobbies = teachers.TeacherDetails?.Hobbies,
                Faculty = teachers.TeacherDetails?.Faculty,
                MailAdress = teachers.TeacherDetails?.MailAdress,
                PhoneNumber = teachers.TeacherDetails?.PhoneNumber,
                Skype = teachers.TeacherDetails?.Skype,
                Language = teachers.TeacherDetails.Language,
                TeamLeader = teachers.TeacherDetails.TeamLeader,
                Development = teachers.TeacherDetails.Development,
                Design = teachers.TeacherDetails.Design,
                Innovation = teachers.TeacherDetails.Innovation,
                Communucation = teachers.TeacherDetails.Communucation,
                AboutMe = teachers.TeacherDetails.AboutMe
            };
            return View(teacherVM);
        }
        [HttpPost]
        [ActionName("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, UpdateTeacherViewModel teacherVM)
        {
            if (!ModelState.IsValid)
            {
                return View(teacherVM);
            }
            if (teacherVM == null)
            {
                return NotFound();
            }
            Teachers? teachers = await _context.Teachers.FindAsync(Id);
            var TeacherDetails = await _context.TeacherDetails.FirstOrDefaultAsync(D => D.Id == teachers.TeacherDetailsId);
            if (teachers == null)
            {
                return NotFound();
            }
            if (TeacherDetails == null)
            {
                return NotFound();
            }
            if (teacherVM.Image != null)
            {
                if (!teacherVM.Image.CheckFileFormat("image"))
                {
                    ModelState.AddModelError("Image", "Select Correct Format!");
                    return View(teacherVM);
                }

                if (!teacherVM.Image.CheckFileLength(300))
                {
                    ModelState.AddModelError("Image", "Size must be less than 300 KB");
                    return View(teacherVM);
                }

                string filePath = await teacherVM.Image.CopyFileAsync(_env.WebRootPath, "assets", "img", "teacher");
                teachers.ImagePath = filePath;
            }
            teachers.TeacherDetails.Degree = teacherVM.Degree;
            teachers.TeacherDetails.Exoerience = teacherVM.Exoerience;
            teachers.TeacherDetails.Hobbies = teacherVM.Hobbies;
            teachers.TeacherDetails.Faculty = teacherVM.Faculty;
            teachers.TeacherDetails.MailAdress = teacherVM.MailAdress;
            teachers.TeacherDetails.PhoneNumber = teacherVM.PhoneNumber;
            teachers.TeacherDetails.Skype = teacherVM.Skype;
            teachers.TeacherDetails.Language = teacherVM.Language;
            teachers.TeacherDetails.TeamLeader = teacherVM.TeamLeader;
            teachers.TeacherDetails.Development = teacherVM.Development;
            teachers.TeacherDetails.Design = teacherVM.Design;
            teachers.TeacherDetails.Innovation = teacherVM.Innovation;
            teachers.TeacherDetails.Communucation = teacherVM.Communucation;
            teachers.TeacherDetails.AboutMe = teacherVM.AboutMe;
            teachers.Name = teacherVM.Name;
            teachers.Surname = teacherVM.Surname;
            teachers.Profession = teacherVM.Profession;

            _context.UpdateRange(teachers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int Id)
        {
            Teachers? teachers = await _context.Teachers
                .Where(e => e.Id == Id)
                .Include(e => e.TeacherDetails)
                .FirstOrDefaultAsync();

            return View(teachers);
        }
    }
}
