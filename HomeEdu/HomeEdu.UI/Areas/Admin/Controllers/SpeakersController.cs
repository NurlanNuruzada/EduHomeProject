using AutoMapper;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.Areas.Admin.ViewModels.EventViewModel;
using HomeEdu.UI.Areas.Admin.ViewModels.NoticeBoardViewModels;
using HomeEdu.UI.Areas.Admin.ViewModels.SpeakersViewModels;
using HomeEdu.UI.Helpers.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeEdu.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpeakersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public SpeakersController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }
        public async Task<IActionResult> GetSpeakers()
        {
            List<Speaker> Speakers = await _context.Speakers.ToListAsync();
            return View(Speakers);
        }
        public async Task<IActionResult> AddSpeaker()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddSpeaker(CreateSpekerVM createSpekerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createSpekerVM);
            }
            if (createSpekerVM.Image is null)
            {
                ModelState.AddModelError("Image", "Please sellect Image!");
                return View(createSpekerVM);
            }
            if (!createSpekerVM.Image.CheckFileFormat("image"))
            {
                ModelState.AddModelError("Image", "Sellect Correct Format!");
                return View(createSpekerVM);
            }
            if (!createSpekerVM.Image.CheckFileLength(300))
            {
                ModelState.AddModelError("Image", "Size Must be less than 300 kb");
                return View(createSpekerVM);
            }
            string filePath = await createSpekerVM.Image.CopyFileAsync(_env.WebRootPath, "assets", "img", "teacher");
            Speaker speaker = new();
            speaker = _mapper.Map<Speaker>(createSpekerVM);
            speaker.ImagePath = filePath;
            await _context.Speakers.AddAsync(speaker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetSpeakers));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var Speaker = await _context.Speakers.FindAsync(id);
            if (Speaker == null)
            {
                return NotFound();
            }
            return View(Speaker);
        }
        [AutoValidateAntiforgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var Speaker = await _context.Speakers.FindAsync(id);
            _context.Speakers.Remove(Speaker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetSpeakers));
        }
        public async Task<IActionResult> Update(int id)
        {
            var Speaker = await _context.Speakers.FindAsync(id);
            if (Speaker == null)
            {
                return NotFound();
            }
            UpdateSpeakerViewModel SpeakerVM = new();
            SpeakerVM.Name = Speaker.Name;
            SpeakerVM.Surname = Speaker.Surname;
            SpeakerVM.Position = Speaker.Position;
            SpeakerVM.passCode = Speaker.Position;
            return View(SpeakerVM);
        }
        [HttpPost]
        [ActionName("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, UpdateSpeakerViewModel SpeakerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(SpeakerVM);
            }
            if (SpeakerVM == null)
            {
                return NotFound();
            }
            Speaker? speaker = await _context.Speakers.FindAsync(Id);
            if (speaker == null)
            {
                return NotFound();
            }
            if (SpeakerVM.Image != null)
            {
                if (!SpeakerVM.Image.CheckFileFormat("image"))
                {
                    ModelState.AddModelError("Image", "Select Correct Format!");
                    return View(SpeakerVM);
                }

                if (!SpeakerVM.Image.CheckFileLength(300))
                {
                    ModelState.AddModelError("Image", "Size must be less than 300 KB");
                    return View(SpeakerVM);
                }

                string filePath = await SpeakerVM.Image.CopyFileAsync(_env.WebRootPath, "assets", "img", "teacher");
                speaker.ImagePath = filePath;
            }
            speaker.Name = SpeakerVM.Name;
            speaker.Surname = SpeakerVM.Surname;
            speaker.Position = SpeakerVM.Position;
            speaker.passCode = SpeakerVM.passCode;
            _context.Speakers.Update(speaker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetSpeakers));
        }
    }
}
