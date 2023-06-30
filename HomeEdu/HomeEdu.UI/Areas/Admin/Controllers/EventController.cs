using AutoMapper;
using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> Index(int Id)
        {
            var @event = await _context.Events.FindAsync(Id);
            var eventDetails = await _context.EventDetails
                .Where(e => e.EventId == Id)
                .ToListAsync();
            var speakers = await _context.Speakers
       .Include(s => s.EventDetail)
       .Where(s => s.EventDetail.EventId == Id)
       .ToListAsync();

            var eventDetailVM = new EventDetailViewModel
            {
                Event = @event,
                eventDetails = eventDetails,
                Speakers = speakers
            };

            return View(eventDetailVM);
        }
    }
}