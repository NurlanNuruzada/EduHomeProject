using AutoMapper;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.Areas.Admin.ViewModels.EventViewModel;
using HomeEdu.UI.Areas.Admin.ViewModels.SliderViewModel;
using HomeEdu.UI.Areas.Admin.ViewModels.TestimoniaViewModel;
using HomeEdu.UI.Helpers.Extentions;
using HomeEdu.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace HomeEdu.UI.Areas.Admin.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public EventController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }
        [Area("Admin")]
        public async Task<IActionResult> Index()
        {

            List<Event> events = await _context.Events.Include(e => e.EventDetail).ToListAsync();
            return View(events);
        }
        [Area("Admin")]
        public async Task<IActionResult> Details(int Id)
        {
            Event? @event = await _context.Events
                .Where(e => e.Id == Id)
                .Include(e => e.EventDetail)
                .ThenInclude(ed => ed.Speakers)
                .FirstOrDefaultAsync();

            return View(@event);
        }
        [Area("Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Speakers = await _context.Speakers.ToListAsync();
            return View();
        }
        [HttpPost]
        [Area("Admin")]
        [ActionName("Create")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(EventViewModel eventViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(eventViewModel);
            }
            if (!eventViewModel.Image.CheckFileFormat("image"))
            {
                ModelState.AddModelError("Image", "Sellect Correct Format!");
                return View(eventViewModel);
            }
            if (!eventViewModel.Image.CheckFileLength(300))
            {
                ModelState.AddModelError("Image", "Size Must be less than 300 kb");
                return View(eventViewModel);
            }
            if (eventViewModel.StartTime <= DateTime.Now.Date)
            {
                ModelState.AddModelError("Time", "Please select a future time.");
                return View(eventViewModel);
            }
            if (eventViewModel.EndTime <= DateTime.Now.Date)
            {
                ModelState.AddModelError("Time", "Please select a future time.");
                return View(eventViewModel);
            }
            if ( eventViewModel.StartTime >= eventViewModel.EndTime)
            {
                ModelState.AddModelError("Time", "Please select a Proper time.");
                return View(eventViewModel);
            }
            string filePath = await eventViewModel.Image.CopyFileAsync(_env.WebRootPath, "assets", "img", "event");
            var eventEntity = new Event
            {
                StartTime = eventViewModel.StartTime,
                EndTime = eventViewModel.EndTime,
                Title = eventViewModel.Title,
                Location = eventViewModel.Location,
                EventDetail = new EventDetail
                {
                    ImagePath = filePath,
                    Title = eventViewModel.EventDetailTitle,
                    Description = eventViewModel.EventDetailDescription
                }
            };
            await _context.EventDetails.AddAsync(eventEntity.EventDetail);
                await _context.Events.AddAsync(eventEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

    }
}