using AutoMapper;
using EduHome.UI.Areas.Admin.ViewModels.CourseViewModels;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.Areas.Admin.ViewModels.SliderViewModel;
using HomeEdu.UI.Helpers.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;

namespace HomeEdu.UI.Areas.Admin.Controllers
{
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }
        [Area("Admin")]
        public async Task<IActionResult> Index()
        {

            List<Slider> Slider = await _context.Sliders.ToListAsync();
            return View(Slider);
        }
        [Area("Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Catagories = await _context.BlogCatagories.ToListAsync();
            return View();
        }
        [Area("Admin")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(SliderViewModel sliderViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(sliderViewModel);
            }
            if (!sliderViewModel.Image.CheckFileFormat("image"))
            {
                ModelState.AddModelError("Image", "Sellect Correct Format!");
                return View(sliderViewModel);
            }
            if (!sliderViewModel.Image.CheckFileLength(300))
            {
                ModelState.AddModelError("Image", "Size Must be less than 300 kb");
                return View(sliderViewModel);
            }
            string filePath = await sliderViewModel.Image.CopyFileAsync(_env.WebRootPath, "assets", "img", "slider");
            Slider sldier = _mapper.Map<Slider>(sliderViewModel);
            await _context.Sliders.AddAsync(sldier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
