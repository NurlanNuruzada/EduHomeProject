using AutoMapper;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.Areas.Admin.ViewModels.EventViewModel;
using HomeEdu.UI.Areas.Admin.ViewModels.SliderViewModel;
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
            Event @event = await _context.Events
                .Where(e => e.Id == Id)
                .Include(e => e.EventDetail)
                .ThenInclude(ed => ed.Speakers)
                .FirstOrDefaultAsync();

            return View(@event);
        }
        [Area("Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(EventViewModel eventViewModel)
        {
            if (ModelState.IsValid)
            {
                var eventEntity = _mapper.Map<Event>(eventViewModel);
                eventEntity.EventDetail = _mapper.Map<EventDetail>(eventViewModel);
                return Json(eventEntity, eventEntity.EventDetail);
                await _context.EventDetails.AddAsync(eventEntity.EventDetail);
                await _context.Events.AddAsync(eventEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(eventViewModel);
        }

    }
}