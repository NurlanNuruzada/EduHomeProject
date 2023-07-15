using AutoMapper;
using HomeEdu.DataAccess.Context;
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
            speaker =_mapper.Map<Speaker>(createSpekerVM);
            speaker.ImagePath = filePath;
            await _context.Speakers.AddAsync(speaker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetSpeakers));
        }
    }
}
