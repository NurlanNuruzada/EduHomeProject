using AutoMapper;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.DataAccess.Migrations;
using HomeEdu.UI.Areas.Admin.ViewModels.SliderViewModel;
using HomeEdu.UI.Areas.Admin.ViewModels.TestimoniaViewModel;
using HomeEdu.UI.Helpers.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static HomeEdu.UI.Helpers.Utilities.AppUserRole;

namespace HomeEdu.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class TestimonialController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public TestimonialController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {

            List<testimonial> testimonials = await _context.testimonials.ToListAsync();
            return View(testimonials);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Catagories = await _context.testimonials.ToListAsync();
            return View();
        }
        [HttpPost]
        [ActionName("Create")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(TestimoniaVM testimoniaVM)
        {
            if (!ModelState.IsValid)
            {
                return View(testimoniaVM);
            }
            if (!testimoniaVM.Image.CheckFileFormat("image"))
            {
                ModelState.AddModelError("Image", "Sellect Correct Format!");
                return View(testimoniaVM);
            }
            if (!testimoniaVM.Image.CheckFileLength(300))
            {
                ModelState.AddModelError("Image", "Size Must be less than 300 kb");
                return View(testimoniaVM);
            }
            string filePath = await testimoniaVM.Image.CopyFileAsync(_env.WebRootPath, "assets", "img", "slider");
            testimonial testimonial = _mapper.Map<testimonial>(testimoniaVM);
            testimonial.ImagePath = filePath;
            await _context.testimonials.AddAsync(testimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Delete(int id)
        {
            var testimonial = await _context.testimonials.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }
            return View(testimonial);
        }
        [HttpPost]
        [ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var testimonials = await _context.testimonials.FindAsync(id);
            if (testimonials == null)
            {
                return NotFound();
            }
            _context.testimonials.Remove(testimonials);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var testimonials = await _context.testimonials.FindAsync(id);
            if (testimonials == null)
            {
                return NotFound();
            }

            var testimoniaVM = new UpdateTestimonialViewModel
            {
                Name = testimonials.Name,
                Surname = testimonials.Surname,
                Position = testimonials.Position,
                Description = testimonials.Description,
            };

            return View(testimoniaVM);
        }
        [ActionName("Update")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateTestimonialViewModel testimoniaVM)
        {
            if (id != testimoniaVM.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(testimoniaVM);
            }

            testimonial? Testimonials = await _context.testimonials.FindAsync(id);
            if (Testimonials == null)
            {
                return BadRequest();
            }
            if (testimoniaVM.Image != null)
            {
                if (!testimoniaVM.Image.CheckFileFormat("image"))
                {
                    ModelState.AddModelError("Image", "Select Correct Format!");
                    return View(testimoniaVM);
                }

                if (!testimoniaVM.Image.CheckFileLength(300))
                {
                    ModelState.AddModelError("Image", "Size must be less than 300 KB");
                    return View(testimoniaVM);
                }
                testimonial testimonial = _mapper.Map<testimonial>(testimoniaVM);
                string filePath = await testimoniaVM.Image.CopyFileAsync(_env.WebRootPath, "assets", "img", "slider");
                Testimonials.ImagePath = filePath;
            }
            Testimonials.Name = testimoniaVM.Name;
            Testimonials.Surname = testimoniaVM.Surname;
            Testimonials.Position = testimoniaVM.Position;
            Testimonials.Description = testimoniaVM.Description;
            _context.Entry(Testimonials).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
