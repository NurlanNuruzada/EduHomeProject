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
using System.Text.Json.Serialization;
using System.Text.Json;

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
            if (eventViewModel.StartTime >= eventViewModel.EndTime)
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
        [Area("Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }
        [HttpPost]
        [Area("Admin")]
        [ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //var EventViewModel = new EventViewModel
        //{
        //    Title = @event.Title,
        //    StartTime = @event.StartTime,
        //    EndTime = @event.EndTime,
        //    Location = @event.Location,
        //    EventDetailDescription = @event.EventDetail.Description,
        //    EventDetailTitle = @event.EventDetail.Title
        //};
        [HttpGet]
        [Area("Admin")]
        public async Task<IActionResult> Update(int id)
        {
            Event @event = await _context.Events
                .Where(e => e.Id == id)
                .Include(e => e.EventDetail) 
                .FirstOrDefaultAsync();

            if (@event == null)
            {
                return NotFound();
            }

            if (@event.EventDetail == null)
            {
                @event.EventDetail = new EventDetail();
            }

            var eventViewModel = _mapper.Map<EventViewModel>(@event);
            eventViewModel.EventDetailDescription = @event.EventDetail.Title;
            eventViewModel.EventDetailTitle = @event.EventDetail.Description;
            return View(eventViewModel);
        }


        [Area("Admin")]
        [HttpPost]
        [ActionName("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, EventViewModel eventViewModel)
        {
            if (eventViewModel == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                return View(eventViewModel);
            }
            Event @event = _context.Events.Include(e => e.EventDetail).FirstOrDefault(e => e.Id == Id);
            if (@event == null)
            {
                return NotFound();
            }

            if (@event.EventDetail == null)
            {
                @event.EventDetail = new EventDetail();
            }
            @event.EventDetail.Title = eventViewModel.EventDetailDescription; 
            @event.EventDetail.Description = eventViewModel.EventDetailTitle;
            if (@event.EventDetail == null)
            {
                return Json(eventViewModel);
            }
            if (eventViewModel.Image != null)
            {
                if (!eventViewModel.Image.CheckFileFormat("image"))
                {
                    ModelState.AddModelError("Image", "Select Correct Format!");
                    return View(eventViewModel);
                }

                if (!eventViewModel.Image.CheckFileLength(300))
                {
                    ModelState.AddModelError("Image", "Size must be less than 300 KB");
                    return View(eventViewModel);
                }

                string filePath = await eventViewModel.Image.CopyFileAsync(_env.WebRootPath, "assets", "img", "slider");
                @event.EventDetail.ImagePath = filePath;
            }
            @event.Title = eventViewModel.Title;
            @event.StartTime = eventViewModel.StartTime;
            @event.EndTime = eventViewModel.EndTime;
            @event.Location = eventViewModel.Location;
            @event.EventDetail.Description = eventViewModel.EventDetailDescription;
            @event.EventDetail.Title = eventViewModel.EventDetailTitle;
            _context.EventDetails.Update(@event.EventDetail);
            _context.Events.Update(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}