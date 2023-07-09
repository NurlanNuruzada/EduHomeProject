using AutoMapper;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.Areas.Admin.ViewModels.SliderViewModel;
using HomeEdu.UI.Helpers.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static HomeEdu.UI.Helpers.Utilities.AppUserRole;

namespace HomeEdu.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
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
            sldier.ImagePath = filePath;
            await _context.Sliders.AddAsync(sldier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        [Area("Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }
        [Area("Admin")]
        [HttpPost]
        [ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound();
            }
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Area("Admin")]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound();
            }

            var sliderViewModel = new SliderViewModel
            {
                Id = slider.Id,
                Title = slider.Title,
                Description = slider.Description
            };

            return View(sliderViewModel);
        }
        [Area("Admin")]
        [HttpPost]
        [ActionName("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, SliderViewModel updatedSliderViewModel)
        {
            if (id != updatedSliderViewModel.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(updatedSliderViewModel);
            }

            Slider? slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                return BadRequest();
            }

            if (updatedSliderViewModel.Image != null)
            {
                if (!updatedSliderViewModel.Image.CheckFileFormat("image"))
                {
                    ModelState.AddModelError("Image", "Select Correct Format!");
                    return View(updatedSliderViewModel);
                }

                if (!updatedSliderViewModel.Image.CheckFileLength(300))
                {
                    ModelState.AddModelError("Image", "Size must be less than 300 KB");
                    return View(updatedSliderViewModel);
                }

                string filePath = await updatedSliderViewModel.Image.CopyFileAsync(_env.WebRootPath, "assets", "img", "slider");
                slider.ImagePath = filePath;
            }

            slider.Title = updatedSliderViewModel.Title;
            slider.Description = updatedSliderViewModel.Description;

            _context.Entry(slider).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
